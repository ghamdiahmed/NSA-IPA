namespace NsaIpa.Web.Core;

public static class FileHelpers
{
    public const int TenMbToB = 10485760;

    private static readonly string[] SizeSuffixes = { "Byte", "KB", "MB", "GB" };
    private static readonly byte[] FastaHeader = { 62 };

    public static Dictionary<string, string> GetMimeTypes()
    {
        return new Dictionary<string, string>
        {
            {".fasta", "text/plain"}
        };
    }

    public static string GetContentType(string path)
    {
        var types = GetMimeTypes();
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return types[ext];
    }

    public static string CalcSize(long value, int decimalPlaces = 1)
    {
        if (value < 0) { return "-" + CalcSize(-value); }

        int i = 0;
        decimal dValue = (decimal)value;
        while (Math.Round(dValue, decimalPlaces) >= 1000)
        {
            dValue /= 1024;
            i++;
        }

        return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
    }

    public static bool CheckFileHeader(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();

        var flag = false;

        using var br = new BinaryReader(file.OpenReadStream());
        byte[] buffer;

        switch (extension)
        {
            case ".fasta":
                flag = true;
                break;
        }

        return flag;
    }
}