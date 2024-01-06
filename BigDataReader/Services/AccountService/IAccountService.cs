namespace BigDataReader.Services.AccountService
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(string username, string password);

        Task<string> LoginAsync(string username, string password);
    }
}