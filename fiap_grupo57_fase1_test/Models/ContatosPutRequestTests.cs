using fiap_grupo57_fase1.Models.Requests;
using System.ComponentModel.DataAnnotations;

namespace fiap_grupo57_fase1_test.Models
{
    [TestFixture]
    public class ContatosPutRequestTests
    {
        [Test]
        public void ContatosPutRequest_ValidModel_DoesNotReturnValidationErrors()
        {
            // Arrange
            var contatoRequest = new ContatosPutRequest
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 11,
                Regiao = "Sudeste"
            };

            // Act
            var validationResults = ValidateModel(contatoRequest);

            // Assert
            Assert.That(validationResults, Is.Empty);
        }

        [Test]
        public void ContatosPutRequest_InvalidModel_ReturnsValidationErrors()
        {
            // Arrange
            var contatoRequest = new ContatosPutRequest
            {
                Id = 0,
                Nome = "João",
                Telefone = "123",
                Email = "joao.silva@",
                DDD = 11,
                Regiao = "Sudeste"
            };

            // Act
            var validationResults = ValidateModel(contatoRequest);

            // Assert
            Assert.That(validationResults, Is.Not.Empty);
            Assert.That(validationResults.Count, Is.EqualTo(4));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
