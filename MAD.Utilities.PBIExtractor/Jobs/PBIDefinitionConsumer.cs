using MAD.Utilities.PBIExtractor.Api;
using MAD.Utilities.PBIExtractor.Api.Models;

namespace MAD.Utilities.PBIExtractor.Jobs
{
    public class PBIDefinitionConsumer
    {
        private readonly IPBIApi powerBiApi;

        public PBIDefinitionConsumer(IPBIApi powerBiApi)
        {
            this.powerBiApi = powerBiApi;
        }

        public async Task ConsumeDefinitions()
        {
            // Get all workspaces from power bi
            // ignores all personal workspaces by default
            var modifiedWorkspaces = await this.powerBiApi.GetModifiedWorkspacesAsync();

            if (modifiedWorkspaces.Content is null)
                return;

            // Initialize a metadata scan for each workspace
            var workspaceInfoScanRequest = await this.powerBiApi.PostWorkspaceInfoAsync(new WorkspaceInfoRequest
            {
                Workspaces = modifiedWorkspaces.Content.Select(x => x.Id)
            });

            if (workspaceInfoScanRequest.Content is null)
                return;

            this.ThrowErrorIfNotNull(workspaceInfoScanRequest.Content.Error);

            // Check the status of the scan and continue once succeeded or throw an error
            var scanRequest = workspaceInfoScanRequest.Content;
            var scanStatus = scanRequest.Status;

            do
            {
                await Task.Delay(1000);

                var scanStatusResponse = await this.powerBiApi.GetScanStatusAsync(scanRequest.Id);

                this.ThrowErrorIfNotNull(scanStatusResponse.Content.Error);

                scanStatus = scanStatusResponse.Content.Status;

            } while (scanStatus != "Succeeded");

            // Get the list of measure definitions from the metadata scan
            var scanResultResponse = await this.powerBiApi.GetScanResultAsync(scanRequest.Id);

            if (scanResultResponse.Content is null || scanResultResponse.Content.Workspaces.Any() == false)
                return;

            var measures = from workspace in scanResultResponse.Content.Workspaces
                           from dataset in workspace.Datasets
                           from table in dataset.Tables
                           from measure in table.Measures
                           select measure;

            // Do stuff with measure
        }

        private void ThrowErrorIfNotNull(PowerBIErrorResponseDetail error)
        {
            if (error is null)
                return;

            throw new Exception($"Error occurred during metadata scan: Code - {error.Code}, Target - {error.Target}, Message - {error.Message}");
        }
    }
}
