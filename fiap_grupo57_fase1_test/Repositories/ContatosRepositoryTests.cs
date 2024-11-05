using Dapper;
using fiap_grupo57_fase1.Interfaces.Dapper;
using fiap_grupo57_fase1.Models.Entities;
using fiap_grupo57_fase1.Models.Enums;
using fiap_grupo57_fase1.Models.Requests;
using fiap_grupo57_fase1.Models.Responses;
using fiap_grupo57_fase1.Repositories;
using Moq;
using System.Data;

namespace fiap_grupo57_fase1_test.Repositories
{
    [TestFixture]
    public class ContatosRepositoryTests
    {
        private Mock<IDbConnection> _dbConnectionMock;
        private Mock<IDapperWrapper> _dapperWrapperMock;
        private ContatosRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _dbConnectionMock = new Mock<IDbConnection>();
            _dapperWrapperMock = new Mock<IDapperWrapper>();
            _repository = new ContatosRepository(_dbConnectionMock.Object, _dapperWrapperMock.Object);
        }

        [Test]
        public async Task Adicionar_ContatoValido_RetornaId()
        {
            // Arrange
            var contato = new ContatoEntity { Nome = "João Silva", Telefone = "123456789", Email = "joao.silva@example.com", DDD = 11, Regiao = RegiaoEnum.Sudeste };
            var sql = @"INSERT INTO Contatos (Nome, Telefone, Email, DDD, Regiao) VALUES (@Nome, @Telefone, @Email, @DDD, @Regiao);SELECT LAST_INSERT_ID();";
            _dapperWrapperMock.Setup(d => d.QuerySingleAsync<int>(_dbConnectionMock.Object, sql, contato)).ReturnsAsync(1);

            // Act
            var result = await _repository.Adicionar(contato);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task ContatoExiste_ContatoExistente_RetornaTrue()
        {
            // Arrange
            var contato = new ContatosPostRequest { Email = "joao.silva@example.com" };
            var sql = "SELECT 1 FROM Contatos WHERE Email = @Email";
            _dapperWrapperMock.Setup(d => d.QueryFirstOrDefaultAsync<int>(_dbConnectionMock.Object, sql, It.IsAny<object>())).ReturnsAsync(1);

            // Act
            var result = await _repository.ContatoExiste(contato);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ContatoExistePorId_ContatoExistente_RetornaTrue()
        {
            // Arrange
            var id = 1;
            var sql = "SELECT 1 FROM Contatos WHERE Id = @Id";
            _dapperWrapperMock.Setup(d => d.QueryFirstOrDefaultAsync<int>(_dbConnectionMock.Object, sql, It.IsAny<object>())).ReturnsAsync(1);

            // Act
            var result = await _repository.ContatoExistePorId(id);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ObterPorDDD_ContatosExistentes_RetornaListaDeContatos()
        {
            // Arrange
            var ddd = 11;
            var sql = "SELECT c.Id, c.Nome, c.Telefone, c.Email, c.DDD, r.Nome AS Regiao FROM Contatos c JOIN Regioes r ON c.Regiao = r.Id WHERE c.DDD = @DDD";
            var contatos = new List<ContatosGetResponse>
            {
                new ContatosGetResponse { Id = 1, Nome = "João Silva", Telefone = "123456789", Email = "joao.silva@example.com", DDD = 11, Regiao = "Sudeste" }
            };
            //_dapperWrapperMock.Setup(d => d.Query<ContatosGetResponse>(_dbConnectionMock.Object, sql, new { DDD = ddd })).Returns(contatos);
            _dapperWrapperMock.Setup(d => d.Query<ContatosGetResponse>(_dbConnectionMock.Object, sql, It.IsAny<object>())).Returns(contatos);

            // Act
            var result = _repository.ObterPorDDD(ddd);

            // Assert
            Assert.That(result, Is.EqualTo(contatos));
        }

        [Test]
        public void ObterPorDDDRegiao_ContatosExistentes_RetornaListaDeContatos()
        {
            // Arrange
            var ddd = 11;
            RegiaoEnum regiao = RegiaoEnum.Sudeste;
            var sql = @"SELECT c.Id, c.Nome, c.Telefone, c.Email, c.DDD, r.Nome AS Regiao FROM Contatos c JOIN Regioes r ON c.Regiao = r.Id WHERE c.DDD = @DDD AND c.Regiao = @Regiao";
            var contatos = new List<ContatosGetResponse>
            {
                new ContatosGetResponse { Id = 1, Nome = "João Silva", Telefone = "123456789", Email = "joao.silva@example.com", DDD = 11, Regiao = "Sudeste" }
            };
            //_dapperWrapperMock.Setup(d => d.Query<ContatosGetResponse>(_dbConnectionMock.Object, sql, new { DDD = ddd, Regiao = regiao })).Returns(contatos);
            _dapperWrapperMock.Setup(d => d.Query<ContatosGetResponse>(_dbConnectionMock.Object, sql, It.IsAny<object>())).Returns(contatos);

            // Act
            var result = _repository.ObterPorDDDRegiao(ddd, regiao);

            // Assert
            Assert.That(result, Is.EqualTo(contatos));
        }
    }
}
