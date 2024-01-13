using BigDataReader.Models;

namespace BigDataReader.Services.OrganizationService
{
    public interface IOrganizationService
    {
        Task<OrganizationModel> GetAsync(string id);

        Task<IEnumerable<OrganizationModel>> GetOrganizationsWithMostEmployees();

        Task<IEnumerable<IndustryEmployeesModel>> AggregateEmployeeCountByIndustry();

        Task<bool> UploadAsync(List<OrganizationModel> model);
        Task<bool> DeleteAsync(string id);
    }
}