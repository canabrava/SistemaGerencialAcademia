using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGerencialAcademia.Dominio.Entidades;

namespace SistemaGerencialAcademia.Data.Mapeamento
{
    public class InformacoesPagamentoMapeamento : IEntityTypeConfiguration<InformacoesPagamento>
    {
        public void Configure(EntityTypeBuilder<InformacoesPagamento> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.ClienteId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(i => i.DataPagamento)
                .IsRequired()
                .HasColumnType("datetime");

            builder.HasOne(i => i.Cliente)
                .WithMany(c => c.InformacoesPagamentos)
                .HasForeignKey(i => i.ClienteId);

            builder.ToTable("InformacoesDePagamento");
        }
    }
}
