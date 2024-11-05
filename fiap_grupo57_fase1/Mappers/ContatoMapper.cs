using fiap_grupo57_fase1.Models.Entities;
using fiap_grupo57_fase1.Models.Enums;
using fiap_grupo57_fase1.Models.Requests;

namespace fiap_grupo57_fase1.Mappers
{
    public static class ContatoMapper
    {
        public static ContatoEntity ToEntity(ContatosPostRequest request)
        {
            return new ContatoEntity
            {
                Nome = request.Nome,
                Telefone = request.Telefone,
                Email = request.Email,
                DDD = request.DDD,
                Regiao = (RegiaoEnum)Enum.Parse(typeof(RegiaoEnum), request.Regiao)
            };
        }

        public static ContatoEntity ToEntity(ContatosPutRequest request)
        {
            return new ContatoEntity
            {
                Id = request.Id,
                Nome = request.Nome,
                Telefone = request.Telefone,
                Email = request.Email,
                DDD = request.DDD,
                Regiao = (RegiaoEnum)Enum.Parse(typeof(RegiaoEnum), request.Regiao)
            };
        }
    }
}
