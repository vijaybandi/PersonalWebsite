using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using PersonalWebsite.Models;

namespace PersonalWebsite.Migrations
{
    [DbContext(typeof(DataDbContext))]
    partial class DataDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta8-15964")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PersonalWebsite.Models.Content", b =>
                {
                    b.Property<Guid>("ContentGuid")
                        .ValueGeneratedOnAdd()
                        .Annotation("Relational:GeneratedValueSql", "newsequentialid()");

                    b.Property<string>("InternalCaption")
                        .IsRequired()
                        .Annotation("MaxLength", 255);

                    b.HasKey("ContentGuid");
                });

            modelBuilder.Entity("PersonalWebsite.Models.Translation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ContentContentGuid");

                    b.Property<int>("ContentId");

                    b.Property<string>("ContentMarkup")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("State");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UrlName")
                        .IsRequired()
                        .Annotation("MaxLength", 200);

                    b.Property<int>("Version")
                        .Annotation("MaxLength", 10);

                    b.HasKey("Id");

                    b.Index("Version", "ContentId")
                        .Unique();

                    b.Index("Version", "UrlName")
                        .Unique();
                });

            modelBuilder.Entity("PersonalWebsite.Models.Translation", b =>
                {
                    b.HasOne("PersonalWebsite.Models.Content")
                        .WithMany()
                        .ForeignKey("ContentContentGuid");
                });
        }
    }
}
