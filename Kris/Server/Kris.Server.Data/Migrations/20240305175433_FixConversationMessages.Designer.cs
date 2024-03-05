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
    [Migration("20240305175433_FixConversationMessages")]
    partial class FixConversationMessages
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ConversationEntitySessionUserEntity", b =>
                {
                    b.Property<Guid>("ConversationsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ConversationsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ConversationEntitySessionUserEntity");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.ConversationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ConversationType")
                        .HasColumnType("int");

                    b.Property<Guid?>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.MapPointEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SessionUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SessionUserId");

                    b.ToTable("MapPoints");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.MessageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

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
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("UserPositions");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserSettingsEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("GpsRequestInterval")
                        .HasColumnType("int");

                    b.Property<int?>("MapObjectDownloadFrequency")
                        .HasColumnType("int");

                    b.Property<int?>("PositionDownloadFrequency")
                        .HasColumnType("int");

                    b.Property<int?>("PositionUploadFrequency")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("ConversationEntitySessionUserEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.ConversationEntity", null)
                        .WithMany()
                        .HasForeignKey("ConversationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kris.Server.Data.Models.SessionUserEntity", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Kris.Server.Data.Models.ConversationEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.SessionEntity", "Session")
                        .WithMany("Conversations")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.MapPointEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.SessionUserEntity", "SessionUser")
                        .WithMany("MapPoints")
                        .HasForeignKey("SessionUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Kris.Common.Models.GeoPosition", "Position", b1 =>
                        {
                            b1.Property<Guid>("MapPointEntityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Altitude")
                                .HasColumnType("float");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float");

                            b1.HasKey("MapPointEntityId");

                            b1.ToTable("MapPoints");

                            b1.WithOwner()
                                .HasForeignKey("MapPointEntityId");
                        });

                    b.OwnsOne("Kris.Common.Models.MapPointSymbol", "Symbol", b1 =>
                        {
                            b1.Property<Guid>("MapPointEntityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Color")
                                .HasColumnType("int");

                            b1.Property<int>("Shape")
                                .HasColumnType("int");

                            b1.Property<int>("Sign")
                                .HasColumnType("int");

                            b1.HasKey("MapPointEntityId");

                            b1.ToTable("MapPoints");

                            b1.WithOwner()
                                .HasForeignKey("MapPointEntityId");
                        });

                    b.Navigation("Position")
                        .IsRequired();

                    b.Navigation("SessionUser");

                    b.Navigation("Symbol")
                        .IsRequired();
                });

            modelBuilder.Entity("Kris.Server.Data.Models.MessageEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.ConversationEntity", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kris.Server.Data.Models.SessionUserEntity", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Conversation");

                    b.Navigation("Sender");
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
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Kris.Common.Models.GeoSpatialPosition", "Position_0", b1 =>
                        {
                            b1.Property<Guid>("UserPositionEntityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Altitude")
                                .HasColumnType("float");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.HasKey("UserPositionEntityId");

                            b1.ToTable("UserPositions");

                            b1.WithOwner()
                                .HasForeignKey("UserPositionEntityId");
                        });

                    b.OwnsOne("Kris.Common.Models.GeoSpatialPosition", "Position_1", b1 =>
                        {
                            b1.Property<Guid>("UserPositionEntityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Altitude")
                                .HasColumnType("float");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.HasKey("UserPositionEntityId");

                            b1.ToTable("UserPositions");

                            b1.WithOwner()
                                .HasForeignKey("UserPositionEntityId");
                        });

                    b.OwnsOne("Kris.Common.Models.GeoSpatialPosition", "Position_2", b1 =>
                        {
                            b1.Property<Guid>("UserPositionEntityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Altitude")
                                .HasColumnType("float");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.HasKey("UserPositionEntityId");

                            b1.ToTable("UserPositions");

                            b1.WithOwner()
                                .HasForeignKey("UserPositionEntityId");
                        });

                    b.Navigation("Position_0");

                    b.Navigation("Position_1");

                    b.Navigation("Position_2");

                    b.Navigation("SessionUser");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserSettingsEntity", b =>
                {
                    b.HasOne("Kris.Server.Data.Models.UserEntity", "User")
                        .WithOne("Settings")
                        .HasForeignKey("Kris.Server.Data.Models.UserSettingsEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.ConversationEntity", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.SessionEntity", b =>
                {
                    b.Navigation("Conversations");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.SessionUserEntity", b =>
                {
                    b.Navigation("MapPoints");

                    b.Navigation("SentMessages");
                });

            modelBuilder.Entity("Kris.Server.Data.Models.UserEntity", b =>
                {
                    b.Navigation("AllSessions");

                    b.Navigation("Settings")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
