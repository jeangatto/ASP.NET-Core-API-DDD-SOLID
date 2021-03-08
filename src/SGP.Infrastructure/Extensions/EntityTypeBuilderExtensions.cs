﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Shared.Entities;

namespace SGP.Infrastructure.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Configuração das propriedades da Entidade Base <see cref="BaseEntity"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        public static void ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : BaseEntity
        {
            // Configurando o nome da tabela para ser o mesmo que o nome da classe.
            // Por padrão o EF usa o nome declarado no DbSet dentro da classe que herda o DbContexto.
            builder.ToTable(typeof(TEntity).Name);

            // Configurando a coluna "Id" como Chave Primária (PK).
            builder.HasKey(e => e.Id);

            // Configurando a coluna como NOT NULL.
            // Configurando para o banco nunca gerar o valor, o "Id" é gerado pela aplicação.
            builder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}