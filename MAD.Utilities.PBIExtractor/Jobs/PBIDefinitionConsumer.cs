using MAD.Utilities.PBIExtractor.Services;

namespace MAD.Utilities.PBIExtractor.Jobs
{
    public class PBIDefinitionConsumer
    {
        private readonly PowerBIClientFactory powerBIClientFactory;

        public PBIDefinitionConsumer(PowerBIClientFactory powerBIClientFactory)
        {
            this.powerBIClientFactory = powerBIClientFactory;
        }

        public async Task ConsumeDefinitions()
        {
            using var powerBi = await this.powerBIClientFactory.Create();

            //var groupsResult = await powerBi.Groups.GetGroupsAsAdminWithHttpMessagesAsync(10);
            //var groups = groupsResult.Body;

            //var datasetsResult = await powerBi.Datasets.GetDatasetsInGroupAsAdminWithHttpMessagesAsync(groups.Value.FirstOrDefault().Id);
            //var datasets = datasetsResult.Body;

            //var reportsResult = await powerBi.Reports.GetReportsInGroupWithHttpMessagesAsync(groups.Value.FirstOrDefault().Id);
            //var fuck = reportsResult.Body;                       
        }
    }
}
