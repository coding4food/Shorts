using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shorts;
using Shorts.Controllers;
using Shorts.Domain;
using System.Threading.Tasks;

namespace Shorts.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        private ShortsContext context;
        private UrlController controller;

        [TestInitialize]
        public void Setup()
        {
            var shortUrls = Mock.Of<DbSet<ShortUrl>>();
            context = Mock.Of<ShortsContext>(_ => _.ShortUrl == shortUrls);

            controller = new UrlController(context);
        }

        [TestMethod]
        public void Get()
        {
            var urls = new[]
            {
                new ShortUrl("http://google.com") { ShortUrlId = 1, Short = "1" },
                new ShortUrl("http://yandex.ru") { ShortUrlId = 2, Short = "2" }
            };

            // вот тут хорошо видно, как неудобно мокать и тестировать код, использующий EF напрямую

            // Arrange
            Mock.Get(context.ShortUrl).As<IEnumerable<ShortUrl>>()
                .Setup(_ => _.GetEnumerator())
                .Returns(urls.AsEnumerable().GetEnumerator());

            // Act
            var result = controller.Get();

            // Assert
            result.Select(_ => _.Url).ShouldBeEquivalentTo(urls.Select(_ => _.Url));
        }

        [TestMethod]
        public void GetById()
        {
            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Act
            controller.Post("value");

            // Assert
        }
    }
}
