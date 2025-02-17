﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimplifiedLotteryGame.Data;

#nullable disable

namespace SimplifiedLotteryGame.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal?>("Revenue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Profit")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.PlayerPrize", b =>
                {
                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("PrizeId")
                        .HasColumnType("int");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.Property<DateTime>("WinDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PlayerId", "PrizeId", "TicketId");

                    b.HasIndex("PrizeId");

                    b.HasIndex("TicketId");

                    b.ToTable("PlayerPrize", (string)null);
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.Prize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrizeValue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Prizes");
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<Guid>("TicketNumber")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.PlayerPrize", b =>
                {
                    b.HasOne("SimplifiedLotteryGame.Data.Models.Player", "Player")
                        .WithMany("PlayerPrizes")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SimplifiedLotteryGame.Data.Models.Prize", "Prize")
                        .WithMany("PlayerPrizes")
                        .HasForeignKey("PrizeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SimplifiedLotteryGame.Data.Models.Ticket", "Ticket")
                        .WithMany("PlayerPrizes")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Prize");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.Prize", b =>
                {
                    b.HasOne("SimplifiedLotteryGame.Data.Models.Game", "Game")
                        .WithMany("Prizes")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.Ticket", b =>
                {
                    b.HasOne("SimplifiedLotteryGame.Data.Models.Game", "Game")
                        .WithMany("Tickets")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SimplifiedLotteryGame.Data.Models.Player", "Player")
                        .WithMany("Tickets")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.Game", b =>
                {
                    b.Navigation("Prizes");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.Player", b =>
                {
                    b.Navigation("PlayerPrizes");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.Prize", b =>
                {
                    b.Navigation("PlayerPrizes");
                });

            modelBuilder.Entity("SimplifiedLotteryGame.Data.Models.Ticket", b =>
                {
                    b.Navigation("PlayerPrizes");
                });
#pragma warning restore 612, 618
        }
    }
}
