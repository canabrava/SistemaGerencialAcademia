using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGerencialAcademia.Dominio.Entidades;

namespace SistemaGerencialAcademia.Data.Mapeamento
{
    public class PeriodoDeFeriasMapeamento : IEntityTypeConfiguration<PeriodoDeFerias>
    {
        public void Configure(EntityTypeBuilder<PeriodoDeFerias> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ClientId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(p => p.DataInicio)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(p => p.DataFim)
                .IsRequired()
                .HasColumnType("datetime");

            builder.HasOne(p => p.Cliente)
                .WithMany(c => c.PeriodosDeFerias)
                .HasForeignKey(p => p.ClientId);

            builder.ToTable("PeriodosDeFerias");
        }
    }
}
