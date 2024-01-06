using BigDataReader.Models;

namespace BigDataReader.Services.OrganizationService
{
    public interface IOrganizationService
    {
        Task<bool> UploadAsync(List<OrganizationModel> model);
    }
}