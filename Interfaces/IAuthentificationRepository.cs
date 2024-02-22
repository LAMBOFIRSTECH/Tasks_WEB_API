namespace Tasks_WEB_API.Interfaces
{
    public interface IAuthentificationRepository
    {
        	bool BasicAuthentification(string username,string password);
        	Task TokenAuthentification();
    }
}