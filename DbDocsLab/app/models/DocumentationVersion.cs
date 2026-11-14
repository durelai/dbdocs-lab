using System;
using System.Collections.Generic;
using System.Text;

namespace DbDocsLab.App.Models;

public class DocumentationVersion
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public Project Project { get; set; } = null!;

    public string CommitSha { get; set; } = string.Empty;

    public string DocumentationText { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}