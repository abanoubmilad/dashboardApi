﻿// <auto-generated />
using System;
using DashboardApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DashboardApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20191224233957_v1")]
    partial class v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("DashboardApi.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryName");

                    b.HasKey("CategoryId");

                    b.HasIndex("CategoryName");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("DashboardApi.Entities.DashboardUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Role");

                    b.HasKey("Id");

                    b.ToTable("DashboardUsers");
                });

            modelBuilder.Entity("DashboardApi.Entities.Data.DashboardInvitation", b =>
                {
                    b.Property<string>("Email")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("ConfirmationHash");

                    b.Property<byte[]>("ConfirmationSalt");

                    b.Property<int>("Status");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Email");

                    b.ToTable("DashboardInvitations");
                });

            modelBuilder.Entity("DashboardApi.Entities.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ItemExternalLink");

                    b.Property<string>("ItemImageUrl");

                    b.Property<string>("ItemInternalLink");

                    b.Property<string>("ItemName");

                    b.HasKey("ItemId");

                    b.HasIndex("ItemName");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("DashboardApi.Entities.ItemTag", b =>
                {
                    b.Property<int>("TagId");

                    b.Property<int>("ItemId");

                    b.HasKey("TagId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemTag");
                });

            modelBuilder.Entity("DashboardApi.Entities.Lesson", b =>
                {
                    b.Property<int>("LessonId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LessonName");

                    b.HasKey("LessonId");

                    b.HasIndex("LessonName");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("DashboardApi.Entities.LessonCategory", b =>
                {
                    b.Property<int>("LessonId");

                    b.Property<int>("CategoryId");

                    b.HasKey("LessonId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("LessonCategory");
                });

            modelBuilder.Entity("DashboardApi.Entities.LessonItem", b =>
                {
                    b.Property<int>("LessonId");

                    b.Property<int>("ItemId");

                    b.HasKey("LessonId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("LessonItem");
                });

            modelBuilder.Entity("DashboardApi.Entities.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TagName");

                    b.Property<int>("TagType");

                    b.HasKey("TagId");

                    b.HasIndex("TagName");

                    b.HasIndex("TagType");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("DashboardApi.Entities.ItemTag", b =>
                {
                    b.HasOne("DashboardApi.Entities.Item", "Item")
                        .WithMany("ItemTags")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DashboardApi.Entities.Tag", "Tag")
                        .WithMany("ItemTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DashboardApi.Entities.LessonCategory", b =>
                {
                    b.HasOne("DashboardApi.Entities.Category", "Category")
                        .WithMany("LessonCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DashboardApi.Entities.Lesson", "Lesson")
                        .WithMany("LessonCategories")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DashboardApi.Entities.LessonItem", b =>
                {
                    b.HasOne("DashboardApi.Entities.Item", "Item")
                        .WithMany("LessonItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DashboardApi.Entities.Lesson", "Lesson")
                        .WithMany("LessonItems")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
