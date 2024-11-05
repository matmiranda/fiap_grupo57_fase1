using fiap_grupo57_fase1.Infrastructures.Excpetion;
using fiap_grupo57_fase1.Interfaces.Models.Utils;
using fiap_grupo57_fase1.Interfaces.Repositories;
using fiap_grupo57_fase1.Models.Entities;
using fiap_grupo57_fase1.Models.Enums;
using fiap_grupo57_fase1.Models.Requests;
using fiap_grupo57_fase1.Models.Responses;
using fiap_grupo57_fase1.Services;
using Moq;
using System.Net;

namespace fiap_grupo57_fase1_test.Services
{
    [TestFixture]
    public class ContatosServiceTests
    {
        private Mock<IContatosRepository> _mockContatosRepository;
        private Mock<IObterRegiaoPorDDD> _mockObterRegiaoPorDDD;
        private ContatosService _contatosService;

        [SetUp]
        public void Setup()
        {
            _mockContatosRepository = new Mock<IContatosRepository>();
            _mockObterRegiaoPorDDD = new Mock<IObterRegiaoPorDDD>();
            _contatosService = new ContatosService(_mockContatosRepository.Object, _mockObterRegiaoPorDDD.Object);
        }

        [Test]
        public async Task ObterContatoPorId_IdValido_DeveRetornarContato()
        {
            // Arrange
            var contatoId = 1;
            var contatoEsperado = new ContatosGetResponse { Id = contatoId };
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contatoId))
                .ReturnsAsync(true);
            _mockContatosRepository.Setup(repo => repo.ObterContatoPorId(contatoId))
                .ReturnsAsync(contatoEsperado);

            // Act
            var resultado = await _contatosService.ObterContatoPorId(contatoId);

