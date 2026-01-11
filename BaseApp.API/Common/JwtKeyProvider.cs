using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text.Json;

namespace BaseApp.API.Common
{
    public static class JwtKeyProvider
    {
        public static async Task<RsaSecurityKey> GetSigningKeyAsync()
        {
            using var httpClient = new HttpClient();
            var jwksJson = await httpClient.GetStringAsync("http://localhost:7000/.well-known/openid-configuration/jwks");
            var jwks = JsonDocument.Parse(jwksJson);
            var key = jwks.RootElement.GetProperty("keys")[0];

            var rsa = new RSAParameters
            {
                Modulus = Base64UrlEncoder.DecodeBytes(key.GetProperty("n").GetString()),
                Exponent = Base64UrlEncoder.DecodeBytes(key.GetProperty("e").GetString())
            };

            var rsaKey = new RsaSecurityKey(rsa)
            {
                KeyId = key.GetProperty("kid").GetString()
            };

            return rsaKey;
        }
    }
}
