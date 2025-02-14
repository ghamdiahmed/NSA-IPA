#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NsaIpa.Web.Models.NsaIpi;

public class FormModel
{
    [Display(Name = "Enter all the primers in the multiplex PCR pool:", Prompt = "FWR_1,FWR_2,FWR_3,REV_1,REV_2,REV_3")]
    [Required(ErrorMessage = "Values is Required !")]
    [RegularExpression(@"^[0-9a-zA-Z,_-]*$", ErrorMessage = "Please enter a valid formula ex(x,x,x,x,x) !")]
    public string? Values { get; set; }

    [DisplayName("Enter the number of NSA observed in the multiplex PCR pool:")]
    [Required(ErrorMessage = "Number of NSA observed is Required !")]
    [Range(1, int.MaxValue)]
    public int? NsbCount { get; set; }
    public bool IsResult { get; set; }
}