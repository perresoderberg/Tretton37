using System;
using Xunit;
using FluentAssertions;
using NSubstitute;
using Infrastructure.Shared;
using Microsoft.Extensions.Logging;

namespace Tretton37.Tests.HTTPClientServiceTests
{


    public class HTTPClientServiceTests
    {

        private readonly HTTPClientService _service;
        private readonly ILogger<HTTPClientService> _logger;


        public HTTPClientServiceTests()
        {
            _logger = Substitute.For<ILogger<HTTPClientService>>();
            _service = new HTTPClientService(_logger);
        }

        [Fact]
        public void RetreiveHyperLinksFromHtml_ReturValidHyperLinks()
        {
            //Arrange
            var testHtml = BaseTestClass.GetHTMLFile;

            // Act
            var response = _service.RetreiveHyperLinksFromHtml(testHtml);

            // Assert
            response.Should().NotBeNull()
                .And.HaveCount(35);
        }

        [Fact]
        public void RetreiveHyperLinksFromHtml_ReturExceptionIfEmpty()
        {
            //Arrange
            
            // Act
            Action act = () => _service.RetreiveHyperLinksFromHtml("");

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentNullException>(act);
        }

    }
}
