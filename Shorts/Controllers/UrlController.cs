using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Data.Entity;
using Shorts.Domain;

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
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }
    }
}
