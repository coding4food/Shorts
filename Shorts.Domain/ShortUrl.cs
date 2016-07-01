using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorts.Domain
{
    public class ShortUrl
    {
        public long ShortUrlId { get; set; }
        public string Url { get; set; }
        public string Short { get; set; }
        public int Clicks { get; set; } = 0;
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

        private ShortUrl() { }

        internal ShortUrl(string url)
        {
            this.Url = url;
        }
    }
}
