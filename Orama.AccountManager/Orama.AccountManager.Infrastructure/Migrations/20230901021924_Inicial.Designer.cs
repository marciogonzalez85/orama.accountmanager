﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orama.AccountManager.Infrastructure.DB;

#nullable disable

namespace Orama.AccountManager.Infrastructure.Migrations
{
    [DbContext(typeof(AccountManagerDbContext))]
    [Migration("20230901021924_Inicial")]
    partial class Inicial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Orama.AccountManager.Model.Entities.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("BankAccounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Balance = 1000m,
                            ClienteId = 1
                        });
                });

            modelBuilder.Entity("Orama.AccountManager.Model.Entities.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebhookUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clientes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "cliente_a@email.com",
                            Name = "Cliente A",
                            Password = "password",
                            WebhookUrl = "http://localhost:1234/notification/"
                        });
                });

            modelBuilder.Entity("Orama.AccountManager.Model.Entities.FinancialAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("FinancialAssets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "AXZ Stock",
                            Price = 15m
                        });
                });

            modelBuilder.Entity("Orama.AccountManager.Model.Entities.FinancialTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BankAccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExternalReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FinancialAssetId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.HasIndex("FinancialAssetId");

                    b.ToTable("FinancialTransactions");
                });

            modelBuilder.Entity("Orama.AccountManager.Model.Entities.BankAccount", b =>
                {
                    b.HasOne("Orama.AccountManager.Model.Entities.Cliente", "Cliente")
                        .WithMany("BankAccounts")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("Orama.AccountManager.Model.Entities.FinancialTransaction", b =>
                {
                    b.HasOne("Orama.AccountManager.Model.Entities.BankAccount", "BankAccount")
                        .WithMany("FinancialTransactions")
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Orama.AccountManager.Model.Entities.FinancialAsset", "FinancialAsset")
                        .WithMany("FinancialTransactions")
                        .HasForeignKey("FinancialAssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankAccount");

                    b.Navigation("FinancialAsset");
                });

            modelBuilder.Entity("Orama.AccountManager.Model.Entities.BankAccount", b =>
                {
                    b.Navigation("FinancialTransactions");
                });

            modelBuilder.Entity("Orama.AccountManager.Model.Entities.Cliente", b =>
                {
                    b.Navigation("BankAccounts");
                });

            modelBuilder.Entity("Orama.AccountManager.Model.Entities.FinancialAsset", b =>
                {
                    b.Navigation("FinancialTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
