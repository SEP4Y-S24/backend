﻿// <auto-generated />
using System;
using AlarmServices.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AlarmServices.Migrations
{
    [DbContext(typeof(ClockContext))]
    [Migration("20240514112724_intialAlarmService3")]
    partial class intialAlarmService3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AlarmServices.Model.Alarm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClockId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSnoozed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("SetOffTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClockId");

                    b.ToTable("Alarms");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                            ClockId = new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                            IsActive = true,
                            IsSnoozed = false,
                            SetOffTime = new DateTime(2024, 5, 14, 12, 27, 24, 164, DateTimeKind.Utc).AddTicks(4037)
                        });
                });

            modelBuilder.Entity("AlarmServices.Model.Clock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<long>("TimeOffset")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Clocks");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                            Name = "Test Clock",
                            OwnerId = new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360"),
                            TimeOffset = 0L
                        });
                });

            modelBuilder.Entity("AlarmServices.Model.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ClockId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ClockId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("AlarmServices.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360")
                        });
                });

            modelBuilder.Entity("AlarmServices.Model.Alarm", b =>
                {
                    b.HasOne("AlarmServices.Model.Clock", "Clock")
                        .WithMany()
                        .HasForeignKey("ClockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clock");
                });

            modelBuilder.Entity("AlarmServices.Model.Clock", b =>
                {
                    b.HasOne("AlarmServices.Model.User", "Owner")
                        .WithMany("Clocks")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("AlarmServices.Model.Message", b =>
                {
                    b.HasOne("AlarmServices.Model.Clock", "Clock")
                        .WithMany("Messages")
                        .HasForeignKey("ClockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AlarmServices.Model.User", "Reciever")
                        .WithMany("MessagesRecieved")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AlarmServices.Model.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clock");

                    b.Navigation("Reciever");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("AlarmServices.Model.Clock", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("AlarmServices.Model.User", b =>
                {
                    b.Navigation("Clocks");

                    b.Navigation("MessagesRecieved");

                    b.Navigation("MessagesSent");
                });
#pragma warning restore 612, 618
        }
    }
}
