using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks_WEB_API.Interfaces
{
	public interface IJwtTokenService
	{
		string GetSigningKey();
		string GenerateJwtToken(string email);
	}
}