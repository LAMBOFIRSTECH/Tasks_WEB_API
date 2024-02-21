namespace Tasks_WEB_API.Interfaces
{
    public interface IAuthentificationRepository
    {
        	Task BasicAuthentification(string username,string password);
        	Task TokenAuthentification();
    }
}