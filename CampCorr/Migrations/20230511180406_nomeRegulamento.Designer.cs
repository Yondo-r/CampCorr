﻿// <auto-generated />
using System;
using CampCorr.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CampCorr.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230511180406_nomeRegulamento")]
    partial class nomeRegulamento
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CampCorr.Models.Campeonato", b =>
                {
                    b.Property<int>("CampeonatoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CampeonatoId"), 1L, 1);

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CampeonatoId");

                    b.ToTable("Campeonatos");
                });

            modelBuilder.Entity("CampCorr.Models.Circuito", b =>
                {
                    b.Property<int>("CircuitoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CircuitoId"), 1L, 1);

                    b.Property<string>("Endereço")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CircuitoId");

                    b.ToTable("Circuitos");
                });

            modelBuilder.Entity("CampCorr.Models.Equipe", b =>
                {
                    b.Property<int>("EquipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EquipeId"), 1L, 1);

                    b.Property<int>("CampeonatoId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Emblema")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TemporadaId")
                        .HasColumnType("int");

                    b.HasKey("EquipeId");

                    b.HasIndex("CampeonatoId");

                    b.HasIndex("TemporadaId");

                    b.ToTable("Equipes");
                });

            modelBuilder.Entity("CampCorr.Models.EquipeTemporada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EquipeId")
                        .HasColumnType("int");

                    b.Property<int>("TemporadaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EquipeTemporadas");
                });

            modelBuilder.Entity("CampCorr.Models.Etapa", b =>
                {
                    b.Property<int>("EtapaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EtapaId"), 1L, 1);

                    b.Property<bool>("Concluido")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<int>("KartodromoId")
                        .HasColumnType("int");

                    b.Property<string>("NumeroEvento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TemporadaId")
                        .HasColumnType("int");

                    b.Property<string>("Traçado")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EtapaId");

                    b.HasIndex("KartodromoId");

                    b.HasIndex("TemporadaId");

                    b.ToTable("Etapas");
                });

            modelBuilder.Entity("CampCorr.Models.Piloto", b =>
                {
                    b.Property<int>("PilotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PilotoId"), 1L, 1);

                    b.Property<DateTime?>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Peso")
                        .HasColumnType("Decimal(5,2)");

                    b.Property<int?>("TemporadaId")
                        .HasColumnType("int");

                    b.Property<string>("TipoSanguineo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PilotoId");

                    b.HasIndex("TemporadaId");

                    b.ToTable("Pilotos");
                });

            modelBuilder.Entity("CampCorr.Models.PilotoCampeonato", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CampeonatoId")
                        .HasColumnType("int");

                    b.Property<int>("PilotoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PilotosCampeonatos");
                });

            modelBuilder.Entity("CampCorr.Models.PilotoEquipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EquipeId")
                        .HasColumnType("int");

                    b.Property<int>("PilotoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PilotosEquipes");
                });

            modelBuilder.Entity("CampCorr.Models.PilotoTemporada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("PilotoId")
                        .HasColumnType("int");

                    b.Property<int>("TemporadaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PilotosTemporadas");
                });

            modelBuilder.Entity("CampCorr.Models.Regulamento", b =>
                {
                    b.Property<int>("RegulamentoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegulamentoId"), 1L, 1);

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RegulamentoId");

                    b.ToTable("Regulamentos");
                });

            modelBuilder.Entity("CampCorr.Models.ResultadoCorrida", b =>
                {
                    b.Property<int>("ResultadoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ResultadoId"), 1L, 1);

                    b.Property<string>("DescricaoPenalidade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EquipeId")
                        .HasColumnType("int");

                    b.Property<int>("EtapaId")
                        .HasColumnType("int");

                    b.Property<bool>("MelhorVolta")
                        .HasColumnType("bit");

                    b.Property<int>("PilotoId")
                        .HasColumnType("int");

                    b.Property<int?>("Pontos")
                        .HasColumnType("int");

                    b.Property<int?>("PontosPenalidade")
                        .HasColumnType("int");

                    b.Property<int?>("Posicao")
                        .HasColumnType("int");

                    b.Property<int?>("PosicaoLargada")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("TempoMelhorVolta")
                        .HasColumnType("time");

                    b.Property<TimeSpan?>("TempoTotal")
                        .HasColumnType("time");

                    b.Property<int?>("TotalVoltas")
                        .HasColumnType("int");

                    b.HasKey("ResultadoId");

                    b.ToTable("ResultadosCorrida");
                });

            modelBuilder.Entity("CampCorr.Models.Temporada", b =>
                {
                    b.Property<int>("TemporadaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TemporadaId"), 1L, 1);

                    b.Property<int>("AnoTemporada")
                        .HasColumnType("int");

                    b.Property<int>("CampeonatoId")
                        .HasColumnType("int");

                    b.Property<bool>("Concluida")
                        .HasColumnType("bit");

                    b.Property<int>("QuantidadeEtapas")
                        .HasColumnType("Int");

                    b.Property<int>("RegulamentoId")
                        .HasColumnType("int");

                    b.HasKey("TemporadaId");

                    b.HasIndex("CampeonatoId");

                    b.HasIndex("RegulamentoId");

                    b.ToTable("Temporadas");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CampCorr.Models.Equipe", b =>
                {
                    b.HasOne("CampCorr.Models.Campeonato", null)
                        .WithMany("Equipes")
                        .HasForeignKey("CampeonatoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CampCorr.Models.Temporada", null)
                        .WithMany("Equipes")
                        .HasForeignKey("TemporadaId");
                });

            modelBuilder.Entity("CampCorr.Models.Etapa", b =>
                {
                    b.HasOne("CampCorr.Models.Circuito", "Kartodromo")
                        .WithMany()
                        .HasForeignKey("KartodromoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CampCorr.Models.Temporada", "Temporada")
                        .WithMany("Etapas")
                        .HasForeignKey("TemporadaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kartodromo");

                    b.Navigation("Temporada");
                });

            modelBuilder.Entity("CampCorr.Models.Piloto", b =>
                {
                    b.HasOne("CampCorr.Models.Temporada", null)
                        .WithMany("Pilotos")
                        .HasForeignKey("TemporadaId");
                });

            modelBuilder.Entity("CampCorr.Models.Temporada", b =>
                {
                    b.HasOne("CampCorr.Models.Campeonato", "Campeonato")
                        .WithMany("Temporadas")
                        .HasForeignKey("CampeonatoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CampCorr.Models.Regulamento", "Regulamento")
                        .WithMany()
                        .HasForeignKey("RegulamentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Campeonato");

                    b.Navigation("Regulamento");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CampCorr.Models.Campeonato", b =>
                {
                    b.Navigation("Equipes");

                    b.Navigation("Temporadas");
                });

            modelBuilder.Entity("CampCorr.Models.Temporada", b =>
                {
                    b.Navigation("Equipes");

                    b.Navigation("Etapas");

                    b.Navigation("Pilotos");
                });
#pragma warning restore 612, 618
        }
    }
}
