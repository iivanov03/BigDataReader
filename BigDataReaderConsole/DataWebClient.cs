using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using System;

namespace BigDataReaderConsole
{
    public static class DataWebClient
    {
        private const string BaseApiUrl = "http://localhost:5177";
        private static string JwtToken;
        private static readonly HttpClient Client = new HttpClient();

        public static async Task<OrganizationModel> FetchOrganizationByIdAsync(string organizationId)
        {
            var url = $"{BaseApiUrl}/organization/{organizationId}";
            return await FetchFromApiAsync<OrganizationModel>(url);
        }

        public static async Task<List<OrganizationModel>> FetchTopOrganizationsAsync()
        {
            var url = $"{BaseApiUrl}/organization/top-organizations";
            return await FetchFromApiAsync<List<OrganizationModel>>(url);
        }

        public static async Task<List<IndustryEmployeesModel>> FetchEmployeeCountByIndustryAsync()
        {
            var url = $"{BaseApiUrl}/organization/employee-by-industry";
            return await FetchFromApiAsync<List<IndustryEmployeesModel>>(url);
        }

        public static async Task<bool> UploadOrganizationDataAsync()
        {
            var filePath = Path.Combine("data", "data.csv");
            var url = $"{BaseApiUrl}/organization/upload";

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var records = csv.GetRecords<OrganizationModel>().ToList();

            return await SendToApiAsync(url, records);
        }

        public static async Task DeleteOrganizationByIdAsync(string organizationId)
        {
            var url = $"{BaseApiUrl}/organization/{organizationId}";
            await SendDeleteRequestAsync(url);
        }

        public static async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var authUrl = $"{BaseApiUrl}/account/login";
            var success = await AuthenticateAtApiAsync(authUrl, username, password);
            if (success)
            {
                SetAuthorizationHeader();
            }
            return success;
        }

        private static async Task<T> FetchFromApiAsync<T>(string url)
        {
            try
            {
                return await Client.GetFromJsonAsync<T>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return default;
            }
        }

        private static async Task<bool> SendToApiAsync<T>(string url, T content)
        {
            try
            {
                var response = await Client.PostAsJsonAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending data: {ex.Message}");
                return false;
            }
        }

        private static async Task<bool> AuthenticateAtApiAsync(string url, string username, string password)
        {
            try
            {
                var response = await Client.PostAsJsonAsync(url, new { username, password });
                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
                    JwtToken = authResponse?.Token ?? throw new InvalidOperationException("Failed to retrieve token");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication failed: {ex.Message}");
                return false;
            }
        }

        private static async Task SendDeleteRequestAsync(string url)
        {
            try
            {
                var response = await Client.DeleteAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Organization not found or deletion failed");
                }
                else
                {
                    Console.WriteLine("Organization deleted");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error in deletion: {ex.Message}");
            }
        }

        private static void SetAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(JwtToken))
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            }
        }
    }
}