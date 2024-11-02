using fiap_grupo58_fase1.Models.Enums;

namespace fiap_grupo58_fase1.Models.Entities
{
    public class ContatoEntity
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Telefone { get; set; }
        public required string Email { get; set; }
        public int DDD { get; set; }
        public required RegiaoEnum Regiao { get; set; }
    }
}
