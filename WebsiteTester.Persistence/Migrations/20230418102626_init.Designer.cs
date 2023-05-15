﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebsiteTester.Persistence;

#nullable disable

namespace WebsiteTester.Persistenсe.Migrations
{
    [DbContext(typeof(WebsiteTesterDbContext))]
    [Migration("20230418102626_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebsiteTester.Domain.Models.Link", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("WebsiteTester.Domain.Models.LinkTestResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsInSitemap")
                        .HasColumnType("bit");

                    b.Property<bool>("IsInWebsite")
                        .HasColumnType("bit");

                    b.Property<Guid>("LinkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RenderTimeMilliseconds")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LinkId");

                    b.ToTable("LinkTestResults");
                });

            modelBuilder.Entity("WebsiteTester.Domain.Models.LinkTestResult", b =>
                {
                    b.HasOne("WebsiteTester.Domain.Models.Link", "Link")
                        .WithMany("LinkTestResults")
                        .HasForeignKey("LinkId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Link");
                });

            modelBuilder.Entity("WebsiteTester.Domain.Models.Link", b =>
                {
                    b.Navigation("LinkTestResults");
                });
#pragma warning restore 612, 618
        }
    }
}
