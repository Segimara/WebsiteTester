﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebsiteTester.Persistance;

#nullable disable

namespace WebsiteTester.Persistance.Migrations
{
    [DbContext(typeof(WebsiteTesterDbContext))]
    partial class WebsiteTesterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebsiteTester.Domain.Models.Link", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("GETUTCDATE()");

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
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("GETUTCDATE()");

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
