﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notes.Models;

namespace Notes.Migrations
{
    [DbContext(typeof(NotesContext))]
    partial class NotesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Notes.Models.CheckList", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsChecked");

                    b.Property<int?>("NoteID");

                    b.Property<string>("value");

                    b.HasKey("ID");

                    b.HasIndex("NoteID");

                    b.ToTable("CheckList");
                });

            modelBuilder.Entity("Notes.Models.Labels", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("NoteID");

                    b.Property<string>("value");

                    b.HasKey("ID");

                    b.HasIndex("NoteID");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("Notes.Models.Note", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsPinned");

                    b.Property<string>("plainText");

                    b.Property<string>("title");

                    b.HasKey("ID");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("Notes.Models.CheckList", b =>
                {
                    b.HasOne("Notes.Models.Note")
                        .WithMany("checklist")
                        .HasForeignKey("NoteID");
                });

            modelBuilder.Entity("Notes.Models.Labels", b =>
                {
                    b.HasOne("Notes.Models.Note")
                        .WithMany("label")
                        .HasForeignKey("NoteID");
                });
#pragma warning restore 612, 618
        }
    }
}
