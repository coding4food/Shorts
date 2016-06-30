using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shorts.Domain;

namespace Shorts.Domain.Tests
{
    [TestClass]
    public class UrlShorteningServiceTests
    {
        private ShortsContext context;
        private UrlShorteningService sut;

        [TestInitialize]
        public void Setup()
        {
            var shortUrls = Mock.Of<System.Data.Entity.DbSet<ShortUrl>>();

            context = Mock.Of<ShortsContext>(c => c.ShortUrl == shortUrls);
            sut = new UrlShorteningService(context);
        }

        [TestMethod]
        public async Task UrlShorteningService_Shorten_Should_Save_Shortened_Url()
        {
            var url = "http://google.com";

            await sut.Shorten(url);

            Mock.Get(context.ShortUrl).Verify(_ => _.Add(It.Is<ShortUrl>(u => u.Url == url)), Times.Once());

            Mock.Get(context).Verify(_ => _.SaveChangesAsync(), Times.Exactly(2));
        }

        [TestMethod]
        public void UrlShorteningService_Shorten_Should_Throw_For_Empty_Url()
        {
            var url = "";

            sut.Awaiting(_ => _.Shorten(url))
                .ShouldThrow<ArgumentNullException>()
                .Where(ex => ex.Message.Contains("url"));
        }

        [TestMethod]
        public void UrlShorteningService_Shorten_Should_Throw_For_Invalid_Url()
        {
            var url = "foobar";

            sut.Awaiting(_ => _.Shorten(url))
                .ShouldThrow<ArgumentException>()
                .Where(ex => ex.Message.Contains("url"));
        }
    }
}
