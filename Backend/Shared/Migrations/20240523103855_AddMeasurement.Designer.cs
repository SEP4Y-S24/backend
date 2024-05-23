﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Shared.Context;

#nullable disable

namespace Shared.Migrations
{
    [DbContext(typeof(ClockContext))]
    [Migration("20240523103855_AddMeasurement")]
    partial class AddMeasurement
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Models.Alarm", b =>
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeOnly>("SetOffTime")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.ToTable("Alarms");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ac96066e-c7da-4b53-9203-d1bf4b5a88b9"),
                            ClockId = new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                            IsActive = true,
                            IsSnoozed = false,
                            Name = "Default alarm",
                            SetOffTime = new TimeOnly(1, 20, 0)
                        });
                });

            modelBuilder.Entity("Models.Clock", b =>
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

            modelBuilder.Entity("Models.Measurement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClockId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("TimeOfReading")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ClockId");

                    b.ToTable("Measurements");
                });

            modelBuilder.Entity("Models.Message", b =>
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

            modelBuilder.Entity("Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Models.Todo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Todos");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                            Deadline = new DateTime(2024, 5, 30, 10, 38, 54, 974, DateTimeKind.Utc).AddTicks(8894),
                            Description = "hello description",
                            Name = "Hello",
                            Status = 1,
                            UserId = new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360")
                        });
                });

            modelBuilder.Entity("Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AvatarId")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360"),
                            AvatarId = 1,
                            Email = "email@gmail.com",
                            Name = "Test User",
                            PasswordHash = "psgzhj,sxzjh"
                        });
                });

            modelBuilder.Entity("TagTodo", b =>
                {
                    b.Property<Guid>("TagsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TodosId")
                        .HasColumnType("uuid");

                    b.HasKey("TagsId", "TodosId");

                    b.HasIndex("TodosId");

                    b.ToTable("TagTodo");
                });

            modelBuilder.Entity("Models.Clock", b =>
                {
                    b.HasOne("Models.User", "Owner")
                        .WithMany("Clocks")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Models.Measurement", b =>
                {
                    b.HasOne("Models.Clock", "Clock")
                        .WithMany("Measurements")
                        .HasForeignKey("ClockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clock");
                });

            modelBuilder.Entity("Models.Message", b =>
                {
                    b.HasOne("Models.Clock", "Clock")
                        .WithMany("Messages")
                        .HasForeignKey("ClockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.User", "Reciever")
                        .WithMany("MessagesRecieved")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clock");

                    b.Navigation("Reciever");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Models.Todo", b =>
                {
                    b.HasOne("Models.User", "User")
                        .WithMany("Todos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TagTodo", b =>
                {
                    b.HasOne("Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Todo", null)
                        .WithMany()
                        .HasForeignKey("TodosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Clock", b =>
                {
                    b.Navigation("Measurements");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Models.User", b =>
                {
                    b.Navigation("Clocks");

                    b.Navigation("MessagesRecieved");

                    b.Navigation("MessagesSent");

                    b.Navigation("Todos");
                });
#pragma warning restore 612, 618
        }
    }
}
