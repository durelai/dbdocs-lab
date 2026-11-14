using System;
using System.Collections.Generic;
using System.Text;

namespace DbDocsLab.App.Models;

public class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string RepositoryUrl { get; set; } = string.Empty;

    public string TrackedBranch { get; set; } = "main";

    public List<DocumentationVersion> DocumentationVersions { get; set; } = new();
}
