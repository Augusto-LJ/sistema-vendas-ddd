using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vendas.Domain.Pedidos;

namespace Vendas.Infrastructure.Persistence.Configurations;
public sealed class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.NumeroPedido)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.ClienteId)
            .IsRequired();

        builder.Property(p => p.StatusPedido)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.ValorTotal)
            .HasPrecision(18, 2);

        builder.Property(p => p.DataCriacao)
            .IsRequired();

        builder.Property(p => p.DataAtualizacao)
            .IsRequired(false);


        builder.OwnsOne(p => p.EnderecoEntrega, endereco =>
        {
            endereco.Property(e => e.Cep)
                .HasColumnName("Endereco_Cep")
                .HasMaxLength(9)
                .IsRequired();

            endereco.Property(e => e.Logradouro)
                .HasColumnName("Endereco_Logradouro")
                .HasMaxLength(200)
                .IsRequired();

            endereco.Property(e => e.Numero)
                .HasColumnName("Endereco_Numero")
                .HasMaxLength(20);

            endereco.Property(e => e.Complemento)
                .HasColumnName("Endereco_Complemento")
                .HasMaxLength(100);

            endereco.Property(e => e.Bairro)
                .HasColumnName("Endereco_Bairro")
                .HasMaxLength(100)
                .IsRequired();

            endereco.Property(e => e.Cidade)
                .HasColumnName("Endereco_Cidade")
                .HasMaxLength(100)
                .IsRequired();

            endereco.Property(e => e.Estado)
                .HasColumnName("Endereco_Estado")
                .HasMaxLength(2)
                .IsRequired();
            endereco.Property(e => e.Pais)
                .HasColumnName("Endereco_Pais")
                .HasMaxLength(50)
                .IsRequired();
        });


        builder.HasMany(p => p.Itens)
            .WithOne()
            .HasForeignKey("PedidoId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Itens)
            .HasField("_itens")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(p => p.Pagamentos)
            .WithOne()
            .HasForeignKey("PedidoId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Pagamentos)
            .HasField("_pagamentos")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
