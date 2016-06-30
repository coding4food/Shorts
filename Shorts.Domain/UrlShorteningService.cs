using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorts.Domain
{
    public class UrlShorteningService
    {
        private ShortsContext context;

        public UrlShorteningService(ShortsContext context)
        {
            this.context = context;
        }

        public async Task Shorten(string url)
        {
            Uri link;

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            // предположим, что сервис сокращает все типы URI
            if (!Uri.TryCreate(url, UriKind.Absolute, out link))
            {
                throw new ArgumentException(nameof(url));
            }

            var shortUrl = new ShortUrl(url);

            context.ShortUrl.Add(shortUrl);

            await context.SaveChangesAsync();

            // после сохранения ссылки в базе делаем ей короткий URL из идентификатора
            // !!! алгоритм укорачивания можно было бы инджектить, но для такого проекта это пока не нужно
            shortUrl.Short = Encoder.Encode(shortUrl.ShortUrlId);

            await context.SaveChangesAsync();
        }
    }
}
