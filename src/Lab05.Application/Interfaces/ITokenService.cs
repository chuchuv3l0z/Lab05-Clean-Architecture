namespace Lab05.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username, string role);
    }
}
