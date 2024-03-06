using System.Security.Cryptography;
namespace Tasks_WEB_API.Authentifications
{
    public class RandomUserSecret
	{
		public string GenerateRandomKey(int length)
		{
			string refreshToken = "";
			var randomNumber = new byte[length];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				refreshToken = Convert.ToBase64String(randomNumber);
			}
			return refreshToken;
		}
	}
}