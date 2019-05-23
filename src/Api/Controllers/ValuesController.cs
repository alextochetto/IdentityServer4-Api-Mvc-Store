using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //return new string[] { "value1", "value2" };
            var claims = User.Claims.Select(x => $"{x.Type}:{x.Value}");

            return Ok(new { message = "Hello Claims", claims = claims.ToArray() });
        }

        // GET api/values/5
        [HttpGet("{thumbprint}")]
        public dynamic Get(string thumbprint)
        {
            var certSubject = string.Empty;
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var store2 = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

            foreach (var item in store2)
            {
                certSubject = certSubject + item.Subject;
            }

            return new { sub = certSubject, store = store2.Count };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
