using System.IdentityModel.Tokens;
using System.Linq;

namespace OAuthDemo
{
    public static class SecurityKeyIdentifierExtension
    {
        public static string ToLongString(this SecurityKeyIdentifier identifier)
        {
            var clauses = identifier.Select(c =>
            {
                if (c.ClauseType == null || c.Id == null)
                {
                    return c.ToString();
                }
                else
                {
                    return $"{c.ClauseType}: {c.Id}";
                }
            });

            return $"[{string.Join(", ", clauses)}]";
        }
    }
}