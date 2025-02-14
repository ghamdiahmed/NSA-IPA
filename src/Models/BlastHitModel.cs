namespace NsaIpa.Web.Models;

public class BlastHitModel
{
    public long seqID { get; set; }
    public string Contig { get; set; }
    public string Descr { get; set; }
    public string Score { get; set; }
    public string Evalue { get; set; }
    public string Align { get; set; }
    public string HitStart { get; set; }
    public string HitEnd { get; set; }
    public string Frame { get; set; }
}