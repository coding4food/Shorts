using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shorts;
using Shorts.Controllers;
using Shorts.Domain;
using System.Threading.Tasks;
using System.Threading;

namespace Shorts.Tests.Controllers
{
    [TestClass]
    public class UrlControllerTest
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
        public async Task GetById_Returns_BadRequest_For_Invalid_Id()
        {
            // Act
            var result = await controller.Get("50-0");

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public async Task GetById_Returns_NotFound_For_Nonexisting_Id()
        {
            // Act
            var result = await controller.Get("AbCd5");

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task GetById_Returns_Redirect_For_Existing_Id()
        {
            var url = "http://google.com";
            var shortId = "AbCd5";
            var shortUrl = new ShortUrl(url) { ShortUrlId = 100, Short = "AbCd5" };

            Mock.Get(context.ShortUrl).Setup(_ => _.Find(It.IsAny<long>())).Returns(shortUrl);

            // Act
            var result = await controller.Get(shortId);

            // Assert
            result.Should().BeOfType<RedirectResult>();
            result.As<RedirectResult>().Location.Should().Be(new Uri(url));
        }

        [TestMethod]
        public async Task GetById_Increments_Click_Count_For_Existing_Id()
        {
            var url = "http://google.com";
            var shortId = "AbCd5";
            var shortUrl = new ShortUrl(url) { ShortUrlId = 100, Short = "AbCd5" };
            var clickCount = shortUrl.Clicks;

            Mock.Get(context.ShortUrl).Setup(_ => _.Find(It.IsAny<long>())).Returns(shortUrl);

            // Act
            var result = await controller.Get(shortId);

            // Assert
            result.Should().BeOfType<RedirectResult>();
            result.As<RedirectResult>().Location.Should().Be(new Uri(url));
            shortUrl.Clicks.Should().Be(clickCount + 1);

            Mock.Get(context).Verify(_ => _.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task Post_Returns_BadRequest_For_Invalid_Url()
        {
            // Act
            var result = await controller.Post("value");

            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task Post_Returns_BadRequest_For_Empty_Url()
        {
            // Act
            var result = await controller.Post("");

            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task Post_Returns_Created_For_Valid_Url()
        {
            // Act
            var result = await controller.Post("http://google.com");

            // Assert
            result.Should().BeOfType<CreatedAtRouteNegotiatedContentResult<string>>();
        }
    }
}
