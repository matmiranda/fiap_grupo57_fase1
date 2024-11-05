using fiap_grupo57_fase1.Models.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiap_grupo57_fase1_test.Models
{
    [TestFixture]
    public class ContatosPostRequestTests
    {
        [Test]
        public void ContatosPostRequest_ValidModel_DoesNotReturnValidationErrors()
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

            // Act
            var validationResults = ValidateModel(contatoRequest);

            // Assert
            Assert.That(validationResults, Is.Empty);
        }

        [Test]
        public void ContatosPostRequest_InvalidModel_ReturnsValidationErrors()
        {
            // Arrange
            var contatoRequest = new ContatosPostRequest
            {
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
            Assert.That(validationResults.Count, Is.EqualTo(3));
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
