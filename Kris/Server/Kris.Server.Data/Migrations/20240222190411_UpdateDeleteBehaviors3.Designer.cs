﻿// <auto-generated />
using System;
using Kris.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240222190411_UpdateDeleteBehaviors3")]
    partial class UpdateDeleteBehaviors3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Joined")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.HasIndex("UserId");

                    b.ToTable("SessionUsers");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
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

                    b.HasIndex("CurrentSessionId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserPositionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Position0Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position1Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position2Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SessionUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SessionUserId");

                    b.ToTable("UserPositions");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.SessionUserEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.SessionEntity", "Session")
                        .WithMany("Users")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kris.Server.Data.Models.UserEntity", "User")
                        .WithMany("AllSessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.SessionUserEntity", "CurrentSession")
                        .WithMany()
                        .HasForeignKey("CurrentSessionId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("CurrentSession");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserPositionEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.SessionUserEntity", "SessionUser")
                        .WithMany()
                        .HasForeignKey("SessionUserId")
                        .OnDelete(DeleteBehavior.Cascade)
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
