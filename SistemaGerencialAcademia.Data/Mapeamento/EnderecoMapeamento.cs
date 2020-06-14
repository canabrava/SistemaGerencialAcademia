using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGerencialAcademia.Dominio.Entidades;

namespace SistemaGerencialAcademia.Data.Mapeamento
{
    public class EnderecoMapeamento : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.ClienteId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(e => e.Rua)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Bairro)
                .IsRequired()
                .HasColumnType("varchar(15)");

            builder.Property(e => e.Numero)
                .IsRequired()
                .HasColumnType("varchar(5)");

            builder.Property(e => e.Complemento)
                .HasColumnType("varchar(5)");

            builder.ToTable("Enderecos");
        }
    }
}
