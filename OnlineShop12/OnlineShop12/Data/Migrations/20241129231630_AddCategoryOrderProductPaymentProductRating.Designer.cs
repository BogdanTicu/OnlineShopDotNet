﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineShop12.Data;

#nullable disable

namespace OnlineShop12.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241129231630_AddCategoryOrderProductPaymentProductRating")]
    partial class AddCategoryOrderProductPaymentProductRating
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OnlineShop.Models.Category", b =>
                {
                    b.Property<int>("Id_Category")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Category"));

                    b.Property<string>("Category_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id_User")
                        .HasColumnType("int");

                    b.HasKey("Id_Category");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("OnlineShop.Models.Order", b =>
                {
                    b.Property<int>("Id_Order")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Order"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Total_Amount")
                        .HasColumnType("int");

                    b.HasKey("Id_Order");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OnlineShop.Models.OrderProduct", b =>
                {
                    b.Property<int?>("Id_Order")
                        .HasColumnType("int");

                    b.Property<int?>("Id_Product")
                        .HasColumnType("int");

                    b.HasKey("Id_Order", "Id_Product");

                    b.HasIndex("Id_Product");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("OnlineShop.Models.Payment", b =>
                {
                    b.Property<int>("Id_Payment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Payment"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id_Order")
                        .HasColumnType("int");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Order_Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id_Payment");

                    b.HasIndex("Id_Order")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("OnlineShop.Models.Product", b =>
                {
                    b.Property<int>("Id_Product")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Product"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id_Category")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Product");

                    b.HasIndex("Id_Category");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("OnlineShop.Models.Rating", b =>
                {
                    b.Property<int>("Id_Rating")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Rating"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id_Product")
                        .HasColumnType("int");

                    b.Property<int>("Id_User")
                        .HasColumnType("int");

                    b.HasKey("Id_Rating");

                    b.HasIndex("Id_Product");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("OnlineShop.Models.Review", b =>
                {
                    b.Property<int>("Id_Review")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Review"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id_Product")
                        .HasColumnType("int");

                    b.Property<int>("Id_User")
                        .HasColumnType("int");

                    b.HasKey("Id_Review");

                    b.HasIndex("Id_Product");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("OnlineShop.Models.OrderProduct", b =>
                {
                    b.HasOne("OnlineShop.Models.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("Id_Order")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OnlineShop.Models.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("Id_Product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OnlineShop.Models.Payment", b =>
                {
                    b.HasOne("OnlineShop.Models.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("OnlineShop.Models.Payment", "Id_Order")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("OnlineShop.Models.Product", b =>
                {
                    b.HasOne("OnlineShop.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("Id_Category")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("OnlineShop.Models.Rating", b =>
                {
                    b.HasOne("OnlineShop.Models.Product", "Product")
                        .WithMany("Ratings")
                        .HasForeignKey("Id_Product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OnlineShop.Models.Review", b =>
                {
                    b.HasOne("OnlineShop.Models.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("Id_Product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OnlineShop.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("OnlineShop.Models.Order", b =>
                {
                    b.Navigation("OrderProducts");

                    b.Navigation("Payment")
                        .IsRequired();
                });

            modelBuilder.Entity("OnlineShop.Models.Product", b =>
                {
                    b.Navigation("OrderProducts");

                    b.Navigation("Ratings");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
