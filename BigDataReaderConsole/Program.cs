using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

using BigDataReaderConsole;

string filePath = "data" + "/" + "data.csv";

try
{
    using (var reader = new StreamReader(filePath))
    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
    {
        var records = csv.GetRecords<OrganizationModel>().ToList();
        foreach (var record in records)
        {
            Console.WriteLine($"Name: {record.Name}, Country: {record.Country}, Industry: {record.Industry}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred: " + ex.Message);
}
