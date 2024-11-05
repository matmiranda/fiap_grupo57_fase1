using fiap_grupo57_fase1.Controllers;
using fiap_grupo57_fase1.Infrastructures.Excpetion;
using fiap_grupo57_fase1.Interfaces.Services;
using fiap_grupo57_fase1.Models.Requests;
using fiap_grupo57_fase1.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Net.Http;

namespace fiap_grupo57_fase1_test.Controllers
{
    [TestFixture]
    public class ContatosControllerTests : IDisposable
    {
        private Mock<IContatosService> _contatosServiceMock;
        private ContatosController _controller;
        private DefaultHttpContext _httpContext;

        [SetUp]
        public void SetUp()
        {
            _contatosServiceMock = new Mock<IContatosService>();
            _controller = new ContatosController(_contatosServiceMock.Object);
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();
        }

        [TearDown]
        public void TearDown()
        {
            _httpContext.Response.Body.Dispose();
        }

        [Test]
        public void GetContatos_ReturnsOkResult_WithListOfContatos()
        {
            // Arrange
            var contatos = new List<ContatosGetResponse>
            {
                new() {
                    Id = 1,
                    Nome = "João Silva",
                    Telefone = "123456789",
                    Email = "joao.silva@example.com",
                    DDD = 11,
                    Regiao = "Sudeste"
                }
            };
            _contatosServiceMock.Setup(service => service.ObterContatos(11, null)).Returns(contatos);

            // Act
            var result = _controller.GetContatos(11, null);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(contatos));
        }

        [Test]
        public async Task PostContato_ReturnsOkResult_WithContatoResponse()
        {
            // Arrange
            var contatoRequest = new ContatosPostRequest
            {
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 11,
                Regiao = "Sudeste"
            };
            var contatoResponse = new ContatosPostResponse { Id = 1 };
            _contatosServiceMock.Setup(service => service.AdicionarContato(contatoRequest)).ReturnsAsync(contatoResponse);

            // Act
            var result = await _controller.PostContato(contatoRequest);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(contatoResponse));
        }

        [Test]
        public async Task GetContatoById_ShouldReturnOk_WhenContatoExists()
        {
            // Arrange
            var contatoId = 1;
            var contatoResponse = new ContatosGetResponse { Id = contatoId, Nome = "Teste" };
            _contatosServiceMock.Setup(service => service.ObterContatoPorId(contatoId))
                               .ReturnsAsync(contatoResponse);

            // Act
            var result = await _controller.GetContatoById(contatoId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                Assert.That(okResult.Value, Is.EqualTo(contatoResponse));
            });
        }

        [Test]
        public async Task PutContato_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var contatoRequest = new ContatosPutRequest { Id = 1, Nome = "Teste Atualizado" };
            _contatosServiceMock.Setup(service => service.AtualizarContato(contatoRequest))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutContato(contatoRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteContato_ShouldReturnNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            var contatoId = 1;
            _contatosServiceMock.Setup(service => service.ExcluirContato(contatoId))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteContato(contatoId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        public void Dispose()
        {
            _httpContext.Response.Body?.Dispose();
        }
    }
}
