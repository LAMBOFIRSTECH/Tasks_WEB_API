using System.Security.Cryptography;
namespace Tasks_WEB_API
{
    public class RandomUserSecret
    {

        public  string GenerateRandomKey(int length)
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[length];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
    }

}