            // Assert
            Assert.That(resultado, Is.EqualTo(contatoEsperado));
        }

        [Test]
        public void ObterContatos_RegiaoInvalida_DeveLancarExcecao()
        {
            // Arrange
            var ddd = 11;
            var regiao = "RegiaoInvalida";

            // Act & Assert
            var ex = Assert.Throws<CustomException>(() => _contatosService.ObterContatos(ddd, regiao));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(ex.Message, Is.EqualTo("A região fornecida é inválida."));
            });
        }

        [Test]
        public void ObterContatos_SemRegiao_DeveRetornarListaDeContatos()
        {
            // Arrange
            var ddd = 11;
            var contatosEsperados = new List<ContatosGetResponse> { new ContatosGetResponse { Id = 1 } };
            _mockContatosRepository.Setup(repo => repo.ObterPorDDD(ddd))
                .Returns(contatosEsperados);

            // Act
            var resultado = _contatosService.ObterContatos(ddd, null);

            // Assert
            Assert.That(resultado, Is.EqualTo(contatosEsperados));
        }

        [Test]
        public void ObterContatos_ComRegiao_DeveRetornarListaDeContatos()
        {
            // Arrange
            var ddd = 11;
            var regiao = "Sul";
            var contatosEsperados = new List<ContatosGetResponse> { new ContatosGetResponse { Id = 1 } };
            _mockContatosRepository.Setup(repo => repo.ObterPorDDDRegiao(ddd, RegiaoEnum.Sul))
                .Returns(contatosEsperados);

            // Act
            var resultado = _contatosService.ObterContatos(ddd, regiao);

            // Assert
            Assert.That(resultado, Is.EqualTo(contatosEsperados));
        }

        [Test]
        public void ObterContatos_SemContatos_DeveLancarExcecao()
        {
            // Arrange
            var ddd = 11;
            _mockContatosRepository.Setup(repo => repo.ObterPorDDD(ddd))
                .Returns(new List<ContatosGetResponse>());

            // Act & Assert
            var ex = Assert.Throws<CustomException>(() => _contatosService.ObterContatos(ddd, null));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(ex.Message, Is.EqualTo("Contato não encontrado"));
            });
        }

        [Test]
        public void ObterContatos_ComRegiaoSemContatos_DeveLancarExcecao()
        {
            // Arrange
            var ddd = 11;
            var regiao = "Sul";
            _mockContatosRepository.Setup(repo => repo.ObterPorDDDRegiao(ddd, RegiaoEnum.Sul))
                .Returns(new List<ContatosGetResponse>());

            // Act & Assert
            var ex = Assert.Throws<CustomException>(() => _contatosService.ObterContatos(ddd, regiao));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(ex.Message, Is.EqualTo("Contato não encontrado"));
            });
        }

        [Test]
        public void AdicionarContato_ContatoInvalido_DeveLancarExcecao()
        {
            // Arrange
            var contato = new ContatosPostRequest { DDD = 0, Nome = "Matheus Miranda", Telefone = "12345678", Email = "email" }; // Dados inválidos

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.AdicionarContato(contato));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(ex.Message, Is.EqualTo("O e-mail fornecido é inválido.")); // Mensagem de erro de validação
            });
        }

        [Test]
        public void AdicionarContato_ContatoJaExiste_DeveLancarExcecao()
        {
            // Arrange
            var contato = new ContatosPostRequest { DDD = 11, Nome = "Nome Sobrenome", Telefone = "12345678", Email = "teste@teste.com" };
            _mockContatosRepository.Setup(repo => repo.ContatoExiste(contato))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.AdicionarContato(contato));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
                Assert.That(ex.Message, Is.EqualTo("Contato com este email já existe."));
            });
        }

        [Test]
        public void AdicionarContato_DDDInvalido_DeveLancarExcecao()
        {
            // Arrange
            var contato = new ContatosPostRequest { DDD = 1, Nome = "Nome Sobrenome", Telefone = "12345678", Email = "teste@teste.com" };
            _mockContatosRepository.Setup(repo => repo.ContatoExiste(contato))
                .ReturnsAsync(false);
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contato.DDD))
                .Returns("DDD_INVALIDO");

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.AdicionarContato(contato));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(ex.Message, Is.EqualTo($"Região NÃO ENCONTRADA para o DDD: {contato.DDD}"));
            });
        }

        [Test]
        public async Task AdicionarContato_ContatoValido_DeveRetornarId()
        {
            // Arrange
            var contato = new ContatosPostRequest { DDD = 11, Nome = "Nome Sobrenome", Telefone = "12345678", Email = "teste@teste.com" };
            _mockContatosRepository.Setup(repo => repo.ContatoExiste(contato))
                .ReturnsAsync(false);
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contato.DDD))
                .Returns("Sul");
            _mockContatosRepository.Setup(repo => repo.Adicionar(It.IsAny<ContatoEntity>()))
                .ReturnsAsync(1);

            // Act
            var resultado = await _contatosService.AdicionarContato(contato);

            // Assert
            Assert.That(resultado.Id, Is.EqualTo(1));
        }

        [Test]
        public void AtualizarContato_ContatoInvalido_DeveLancarExcecao()
        {
            // Arrange
            var contato = new ContatosPutRequest { Id = 1, DDD = 0, Nome = "Nome", Telefone = "123", Email = "email" }; // Dados inválidos

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.AtualizarContato(contato));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(ex.Message, Is.EqualTo("O nome deve conter nome e sobrenome, separados por um espaço.")); // Mensagem de erro de validação
            });
        }

        [Test]
        public void AtualizarContato_IdInexistente_DeveLancarExcecao()
        {
            // Arrange
            var contato = new ContatosPutRequest { Id = 1, DDD = 11, Nome = "Nome Sobrenome", Telefone = "12345678", Email = "teste@teste.com" };
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contato.Id))
                .ReturnsAsync(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.AtualizarContato(contato));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(ex.Message, Is.EqualTo("O id do contato não existe."));
            });
        }

        [Test]
        public async Task AtualizarContato_ContatoValido_DeveAtualizarContato()
        {
            // Arrange
            var contato = new ContatosPutRequest { Id = 1, DDD = 63, Nome = "Nome Sobrenome", Telefone = "12345678", Email = "teste@teste.com" };
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contato.Id))
                .ReturnsAsync(true);
            _mockContatosRepository.Setup(repo => repo.ObterContatoPorId(contato.Id))
                .ReturnsAsync(new ContatosGetResponse { Id = contato.Id, DDD = 11 });
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contato.DDD))
                .Returns("Norte");
            _mockContatosRepository.Setup(repo => repo.Atualizar(It.IsAny<ContatoEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            await _contatosService.AtualizarContato(contato);

            // Assert
            _mockContatosRepository.Verify(repo => repo.Atualizar(It.IsAny<ContatoEntity>()), Times.Once);
        }


        [Test]
        public void AtualizarContato_DDDInvalido_DeveLancarExcecao()
        {
            // Arrange
            var contato = new ContatosPutRequest { Id = 1, DDD = 99, Nome = "Nome Sobrenome", Telefone = "12345678", Email = "teste@teste.com" };
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contato.Id))
                .ReturnsAsync(true);
            _mockContatosRepository.Setup(repo => repo.ObterContatoPorId(contato.Id))
                .ReturnsAsync(new ContatosGetResponse { Id = contato.Id, DDD = 11 });
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contato.DDD))
                .Returns("DDD_INVALIDO");

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.AtualizarContato(contato));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(ex.Message, Is.EqualTo($"Região NÃO ENCONTRADA para o DDD: {contato.DDD}"));
            });
        }

        [Test]
        public void ExcluirContato_IdInvalido_DeveLancarExcecao()
        {
            // Arrange
            var contatoId = 0;

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.ExcluirContato(contatoId));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(ex.Message, Is.EqualTo("O id deve ser maior que zero."));
            });
        }

        [Test]
        public void ExcluirContato_IdInexistente_DeveLancarExcecao()
        {
            // Arrange
            var contatoId = 1;
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contatoId))
                .ReturnsAsync(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.ExcluirContato(contatoId));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(ex.Message, Is.EqualTo("O id do contato não existe."));
            });
        }

        [Test]
        public async Task ExcluirContato_IdValido_DeveExcluirContato()
        {
            // Arrange
            var contatoId = 1;
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contatoId))
                .ReturnsAsync(true);
            _mockContatosRepository.Setup(repo => repo.Excluir(contatoId))
                .Returns(Task.CompletedTask);

            // Act
            await _contatosService.ExcluirContato(contatoId);

            // Assert
            _mockContatosRepository.Verify(repo => repo.Excluir(contatoId), Times.Once);
        }
    }
}
