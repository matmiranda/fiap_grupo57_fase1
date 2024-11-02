using fiap_grupo58_fase1.Infrastructures.Excpetion;
using fiap_grupo58_fase1.Models.Responses;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Text.Json;

namespace fiap_grupo58_fase1_test.Infrastructures
{
    [TestFixture]
    public class ExceptionMiddlewareTests
    {
        private Mock<RequestDelegate> _nextMock;
        private DefaultHttpContext _httpContext;
        private ExceptionMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _nextMock = new Mock<RequestDelegate>();
            _httpContext = new DefaultHttpContext();
            _middleware = new ExceptionMiddleware(_nextMock.Object);
        }

        [Test]
        public async Task InvokeAsync_CustomException_ReturnsCustomErrorResponse()
        {
            // Arrange
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new CustomException(HttpStatusCode.BadRequest, "Custom error message"));

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.That(_httpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(_httpContext.Response.Body).ReadToEnd();
            var response = JsonSerializer.Deserialize<ExceptionResponse>(responseBody);
            Assert.That(response.Message, Is.EqualTo("Custom error message"));
        }

        [Test]
        public async Task InvokeAsync_GenericException_ReturnsInternalServerErrorResponse()
        {
            // Arrange
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new Exception("Generic error message"));

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(_httpContext.Response.Body).ReadToEnd();
            var response = JsonSerializer.Deserialize<ExceptionResponse>(responseBody);
            Assert.AreEqual("Generic error message", response.Message);
        }
    }
}
