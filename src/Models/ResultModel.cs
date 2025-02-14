using System.ComponentModel;

namespace NsaIpa.Web.Models;

public class ResultModel
{
    public ResultModel()
    {
        Blasts = new List<BlastHitModel>();
    }

    [DisplayName("Result:")]
    public List<BlastHitModel> Blasts { get; set; }

    [DisplayName("Result:")]
    public string Txt { get; set; }
}