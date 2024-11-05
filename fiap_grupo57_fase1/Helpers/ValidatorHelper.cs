using fiap_grupo57_fase1.Infrastructures.Excpetion;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace fiap_grupo57_fase1.Helpers
{
    public static class ValidatorHelper
    {
        public static void Validar(object obj)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, context, results, true);

            if (!isValid)
                throw new CustomException(HttpStatusCode.BadRequest, results.Select(validationResult => validationResult.ErrorMessage).FirstOrDefault());
        }
    }
}
