#nullable enable

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NsaIpa.Web.Core;

namespace NsaIpa.Web.Models;

public class BlastModel
{

    [DisplayName("Reference: ")]
    public bool ReferenceIsText { get; set; } = true;

    [DisplayName("Reference:")]
    [RequiredIfTrue("ReferenceIsText", ErrorMessage = "Reference is Required")]
    public string? Reference { get; set; }


    [RequiredIfFalse("ReferenceIsText", ErrorMessage = "Reference file required.")]
    [AcceptFiles(Extensions = ".fasta", ErrorMessage = "Reference file type must be {0} only.", InvalidErrorMessage = "The reference file is invalid.")]
    [DataType(DataType.Upload)]
    public IFormFile? ReferenceFile { get; set; }


    [DisplayName("Query: ")]
    public bool QueryIsText { get; set; } = true;

    [DisplayName("Query:")]
    [RequiredIfTrue("QueryIsText", ErrorMessage = "Query is Required")]
    public string? Query { get; set; }

    [RequiredIfFalse("QueryIsText", ErrorMessage = "Query file required.")]
    [AcceptFiles(Extensions = ".fasta", ErrorMessage = "Query file type must be {0} only.", InvalidErrorMessage = "The query file is invalid.")]
    [DataType(DataType.Upload)]
    public IFormFile? QueryFile { get; set; }

    [DisplayName("Strand Type:")]
    public List<SelectListItem> StrandTypes => new()
    {
        new SelectListItem { Value = "both", Text = "Both (Default)" },
        new SelectListItem { Value = "minus", Text = "Minus" },
        new SelectListItem { Value = "plus", Text = "Plus" }
    };

    public string TaskType { get; set; } = "megablast";

    [DisplayName("Task Types:")]
    public List<SelectListItem> TaskTypes => new()
    {
        new SelectListItem { Value = "blastn", Text = "BLASTN" },
        new SelectListItem { Value = "blastn-short", Text = "BLASTN-SHORT" },
        new SelectListItem { Value = "dc-megablast", Text = "DC-MEGABLAST" },
        new SelectListItem { Value = "megablast", Text = "MEGABLAST (Default)", Selected = true},
        new SelectListItem { Value = "rmblastn", Text = "RMBLAST " },
    };

    [DisplayName("EValue:")] public int? Evalue { get; set; } = 10;

    [DisplayName("Word Size:")]
    [Range(4, int.MaxValue)]
    public int? WordSize { get; set; } = 7;

    [DisplayName("Gap Open:")] public int? GapOpen { get; set; } = 5;

    [DisplayName("Gap Extend:")] public int? GapExtend { get; set; } = 2;

    [DisplayName("Penalty:")]
    [Range(int.MinValue, 0)]
    public int? Penalty { get; set; } = -1;

    [DisplayName("Reward:")]
    [Range(0, int.MaxValue)]
    public int? Reward { get; set; } = 1;

    [Range(0, 4)]
    public int? SortHit { get; set; }

    [DisplayName("Sort Hits:")]
    public List<SelectListItem> SortHits => new()
    {
        new SelectListItem { Value = "", Text = "" },
        new SelectListItem { Value = "0", Text = "Sort by evalue" },
        new SelectListItem { Value = "1", Text = "Sort by bit score" },
        new SelectListItem { Value = "2", Text = "Sort by total score" },
        new SelectListItem { Value = "3", Text = "Sort by percent identity" },
        new SelectListItem { Value = "4", Text = "Sort by query coverage" }
    };
}