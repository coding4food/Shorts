using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Data.Entity;
using Shorts.Domain;
using System.ComponentModel.DataAnnotations;

namespace Shorts.Controllers
{
    public class UrlController : ApiController
    {
        // Should be injected
        private ShortsContext context = new ShortsContext();

        public UrlController(ShortsContext context)
        {
            this.context = context;
        }

        // GET api/values
        public IEnumerable<ShortUrl> Get()
        {
            // using async makes mocking EF hard
            return context.ShortUrl.ToArray();
        }

        // GET api/values/5
        public string Get(int id)
        {
            // 404 NotFound
            // 200 Ok

            return "value";
        }

        // POST api/values
        public async Task<IHttpActionResult> Post([FromBody]string value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var shortUrl = await (new UrlShorteningService(context).Shorten(value));

                    return CreatedAtRoute("Get", new { id = shortUrl.Short }, "");
                }
                catch (ArgumentNullException)
                {
                    return BadRequest("Invalid URL");
                }
                catch (ArgumentException)
                {
                    return BadRequest("Invalid URL");
                }
            }
            else
            {
                return BadRequest("Invalid URL");
            }
        }
    }
}
