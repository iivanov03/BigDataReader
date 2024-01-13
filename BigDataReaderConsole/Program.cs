using System.Text;
using BigDataReaderConsole;

bool running = true;

var sb = new StringBuilder();
sb.AppendLine();
sb.AppendLine("Choose operation:");
sb.AppendLine("1. Authenticate User");
sb.AppendLine("2. Upload Organization Data");
sb.AppendLine("3. Fetch Organization by ID");
sb.AppendLine("4. Fetch Top Organizations");
sb.AppendLine("5. Fetch Employee Count by Industry");
sb.AppendLine("6. Delete Organization");
sb.AppendLine("7. Close");

while (running)
{
    Console.WriteLine(sb.ToString());
    Console.Write("Enter your choice: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await AuthenticateUserAsync();
            break;
        case "2":
            await UploadOrganizationDataAsync();
            break;
        case "3":
            await FetchOrganizationByIdAsync();
            break;
        case "4":
            await FetchTopOrganizationsAsync();
            break;
        case "5":
            await FetchEmployeeCountByIndustryAsync();
            break;
        case "6":
            await DeleteOrganizationByIdAsync();
            break;
        case "7":
            running = false;
            break;
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}

static async Task AuthenticateUserAsync()
{
    Console.Write("Enter username: ");
    var username = Console.ReadLine();
    Console.Write("Enter password: ");
    var password = Console.ReadLine();

    var isAuthenticated = await DataWebClient.AuthenticateUserAsync(username, password);
    if (isAuthenticated)
    {
        Console.WriteLine("Authentication successful.");
    }
    else
    {
        Console.WriteLine("Authentication failed. Please check your username and password.");
    }
}


static async Task UploadOrganizationDataAsync()
{
    var result = await DataWebClient.UploadOrganizationDataAsync();
    Console.WriteLine(result ? "Organization data uploaded successfully." : "Failed to upload data.");
}

static async Task FetchOrganizationByIdAsync()
{
    Console.Write("Enter Organization ID: ");
    var organizationId = Console.ReadLine();

    var organization = await DataWebClient.FetchOrganizationByIdAsync(organizationId);
    if (organization != null)
    {
        Console.WriteLine($"Organization Name: {organization.Name}");
    }
    else
    {
        Console.WriteLine("Organization not found or an error occurred.");
    }
}

static async Task FetchTopOrganizationsAsync()
{
    var organizations = await DataWebClient.FetchTopOrganizationsAsync();
    if (organizations != null)
    {
        foreach (var org in organizations)
        {
            Console.WriteLine(org.Name);
        }
    }
    else
    {
        Console.WriteLine("Failed to fetch top organizations.");
    }
}

static async Task FetchEmployeeCountByIndustryAsync()
{
    var industries = await DataWebClient.FetchEmployeeCountByIndustryAsync();
    if (industries != null)
    {
        foreach (var industry in industries)
        {
            Console.WriteLine($"{industry.Industry} - Employees: {industry.NumberOfEmployees}");
        }
    }
    else
    {
        Console.WriteLine("Failed to fetch employee count by industry.");
    }
}

static async Task DeleteOrganizationByIdAsync()
{
    Console.Write("Enter OrganizationId: ");
    var organizationId = Console.ReadLine();
    await DataWebClient.DeleteOrganizationByIdAsync(organizationId);
    Console.WriteLine("Delete operation attempted.");
}
