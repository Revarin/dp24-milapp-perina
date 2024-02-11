﻿// <auto-generated />
using System;
using Kris.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Kris.Server.Data.Models.SessionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.SessionUserEntity", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Joined")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("UserId", "SessionId");

                    b.HasIndex("SessionId");

                    b.ToTable("SessionUsers");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CurrentSessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id", "CurrentSessionId")
                        .IsUnique()
                        .HasFilter("[CurrentSessionId] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserPositionEntity", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Position0Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position1Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position2Data")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "SessionId");

                    b.ToTable("UserPositions");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.SessionUserEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.SessionEntity", "Session")
                        .WithMany("Users")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Kris.Server.Data.Models.UserEntity", "User")
                        .WithMany("AllSessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.SessionUserEntity", "CurrentSession")
                        .WithOne()
                        .HasForeignKey("Kris.Server.Data.Models.UserEntity", "Id", "CurrentSessionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CurrentSession");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserPositionEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.SessionUserEntity", "SessionUser")
                        .WithOne()
                        .HasForeignKey("Kris.Server.Data.Models.UserPositionEntity", "UserId", "SessionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("SessionUser");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.SessionEntity", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserEntity", b =>
                {
                    b.Navigation("AllSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
