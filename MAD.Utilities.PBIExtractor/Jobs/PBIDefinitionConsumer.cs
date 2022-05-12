using MAD.Utilities.PBIExtractor.Services;
using Microsoft.PowerBI.Api.Models;

namespace MAD.Utilities.PBIExtractor.Jobs
{
    public class PBIDefinitionConsumer
    {
        private readonly PowerBIClientFactory powerBiClientFactory;

        public PBIDefinitionConsumer(PowerBIClientFactory powepowerBiClientFactoryrBiApi)
        {
            this.powerBiClientFactory = powepowerBiClientFactoryrBiApi;
        }

        public async Task ConsumeDefinitions()
        {
            // Get all workspaces from power bi
            // ignores all personal workspaces by default
            using var powerbi = await this.powerBiClientFactory.Create();

            var modifiedWorkspaces = await powerbi.WorkspaceInfo.GetModifiedWorkspacesWithHttpMessagesAsync(excludePersonalWorkspaces: true);

            if (modifiedWorkspaces.Body is null || modifiedWorkspaces.Body.Count == 0)
                return;

            // Initialize a metadata scan for each workspace
            var workspaceInfoScanRequest = await powerbi.WorkspaceInfo.PostWorkspaceInfoWithHttpMessagesAsync(datasourceDetails: true, datasetSchema: true, datasetExpressions: true, requiredWorkspaces: new RequiredWorkspaces
            {
                Workspaces = modifiedWorkspaces.Body.Select(x => x.Id as Guid?).ToList()
            });

            if (workspaceInfoScanRequest.Body is null)
                return;

            this.ThrowErrorIfNotNull(workspaceInfoScanRequest.Body.Error);

            // Check the status of the scan and continue once succeeded or throw an error
            var scanRequest = workspaceInfoScanRequest.Body;
            var scanStatus = scanRequest.Status;

            do
            {
                await Task.Delay(1000);

                var scanStatusResponse = await powerbi.WorkspaceInfo.GetScanStatusWithHttpMessagesAsync(scanRequest.Id.Value);

                this.ThrowErrorIfNotNull(scanStatusResponse.Body.Error);

                scanStatus = scanStatusResponse.Body.Status;

            } while (scanStatus != "Succeeded");

            // Get the list of measure definitions from the metadata scan
            var scanResultResponse = await powerbi.WorkspaceInfo.GetScanResultWithHttpMessagesAsync(scanRequest.Id.Value);

            if (scanResultResponse.Body is null || scanResultResponse.Body.Workspaces.Any() == false)
                return;

            var measures = from workspace in scanResultResponse.Body.Workspaces
                           from dataset in workspace.Datasets
                           from table in dataset.Tables
                           from measure in table.Measures
                           select measure;

            // Do stuff with measure
        }

        private void ThrowErrorIfNotNull(PowerBIApiErrorResponseDetail error)
        {
            if (error is null)
                return;

            throw new Exception($"Error occurred during metadata scan: Code - {error.Code}, Target - {error.Target}, Message - {error.Message}");
        }
    }
}
