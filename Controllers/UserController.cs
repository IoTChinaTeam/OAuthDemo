using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace OAuthDemo
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> GetDetails()
        {
            var result = new Dictionary<string, string>();

            var fields = typeof(ClaimTypes).GetFields().Where(f => f.IsPublic && f.IsStatic && f.FieldType == typeof(string));
            foreach (var field in fields)
            {
                var name = field.GetValue(null).ToString();
                var value = ClaimsPrincipal.Current.FindFirst(name)?.Value;

                if (value != null)
                {
                    result.Add(field.Name, value);
                }
            }

            return result;
        }
    }
}
