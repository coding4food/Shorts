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

        public UrlController() { }

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
        public async Task<IHttpActionResult> Get(string id)
        {
            if (ModelState.IsValid)
            {
                //Validate by regex

                try
                {
                    // Transactional
                    var shortUrl = new UrlShorteningService(context).GetUrl(id);

                    if (shortUrl == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        context.ShortUrl.Attach(shortUrl);
                        shortUrl.Clicks++;

                        await context.SaveChangesAsync();

                        return Redirect(shortUrl.Url);
                    }
                }
                catch (ArgumentException)
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        // POST api/values
        //[Route(@"api/url/{value:regex(^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$)}")]
        //[HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var shortUrl = await (new UrlShorteningService(context).Shorten(value));

                    var routeValues = new { controller = "url", id = shortUrl.Short };

                    return CreatedAtRoute("DefaultApi", routeValues, Url.Link("DefaultApi", routeValues));
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
