using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LunarDeckFoxyApi.Authentication
{
    public class JwtSecurityKey
    {
        private static IConfiguration _configuration;

        public JwtSecurityKey(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a JWT <c>SecutiryKey</c> from secret kept in config
        /// </summary>
        /// <returns><c>SecurityKey</c></returns>
        public static SecurityKey Create()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("Auth").GetSection("Key").Value));
        }
    }
}
