using fiap_grupo57_fase1.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace fiap_grupo57_fase1.Infrastructures.Data
{
    public class ContatoContext : DbContext
    {
        public ContatoContext(DbContextOptions<ContatoContext> options) : base(options) { }

        public DbSet<ContatosPostRequest> Contatos { get; set; }

    }
}
