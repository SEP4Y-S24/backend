﻿// <auto-generated />
using System;
using ClockServices.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TodoServices.Migrations
{
    [DbContext(typeof(ClockContext))]
    partial class ClockContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TodoServices.Model.Clock", b =>
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

            modelBuilder.Entity("TodoServices.Model.Message", b =>
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

            modelBuilder.Entity("TodoServices.Model.Todo", b =>
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
                            Deadline = new DateTime(2024, 5, 21, 12, 8, 11, 80, DateTimeKind.Utc).AddTicks(9283),
                            Description = "hello description",
                            Name = "Hello",
                            Status = 1,
                            UserId = new Guid("5f3bb5af-e982-4a8b-8590-b620597a7360")
                        });
                });

            modelBuilder.Entity("TodoServices.Model.User", b =>
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

            modelBuilder.Entity("TodoServices.Model.Clock", b =>
                {
                    b.HasOne("TodoServices.Model.User", "Owner")
                        .WithMany("Clocks")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TodoServices.Model.Message", b =>
                {
                    b.HasOne("TodoServices.Model.Clock", "Clock")
                        .WithMany("Messages")
                        .HasForeignKey("ClockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoServices.Model.User", "Reciever")
                        .WithMany("MessagesRecieved")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoServices.Model.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clock");

                    b.Navigation("Reciever");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("TodoServices.Model.Todo", b =>
                {
                    b.HasOne("TodoServices.Model.User", "User")
                        .WithMany("Todos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TodoServices.Model.Clock", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("TodoServices.Model.User", b =>
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
