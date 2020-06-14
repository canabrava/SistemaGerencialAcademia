using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGerencialAcademia.Dominio.Entidades;

namespace SistemaGerencialAcademia.Data.Mapeamento
{
    public class ClienteMapeamento : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.EnderecoId)
                .HasColumnType("uniqueidentifier");

            builder.Property(c => c.NumeroDeMatricula)
                .IsRequired()
                .HasColumnType("bigint");

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(c => c.CPF)
                .IsRequired()
                .HasColumnType("varchar(11)");

            builder.Property(c => c.Identidade)
                .IsRequired()
                .HasColumnType("varchar(11)");

            builder.Property(c => c.VencimentoPagamento)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(c => c.UltimaAvaliacaoFisica)
                .HasColumnType("datetime");

            builder.HasOne(c => c.Endereco)
                .WithOne(e => e.Cliente)
                .HasForeignKey<Endereco>(e => e.ClienteId);

            builder.ToTable("Clientes");

        }
    }
}
