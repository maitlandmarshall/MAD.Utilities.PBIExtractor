using MAD.Utilities.PBIExtractor.Api.Models;
using Refit;

namespace MAD.Utilities.PBIExtractor.Api
{
    [Headers("Authorizationi: Bearer")]
    public interface IPBIApi
    {
        [Get("/myorg/admin/workspaces/modified")]
        Task<ApiResponse<IEnumerable<ModifiedWorkspaceResult>>> GetModifiedWorkspacesAsync([Query("excludePersonalWorkspaces")] bool excludePersonalWorkspace = true);

        [Post("/myorg/admin/workspaces/getInfo")]
        Task<ApiResponse<ScanRequest>> PostWorkspaceInfoAsync([Body(BodySerializationMethod.Serialized)] WorkspaceInfoRequest workspaces,
                                                              [Query("datasourceDetails")] bool datasourceDetails = true,
                                                              [Query("datasetSchema")] bool datasetSchema = true,
                                                              [Query("datasetExpressions")] bool datasetExpressions = true);

        [Get("/myorg/admin/workspaces/scanStatus/{scanId}")]
        Task<ApiResponse<ScanRequest>> GetScanStatusAsync([AliasAs("scanId")] string scanId);

        [Get("/myorg/admin/workspaces/scanResult/{scanId}")]
        Task<ApiResponse<ScanResult>> GetScanResultAsync([AliasAs("scanId")] string scanId);
    }
}
