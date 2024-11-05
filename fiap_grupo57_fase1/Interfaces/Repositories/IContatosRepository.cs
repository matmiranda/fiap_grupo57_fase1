using fiap_grupo57_fase1.Models.Entities;
using fiap_grupo57_fase1.Models.Enums;
using fiap_grupo57_fase1.Models.Requests;
using fiap_grupo57_fase1.Models.Responses;

namespace fiap_grupo57_fase1.Interfaces.Repositories
{
    public interface IContatosRepository
    {
        Task<ContatosGetResponse> ObterContatoPorId(int id);
        Task<int> Adicionar(ContatoEntity contato);
        Task<bool> ContatoExiste(ContatosPostRequest contato);
        Task<bool> ContatoExistePorId(int id);
        List<ContatosGetResponse> ObterPorDDD(int ddd);
        List<ContatosGetResponse> ObterPorDDDRegiao(int ddd, RegiaoEnum regiao);
        Task Atualizar(ContatoEntity contato);
        Task Excluir(int id);
    }
}
