using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

using DbDocsLab.App.Models;
using Microsoft.EntityFrameworkCore;

namespace DbDocsLab.App.Data;

public class AppDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<DocumentationVersion> DocumentationVersions => Set<DocumentationVersion>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        string? connectionString =
            Environment.GetEnvironmentVariable(
                "DBDOCS_CONNECTION",
                EnvironmentVariableTarget.User);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Не задана переменная DBDOCS_CONNECTION.");
        }

        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var project = modelBuilder.Entity<Project>();

        project.ToTable("projects");

        project.HasKey(p => p.Id);

        project.Property(p => p.Name).IsRequired().HasMaxLength(200);

        project.Property(p => p.RepositoryUrl).IsRequired().HasMaxLength(2048);

        project.Property(p => p.TrackedBranch).IsRequired().HasMaxLength(200);

        var version = modelBuilder.Entity<DocumentationVersion>();

        version.ToTable("documentation_versions");

        version.HasKey(v => v.Id);

        version.Property(v => v.CommitSha).IsRequired().HasMaxLength(64);

        version.Property(v => v.DocumentationText).IsRequired();

        version.HasOne(v => v.Project).WithMany(p => p.DocumentationVersions).HasForeignKey(v => v.ProjectId).OnDelete(DeleteBehavior.Cascade);
    }
}