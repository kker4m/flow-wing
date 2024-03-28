﻿// <auto-generated />
using System;
using FlowWing.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlowWing.DataAccess.Migrations
{
    [DbContext(typeof(FlowWingDbContext))]
    partial class FlowWingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FlowWing.Entities.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<int>("EmailLogId")
                        .HasColumnType("integer");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("EmailLogId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("FlowWing.Entities.EmailLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("Answer")
                        .HasColumnType("integer");

                    b.Property<string>("AttachmentIds")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EmailSubject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ForwardedFrom")
                        .HasColumnType("integer");

                    b.Property<string>("HangfireJobId")
                        .HasColumnType("text");

                    b.Property<bool>("IsScheduled")
                        .HasColumnType("boolean");

                    b.Property<string>("RecipientsEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SenderEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("SentDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SentEmailBody")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int?>("repeatingLogId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("EmailLogs");
                });

            modelBuilder.Entity("FlowWing.Entities.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("LogTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("FlowWing.Entities.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("FlowWing.Entities.ScheduledEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EmailLogId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRepeating")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastSendingDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("NextSendingDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RepeatEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RepeatInterval")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EmailLogId");

                    b.ToTable("ScheduledEmails");
                });

            modelBuilder.Entity("FlowWing.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsApplicationUser")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastLoginDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FlowWing.Entities.Attachment", b =>
                {
                    b.HasOne("FlowWing.Entities.EmailLog", "EmailLog")
                        .WithMany()
                        .HasForeignKey("EmailLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmailLog");
                });

            modelBuilder.Entity("FlowWing.Entities.EmailLog", b =>
                {
                    b.HasOne("FlowWing.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FlowWing.Entities.ScheduledEmail", b =>
                {
                    b.HasOne("FlowWing.Entities.EmailLog", "EmailLog")
                        .WithMany()
                        .HasForeignKey("EmailLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmailLog");
                });
#pragma warning restore 612, 618
        }
    }
}
