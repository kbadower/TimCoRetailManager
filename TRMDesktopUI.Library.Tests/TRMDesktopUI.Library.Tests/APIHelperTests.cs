using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUILibrary.Api;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUI.Library.Tests
{
    public class APIHelperTests
    {
        private IAPIHelper _sut;
        private readonly ILoggedInUserModel _loggedInUserModel = Substitute.For<ILoggedInUserModel>();
        private static Dictionary<string,string> apiHelperConfig = new Dictionary<string, string>
            {
                {"api", "https://localhost/dummyApi"},
            };

        private IConfiguration _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(apiHelperConfig)
            .Build();

        public APIHelperTests()
        {
            _sut = new APIHelper(_loggedInUserModel, _configuration);
        }

        [Test]
        public void InitializeClient_ShouldCreateHttpClient_WithCorrectValues()
        {
            // Arrange
            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            var uri = new Uri(apiHelperConfig["api"]);

            var expected = new HttpClient
            {
                BaseAddress = uri,
                DefaultRequestHeaders =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };

            // Act
            _sut.InitializeClient();

            // Assert
            Assert.That(_sut.ApiClient.BaseAddress, Is.EqualTo(expected.BaseAddress));
            Assert.That(_sut.ApiClient.DefaultRequestHeaders.Accept, Is.EqualTo(expected.DefaultRequestHeaders.Accept));
        }

        [Test]
        public void LogOffUser_ShouldClearRequestHeaders()
        {
            // Arrange
            _sut.InitializeClient();

            // Act
            _sut.LogOffUser();

            // Assert
            Assert.That(_sut.ApiClient.DefaultRequestHeaders.Accept, Is.Empty);

        }
    }
}
