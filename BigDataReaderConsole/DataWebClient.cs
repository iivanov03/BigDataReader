using CsvHelper.Configuration;
using CsvHelper;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;

namespace BigDataReaderConsole
{
    public static class DataWebClient
    {
        private const string _baseApiUrl = "http://localhost:5177";

        public static async Task<bool> UploadRecords()
        {
            var filePath = Path.Combine("data", "data.csv");
            var url = _baseApiUrl + "/organization/upload";

            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    var records = csv.GetRecords<OrganizationModel>().ToList();
                    using (var client = new HttpClient())
                    {
                        var json = JsonConvert.SerializeObject(records);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(url, content);

                        return response.IsSuccessStatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
                return false;
            }
        }
    }
}
