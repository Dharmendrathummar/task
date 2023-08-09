using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using task.data.Essentials;
using task.data.Global.Helpers;
using task.data.Essentials;
using task.data.Model;

namespace task.data.Access
{
    public class DataToken
    {
        public static AuthTokenJ GetToken(CustomerJ customerJ)
        {
            try
            {
               
                DateTime CurrentTime = DataDateTime.Now;
                DateTime NotBefore = CurrentTime;
                DateTime Expiry = CurrentTime.AddMinutes(GlobalVariable.TokenExpiry);
                var key = Encoding.ASCII.GetBytes(GlobalVariable.JwtSecret);
                var claims = new List<Claim>();
                claims.Add(new Claim("id", customerJ.Id.ToString()));

                var token = new JwtSecurityToken(
                    GlobalVariable.JwtIssuer,
                    "task",
                    claims,
                    NotBefore,
                    Expiry,
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

                var FinalTooken = new JwtSecurityTokenHandler().WriteToken(token);

                AuthTokenJ auth = new AuthTokenJ()
                {
                    Token = FinalTooken,
                    RefreshToken = GenerateRefreshToken(),
                    IsDone = true
                };
                return auth;
            }
            catch (Exception ex)
            {
                return new AuthTokenJ() { IsDone = false };
            }
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public static CustomerSession IsJWTTokenValid(string token)
        {
            DateTime CurrentTime = DataDateTime.Now;
            CustomerSession result = new CustomerSession() { IsValid = false, Claims = new List<exParameter>() };
            try
            {
                string encodedJwt = token;
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwt = handler.ReadJwtToken(encodedJwt);
                if (jwt.Header.Alg == SecurityAlgorithms.HmacSha256)
                {
                    string claims = "";
                    foreach (Claim claim in jwt.Claims)
                    {
                        claims = claims + "{" + claim.Type + ":" + claim.Value + "},";
                        result.Claims.Add(new exParameter() { Name = claim.Type, Value = claim.Value });
                    }
                    result.TokenExpiry = jwt.ValidTo;
                    if (jwt.ValidTo > CurrentTime && jwt.ValidFrom < CurrentTime)
                    {
                        if (jwt.Audiences != null && jwt.Audiences.Contains("task"))
                        {
                            SecurityToken jwttoken;
                            var parameters = new TokenValidationParameters
                            {
                                ValidIssuer = GlobalVariable.JwtIssuer,
                                ValidAudience = "task",
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalVariable.JwtSecret)),
                                ValidateIssuerSigningKey = true,
                            };
                            ClaimsPrincipal principal = handler.ValidateToken(encodedJwt, parameters, out jwttoken);
                            result.IsValid = true;
                        }
                    }
                }
            }
            catch
            {
                result.IsValid = false;
            }

            return result;
        }
    }
}
