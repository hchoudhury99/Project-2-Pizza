﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PizzaAPI.Model;

namespace PizzaAPI.Migrations
{
    [DbContext(typeof(PizzaDbContext))]
    [Migration("20190719175931_(1) migrations")]
    partial class _1migrations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PizzaAPI.Model.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("PhoneNo");

                    b.Property<int>("UserId");

                    b.HasKey("CustomerId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("PizzaAPI.Model.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CustomerId");

                    b.Property<DateTime>("Duetime");

                    b.Property<DateTime>("OrderDate");

                    b.Property<double>("TotalPrice");

                    b.HasKey("OrderId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("PizzaAPI.Model.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CardNo");

                    b.Property<int>("CustomerId");

                    b.HasKey("PaymentId");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("PizzaAPI.Model.Pizza", b =>
                {
                    b.Property<int>("PizzaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Crust");

                    b.Property<int?>("OrderId");

                    b.Property<int?>("PizzaName");

                    b.Property<int>("Sauce");

                    b.Property<int>("Size");

                    b.Property<int?>("Topping1");

                    b.Property<int?>("Topping2");

                    b.Property<int?>("Topping3");

                    b.HasKey("PizzaId");

                    b.HasIndex("OrderId");

                    b.ToTable("Pizza");
                });

            modelBuilder.Entity("PizzaAPI.Model.Order", b =>
                {
                    b.HasOne("PizzaAPI.Model.Customer", "Customer")
                        .WithMany("Order")
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("PizzaAPI.Model.Payment", b =>
                {
                    b.HasOne("PizzaAPI.Model.Customer", "Customer")
                        .WithOne("Payment")
                        .HasForeignKey("PizzaAPI.Model.Payment", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PizzaAPI.Model.Pizza", b =>
                {
                    b.HasOne("PizzaAPI.Model.Order", "Order")
                        .WithMany("Pizza")
                        .HasForeignKey("OrderId");
                });
#pragma warning restore 612, 618
        }
    }
}
