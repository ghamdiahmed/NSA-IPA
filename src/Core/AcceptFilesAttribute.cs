using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace NsaIpa.Web.Core;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class AcceptFilesAttribute : ValidationAttribute, IClientModelValidator
{
    public string InvalidErrorMessage { get; set; } = "The attached file is invalid.";
    public string Extensions { get; set; }

    protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
    {
        IEnumerable<IFormFile> files = value is IFormFile formFile ? new[] { formFile } : value as IEnumerable<IFormFile>;


        if (files == null) return ValidationResult.Success;

        Extensions = Extensions.Replace(" ", "");

        bool checkHeader = false;

        foreach (IFormFile file in files)
        {
            if (Extensions.Split(',').Any(extension => file.FileName?.EndsWith(extension) == true))
            {
                if (FileHelpers.CheckFileHeader(file))
                    checkHeader = true;
                else
                {
                    checkHeader = false;
                    InvalidErrorMessage = $"File {file.FileName} incorrect";
                    break;
                }

            }
        }

        if (checkHeader) return ValidationResult.Success;

        return new ValidationResult(InvalidErrorMessage);
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes["data-val"] = "true";
        context.Attributes["data-val-acceptfiles"] = string.Format(ErrorMessageString, Extensions.Replace(".", ""));
        context.Attributes["data-val-acceptfiles-extensions"] = Extensions.Replace(" ", "");
        context.Attributes["accept"] = AcceptTypes();
    }

    private string AcceptTypes()
    {
        string[] extensions = Extensions.Split(',');

        List<string> mimtypes = new List<string>();

        foreach (string extension in extensions)
        {
            mimtypes.Add(extension.Trim() == ".fasta" ? ".fasta" : FileHelpers.GetContentType(extension));
        }


        return string.Join(", ", mimtypes);
    }
}