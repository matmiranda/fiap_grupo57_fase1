using fiap_grupo57_fase1.Models.Requests;
using fiap_grupo57_fase1.Models.Responses;

namespace fiap_grupo57_fase1.Interfaces.Services
{
    public interface IContatosService
    {
        Task<ContatosGetResponse> ObterContatoPorId(int id);
        Task<ContatosPostResponse> AdicionarContato(ContatosPostRequest contato);
        List<ContatosGetResponse> ObterContatos(int ddd, string? regiao);
        Task AtualizarContato(ContatosPutRequest contato);
        Task ExcluirContato(int id);
    }
}
