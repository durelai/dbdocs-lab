using System;
using System.Collections.Generic;
using System.Text;
using DbDocsLab.App.Data;
using DbDocsLab.App.Models;
using Microsoft.EntityFrameworkCore;

namespace DbDocsLab.App.Repositories;

public class ProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);
        ArgumentException.ThrowIfNullOrWhiteSpace(project.Name);

        _context.Projects.Add(project);
        _context.SaveChanges();
    }

    public List<Project> GetAll()
    {
        return _context.Projects.AsNoTracking().OrderBy(p => p.Id).ToList();
    }

    public Project? GetById(int id)
    {
        return _context.Projects.Include(p => p.DocumentationVersions).FirstOrDefault(p => p.Id == id);
    }

    public bool UpdateName(int id, string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);

        var project = _context.Projects.Find(id);

        if (project is null)
        {
            return false;
        }

        project.Name = newName;

        _context.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var project = _context.Projects.Find(id);

        if (project is null)
        {
            return false;
        }

        _context.Projects.Remove(project);
        _context.SaveChanges();

        return true;
    }
}
