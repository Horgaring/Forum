﻿// <auto-generated />
using System;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(CommentDbContext))]
    partial class CommentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Postid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Postid");

                    b.ToTable("Comment");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Comment");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Entities.SubComment", b =>
                {
                    b.HasBaseType("Domain.Entities.Comment");

                    b.Property<Guid>("ParentComment")
                        .HasColumnType("uuid");

                    b.HasDiscriminator().HasValue("SubComment");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.CustomerInfo", "CustomerInfo", b1 =>
                        {
                            b1.Property<Guid>("CommentId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.HasKey("CommentId");

                            b1.ToTable("Comment");

                            b1.WithOwner()
                                .HasForeignKey("CommentId");
                        });

                    b.Navigation("CustomerInfo")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
