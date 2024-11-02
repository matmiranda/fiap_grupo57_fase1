using fiap_grupo58_fase1.Controllers;
using fiap_grupo58_fase1.Interfaces.Services;
using fiap_grupo58_fase1.Models.Requests;
using fiap_grupo58_fase1.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace fiap_grupo58_fase1_test.Controllers
{
    [TestFixture]
    public class ContatosControllerTests
    {
        private Mock<IContatosService> _contatosServiceMock;
        private ContatosController _controller;

        [SetUp]
        public void SetUp()
        {
            _contatosServiceMock = new Mock<IContatosService>();
            _controller = new ContatosController(_contatosServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            //_contatosServiceMock?.Dispose();
            _controller?.Dispose();
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
    }
}
