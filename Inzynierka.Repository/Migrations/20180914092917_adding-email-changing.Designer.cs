﻿// <auto-generated />
using Inzynierka.Repository.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Inzynierka.Repository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180914092917_adding-email-changing")]
    partial class addingemailchanging
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Inzynierka.Data.DbModels.Motive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("FontColor")
                        .IsRequired();

                    b.Property<bool?>("IsSharedGlobally");

                    b.Property<string>("MainColor")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Motives");
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer")
                        .IsRequired();

                    b.Property<bool>("AnsweredBeforeSugestion");

                    b.Property<string>("CorrectAnswer")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<double>("PointsForQuestion");

                    b.Property<int>("QuizId");

                    b.Property<int>("TimeForAnswerInSeconds");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.Quiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("NumberOfNegativeRates");

                    b.Property<int>("NumberOfPositiveRates");

                    b.Property<double>("PointsForGame");

                    b.Property<string>("QuizType")
                        .IsRequired();

                    b.Property<double>("RateInNumber");

                    b.Property<int>("SecondsSpendOnQuiz");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Quizes");
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.Rate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<double>("CurrentPercentageRate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("NumberOfPlayedGames");

                    b.Property<double>("PointsForAllGames");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Rates");
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.SharedMotives", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("MotiveId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("MotiveId");

                    b.HasIndex("UserId");

                    b.ToTable("SharedMotives");
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.Sound", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("OctaveSymbol")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Sounds");
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("BirthDate");

                    b.Property<string>("CookiesActivateLink")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsAcceptedRegister");

                    b.Property<string>("LastName");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.Property<bool?>("Sex");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.UserChangingEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UsersChangingEmail");
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.UserSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int?>("MotiveId");

                    b.Property<string>("PathToAvatar");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("MotiveId")
                        .IsUnique()
                        .HasFilter("[MotiveId] IS NOT NULL");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.Motive", b =>
                {
                    b.HasOne("Inzynierka.Data.DbModels.User", "User")
                        .WithMany("Motives")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.Question", b =>
                {
                    b.HasOne("Inzynierka.Data.DbModels.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.Quiz", b =>
                {
                    b.HasOne("Inzynierka.Data.DbModels.User", "User")
                        .WithMany("Quizes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.Rate", b =>
                {
                    b.HasOne("Inzynierka.Data.DbModels.User", "User")
                        .WithOne("Rate")
                        .HasForeignKey("Inzynierka.Data.DbModels.Rate", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.SharedMotives", b =>
                {
                    b.HasOne("Inzynierka.Data.DbModels.Motive", "Motive")
                        .WithMany("SharedMotives")
                        .HasForeignKey("MotiveId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Inzynierka.Data.DbModels.User", "User")
                        .WithMany("SharedMotives")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.Sound", b =>
                {
                    b.HasOne("Inzynierka.Data.DbModels.User", "User")
                        .WithMany("Sounds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.UserChangingEmail", b =>
                {
                    b.HasOne("Inzynierka.Data.DbModels.User", "User")
                        .WithOne("UserChangingEmail")
                        .HasForeignKey("Inzynierka.Data.DbModels.UserChangingEmail", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inzynierka.Data.DbModels.UserSetting", b =>
                {
                    b.HasOne("Inzynierka.Data.DbModels.Motive", "Motive")
                        .WithOne("UserSetting")
                        .HasForeignKey("Inzynierka.Data.DbModels.UserSetting", "MotiveId");

                    b.HasOne("Inzynierka.Data.DbModels.User", "User")
                        .WithOne("UserSetting")
                        .HasForeignKey("Inzynierka.Data.DbModels.UserSetting", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
