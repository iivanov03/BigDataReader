using BigDataReader.Data;
using BigDataReader.Data.Entities;
using BigDataReader.Models;

using Microsoft.EntityFrameworkCore;

namespace BigDataReader.Services.OrganizationService
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationDbContext _context;

        private List<Country> _currentCountries;
        private List<Industry> _currentIndustries;

        public OrganizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrganizationModel> GetAsync(string id)
        {
            var model = await _context.Organizations
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new OrganizationModel()
                {
                    Name = x.Name,
                    Website = x.Website,
                    Country = x.Country.Name,
                    Description = x.Description,
                    Founded = x.Founded,
                    Industry = x.Industry.Name,
                    NumberOfEmployees = x.NumberOfEmployees,
                })
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<bool> UploadAsync(List<OrganizationModel> model)
        {
            await PreloadDataAsync();

            var organizationIds = model.Select(m => m.OrganizationId).ToList();
            var existingOrganizations = await _context
                .Organizations
                .Where(o => organizationIds.Contains(o.Id))
                .Select(o => o.Id)
                .ToListAsync();

            var organizationsToCreate = new List<Organization>();
            var organizationsToUpdate = new List<Organization>();

            foreach (var organization in model)
            {
                var entity = new Organization();
                entity.Id = organization.OrganizationId;
                entity.Name = organization.Name;
                entity.Website = organization.Website;
                entity.Description = organization.Description;
                entity.Founded = organization.Founded;
                entity.NumberOfEmployees = organization.NumberOfEmployees;

                var country = _currentCountries.FirstOrDefault(c => c.Name == organization.Country);
                if (country is null)
                {
                    country = new Country() { Name = organization.Country };
                    _currentCountries.Add(country);
                    await _context.Countries.AddAsync(country);
                }

                entity.Country = country;

                var industry = _currentIndustries.FirstOrDefault(i => i.Name == organization.Industry);
                if (industry is null)
                {
                    industry = new Industry() { Name = organization.Industry };
                    _currentIndustries.Add(industry);
                    await _context.Industries.AddAsync(industry);
                }

                entity.Industry = industry;

                if (existingOrganizations.Contains(organization.OrganizationId))
                {
                    organizationsToUpdate.Add(entity);
                }
                else
                {
                    organizationsToCreate.Add(entity);
                }
            }

            await _context.Organizations.AddRangeAsync(organizationsToCreate);
            _context.Organizations.UpdateRange(organizationsToUpdate);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Organizations.FindAsync(id);
            if (entity is null)
            {
                return false;
            }

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task PreloadDataAsync()
        {
            _currentCountries = await _context.Countries.ToListAsync();
            _currentIndustries = await _context.Industries.ToListAsync();
        }
    }
}