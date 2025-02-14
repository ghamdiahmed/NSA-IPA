using Bio.Web.Blast;
using NsaIpa.Web.Models;
using NsaIpa.Web.Models.NsaIpi;
using IronPython.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace NsaIpa.Web.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    public IActionResult Index()
    {
        HomeModel model = new HomeModel();

        model.Blast = new BlastModel
        {
            Reference = ">reference\r\ntgcgcgccgcacgtcgcttttattcgccgtcgccgtacccaccgcagcacacgcaactagtcgccgtcgccgtccacacacgcaactccaaatttcaccc",
            Query = ">query\r\nCGTCCACACACGCAACTCCAA"
        };

        return View(model);
    }


    [HttpPost]
    [Route("~/nsa")]
    public IActionResult Nsa(HomeModel model)
    {
        ModelState.Remove(nameof(model.Blast));

        if (ModelState.IsValid)
        {
            NsaResultModel resultModel = new NsaResultModel();

            ScriptEngine _engine = Python.CreateEngine();
            ScriptScope _scope = _engine.CreateScope();

            _engine.Execute($"values = '{model.Form.Values}'", _scope);
            _engine.Execute("value_list = values.split(\",\")", _scope);
            _engine.Execute("n = len(value_list)", _scope);
            _engine.Execute("unit_A = value_list[:n//2]", _scope);
            _engine.Execute("unit_B = value_list[n//2:]", _scope);
            _engine.Execute("unit_A_half_B = unit_A + unit_B[:n//4]", _scope);
            _engine.Execute("unit_B_half_A = unit_A[:n//4] + unit_B ", _scope);


            foreach (object o in _scope.GetVariable("unit_A"))
            {
                if (o != null)
                {
                    resultModel.A.Add(((string)o).Trim());
                }
            }

            foreach (object o in _scope.GetVariable("unit_B"))
            {
                if (o != null)
                {
                    resultModel.B.Add(((string)o).Trim());
                }
            }

            foreach (object o in _scope.GetVariable("unit_A_half_B"))
            {
                if (o != null)
                {
                    resultModel.AHB.Add(((string)o).Trim());
                }
            }

            foreach (object o in _scope.GetVariable("unit_B_half_A"))
            {
                if (o != null)
                {
                    resultModel.BHA.Add(((string)o).Trim());
                }
            }


            _engine.Execute(@"
import itertools
from itertools import combinations
from itertools import chain, repeat, count, islice
from collections import Counter
import math
", _scope);

            _engine.Execute(@"
A_pairs = list(itertools.combinations(unit_A, 2))
B_pairs = list(itertools.combinations(unit_B, 2))
A_half_B_pairs = list(itertools.combinations(unit_A_half_B, 2))
unit_B_half_A_pairs = list(itertools.combinations(unit_B_half_A, 2))
", _scope);

            _engine.Execute(@"
upper_half_A = unit_A[:n//4]
lower_half_A = unit_A[n//4:]

upper_half_B = unit_B[:n//4]
lower_half_B = unit_B[n//4:]

Lower_half = lower_half_A + lower_half_B
lower_pairs = list(itertools.combinations(Lower_half, 2))

for pair in lower_pairs[:]:
    if pair[0] in unit_A and pair[1] in unit_A:
      lower_pairs.remove(pair)
    elif pair[0] in unit_B and pair[1] in unit_B:
      lower_pairs.remove(pair)

lower_half_B_BHA_AHB = unit_B[n//4:] + unit_B_half_A[n//4:] + unit_A_half_B[n//4:]
", _scope);



            resultModel.None1 = _engine.Execute<int>("len(lower_pairs)", _scope);
            resultModel.None2 = _engine.Execute<string>("str(lower_pairs)", _scope);


            _engine.Execute(@"purple_green_orange_without_rep=set(B_pairs) & set(unit_B_half_A_pairs)& set(A_half_B_pairs)", _scope);

            resultModel.Bcd1 = _engine.Execute<int>("len(purple_green_orange_without_rep)", _scope);
            resultModel.Bcd2 = _engine.Execute<string>("str(purple_green_orange_without_rep)", _scope);

            _engine.Execute(@"
red_green_orange_without_rep=set(A_pairs) & set(unit_B_half_A_pairs)& set(A_half_B_pairs)
lower_A_BHA_AHB = unit_A[n//4:] + unit_B_half_A[n//4:] + unit_A_half_B[n//4:]
", _scope);

            resultModel.Abc1 = _engine.Execute<int>("len(red_green_orange_without_rep)", _scope);
            resultModel.Abc2 = _engine.Execute<string>("str(red_green_orange_without_rep)", _scope);


            _engine.Execute(@"
purple_green_without_rep = set(B_pairs) & set(unit_B_half_A_pairs)
purple_green_orange_pairs = set(B_pairs) & set(unit_B_half_A_pairs) & set(A_half_B_pairs)
purple_green_without_rep = purple_green_without_rep - purple_green_orange_pairs
lower_B_BHA = unit_B[n//4:] + unit_B_half_A[n//4:]
", _scope);

            resultModel.Cd1 = _engine.Execute<int>("len(purple_green_without_rep)", _scope);
            resultModel.Cd2 = _engine.Execute<string>("str(purple_green_without_rep)", _scope);


            _engine.Execute(@"
red_orange_pairs = set(A_pairs) & set(A_half_B_pairs)
red_green_orange_pairs = set(A_pairs) & set(unit_B_half_A_pairs)& set(A_half_B_pairs)
red_orange_without_rep = red_orange_pairs - red_green_orange_pairs
lower_A_AHB = unit_A[n//4:] + unit_A_half_B[n//4:]
", _scope);
            resultModel.Ab1 = _engine.Execute<int>("len(red_orange_without_rep)", _scope);
            resultModel.Ab2 = _engine.Execute<string>("str(red_orange_without_rep)", _scope);

            _engine.Execute(@"
green_orange_without_rep=set(unit_B_half_A_pairs) & set(A_half_B_pairs)
green_orange_without_rep_list = list(green_orange_without_rep)
# Remove pairs that are already included in either unit A or unit B
for pair in green_orange_without_rep_list[:]:
  if pair[0] in unit_A and pair[1] in unit_A:
    green_orange_without_rep_list.remove(pair)
  elif pair[0] in unit_B and pair[1] in unit_B:
    green_orange_without_rep_list.remove(pair)

lower_BHA_AHB = unit_B_half_A[n//4:] + unit_A_half_B[n//4:]
", _scope);

            resultModel.Bc1 = _engine.Execute<int>("len(green_orange_without_rep_list)", _scope);
            resultModel.Bc2 = _engine.Execute<string>("str(green_orange_without_rep_list)", _scope);


            _engine.Execute(@"
upper_half_A = unit_A[:n//4]

lower_half_B = unit_B[n//4:]

halfs_green = upper_half_A + lower_half_B
lower_B_pairs = list(itertools.combinations(lower_half_B, 2))
green_pairs = list(itertools.combinations(halfs_green, 2))

for pair in green_pairs[:]:
  if pair in A_half_B_pairs:
    green_pairs.remove(pair)
  elif pair in lower_B_pairs:
      green_pairs.remove(pair)

lower_BHA = unit_B_half_A[n//4:]
", _scope);
            resultModel.C1 = _engine.Execute<int>("len(green_pairs)", _scope);
            resultModel.C2 = _engine.Execute<string>("str(green_pairs)", _scope);


            _engine.Execute(@"
lower_half_A = unit_A[n//4:]

upper_half_B = unit_B[:n//4]

halfs_orange = lower_half_A + upper_half_B

orange_pairs = list(itertools.combinations(halfs_orange, 2))
lower_A_pairs = list(itertools.combinations(lower_half_A, 2))

for pair in orange_pairs[:]:
  if pair in unit_B_half_A_pairs:
    orange_pairs.remove(pair)
  elif pair in lower_A_pairs:
      orange_pairs.remove(pair)

lower_AHB = unit_A_half_B[n//4:]
", _scope);
            resultModel.B1 = _engine.Execute<int>("len(orange_pairs)", _scope);
            resultModel.B2 = _engine.Execute<string>("str(orange_pairs)", _scope);

            resultModel.NsbCount = model.Form.NsbCount.Value;


            return View("NsaResult", resultModel);



        }

        return View("Index", model);

    }

    [HttpPost]
    [Route("~/nsp")]
    public async Task<IActionResult> Nsp(HomeModel model)
    {
        try
        {
            ModelState.Remove(nameof(model.Form));

            if (model.Blast.ReferenceIsText)
            {
                if (model.Blast.Reference != null && !model.Blast.Reference.StartsWith(">"))
                    ModelState.AddModelError(nameof(model.Blast.Reference),
                        "The reference must be in correct format, starting with symbol >");
            }

            if (model.Blast.QueryIsText)
            {
                if (model.Blast.Query != null && !model.Blast.Query.StartsWith(">"))
                    ModelState.AddModelError(nameof(model.Blast.Query),
                        "The query must be in correct format, starting with symbol >");
            }

            if (!ModelState.IsValid)
                return View("Index", model);

            if (!Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "App_Data", "blast", "temp")))
                Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "App_Data", "blast",
                    "temp"));

            string workTempFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "App_Data", "blast", "temp",
                Guid.NewGuid().ToString("N"));

            if (!Directory.Exists(workTempFolder))
                Directory.CreateDirectory(workTempFolder);

            string referenceFilePath = Path.Combine(workTempFolder, "reference.fasta");
            string queryFilePath = Path.Combine(workTempFolder, "query.fasta");
            string outFilePathReferenceDb = Path.Combine(workTempFolder, "reference_db");
            string resultAsn = Path.Combine(workTempFolder, "results.asn");
            string resultXml = Path.Combine(workTempFolder, "results.xml");
            string resultTxt = Path.Combine(workTempFolder, "results.txt");


            if (System.IO.File.Exists(referenceFilePath)) System.IO.File.Delete(referenceFilePath);
            if (System.IO.File.Exists(queryFilePath)) System.IO.File.Delete(queryFilePath);

            if (model.Blast.ReferenceIsText)
            {
                await System.IO.File.WriteAllTextAsync(referenceFilePath, model.Blast.Reference);
            }
            else
            {
                await using var stream = System.IO.File.Create(referenceFilePath);
                await model.Blast.ReferenceFile?.CopyToAsync(stream);
            }

            if (model.Blast.QueryIsText)
            {
                await System.IO.File.WriteAllTextAsync(queryFilePath, model.Blast.Query);
            }
            else
            {
                await using var stream = System.IO.File.Create(queryFilePath);
                await model.Blast.QueryFile.CopyToAsync(stream);
            }

            using Process blastDb = new Process();
            blastDb.StartInfo.FileName = Path.Combine(_webHostEnvironment.ContentRootPath, "App_Data", "blast",
                "makeblastdb.exe");
            blastDb.StartInfo.UseShellExecute = false;
            blastDb.StartInfo.CreateNoWindow = false;
            blastDb.StartInfo.RedirectStandardError = true;
            blastDb.StartInfo.RedirectStandardOutput = true;
            blastDb.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            blastDb.EnableRaisingEvents = true;

            var errorsDb = new StringBuilder();
            var outputDb = new StringBuilder();
            var hadErrorsDb = false;

            blastDb.ErrorDataReceived += (s, d) =>
            {
                if (d.Data != null && d.Data.StartsWith("Error"))
                {
                    if (!hadErrorsDb)
                    {
                        hadErrorsDb = !String.IsNullOrEmpty(d.Data);
                    }

                    errorsDb.Append(d.Data);
                }
            };

            string dbArguments = @" -in " + referenceFilePath + " -out " + outFilePathReferenceDb;
            dbArguments += " -dbtype nucl";

            blastDb.StartInfo.Arguments = dbArguments;
            blastDb.Start();
            blastDb.BeginErrorReadLine();
            blastDb.BeginOutputReadLine();
            blastDb.WaitForExit();

            string stderrDb = errorsDb.ToString();

            if (blastDb.ExitCode != 0 || hadErrorsDb)
            {
                ModelState.AddModelError("", stderrDb);
                return View("Index", model);
            }


            using Process blastN = new Process();
            blastN.StartInfo.FileName =
                Path.Combine(_webHostEnvironment.ContentRootPath, "App_Data", "blast", "blastn.exe");
            blastN.StartInfo.UseShellExecute = false;
            blastN.StartInfo.CreateNoWindow = false;
            blastN.StartInfo.RedirectStandardError = true;
            blastN.StartInfo.RedirectStandardOutput = true;
            blastN.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            blastN.EnableRaisingEvents = true;

            var errorsN = new StringBuilder();
            var hadErrorsN = false;

            blastN.ErrorDataReceived += (s, d) =>
            {

                if (d.Data != null && d.Data.StartsWith("Error"))
                {
                    if (!hadErrorsN)
                    {
                        hadErrorsN = !String.IsNullOrEmpty(d.Data);
                    }

                    errorsN.Append(d.Data);
                }



            };

            string nArguments = " ";

            if (!string.IsNullOrEmpty(model.Blast.TaskType))
                nArguments += $" -task {model.Blast.TaskType}";
            else
                nArguments += $" -task megablast";


            nArguments += " -query " + queryFilePath + " -db " + outFilePathReferenceDb + " -out " + resultAsn +
                          "  -outfmt 11";

            if (model.Blast.WordSize.HasValue)
                nArguments += $" -word_size {model.Blast.WordSize}";

            if (model.Blast.GapOpen.HasValue)
                nArguments += $" -gapopen {model.Blast.GapOpen}";

            if (model.Blast.GapExtend.HasValue)
                nArguments += $" -gapextend {model.Blast.GapExtend}";

            if (model.Blast.Penalty.HasValue)
                nArguments += $" -penalty {model.Blast.Penalty}";

            if (model.Blast.Reward.HasValue)
                nArguments += $" -reward {model.Blast.Reward}";


            if (model.Blast.SortHit.HasValue)
                nArguments += $" -sorthits {model.Blast.SortHit}";

            blastN.StartInfo.Arguments = nArguments;
            blastN.Start();
            blastN.BeginErrorReadLine();
            blastN.BeginOutputReadLine();
            blastN.WaitForExit();

            string stderrN = errorsN.ToString();

            if (blastN.ExitCode != 0 || hadErrorsN)
            {
                ModelState.AddModelError("", stderrN);
                return View("Index", model);
            }


            using Process blastFormatterXml = new Process();
            blastFormatterXml.StartInfo.FileName = Path.Combine(_webHostEnvironment.ContentRootPath, "App_Data",
                "blast", "blast_formatter.exe");
            blastFormatterXml.StartInfo.UseShellExecute = false;
            blastFormatterXml.StartInfo.CreateNoWindow = false;
            blastFormatterXml.StartInfo.RedirectStandardError = true;
            blastFormatterXml.StartInfo.RedirectStandardOutput = true;
            blastFormatterXml.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            blastFormatterXml.EnableRaisingEvents = true;

            var errorsFormatterXml = new StringBuilder();
            var hadErrorsFormatterXml = false;

            blastFormatterXml.ErrorDataReceived += (s, d) =>
            {

                if (d.Data != null && d.Data.StartsWith("Error"))
                {
                    if (!hadErrorsFormatterXml)
                    {
                        hadErrorsFormatterXml = !String.IsNullOrEmpty(d.Data);
                    }

                    errorsFormatterXml.Append(d.Data);
                }



            };

            blastFormatterXml.StartInfo.Arguments = " -archive " + resultAsn + " -outfmt 5 -out " + resultXml;
            blastFormatterXml.Start();
            blastFormatterXml.BeginErrorReadLine();
            blastFormatterXml.BeginOutputReadLine();
            blastFormatterXml.WaitForExit();

            string stderrFormatterXml = errorsFormatterXml.ToString();

            if (blastFormatterXml.ExitCode != 0 || hadErrorsFormatterXml)
            {
                ModelState.AddModelError("", stderrFormatterXml);
                return View("Index", model);
            }


            using Process blastFormatterTxt = new Process();
            blastFormatterTxt.StartInfo.FileName = Path.Combine(_webHostEnvironment.ContentRootPath, "App_Data",
                "blast", "blast_formatter.exe");
            blastFormatterTxt.StartInfo.UseShellExecute = false;
            blastFormatterTxt.StartInfo.CreateNoWindow = false;
            blastFormatterTxt.StartInfo.RedirectStandardError = true;
            blastFormatterTxt.StartInfo.RedirectStandardOutput = true;
            blastFormatterTxt.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            blastFormatterTxt.EnableRaisingEvents = true;

            var errorsFormatterTxt = new StringBuilder();
            var hadErrorsFormatterTxt = false;

            blastFormatterTxt.ErrorDataReceived += (s, d) =>
            {

                if (d.Data != null && d.Data.StartsWith("Error"))
                {
                    if (!hadErrorsFormatterTxt)
                    {
                        hadErrorsFormatterTxt = !String.IsNullOrEmpty(d.Data);
                    }

                    errorsFormatterTxt.Append(d.Data);
                }



            };

            blastFormatterTxt.StartInfo.Arguments = " -archive " + resultAsn + " -outfmt 0 -out " + resultTxt;
            blastFormatterTxt.Start();
            blastFormatterTxt.BeginErrorReadLine();
            blastFormatterTxt.BeginOutputReadLine();
            blastFormatterTxt.WaitForExit();

            string stderrFormatterTxt = errorsFormatterTxt.ToString();

            if (blastFormatterTxt.ExitCode != 0 || hadErrorsFormatterTxt)
            {
                ModelState.AddModelError("", stderrFormatterTxt);
                return View("Index", model);
            }

            ResultModel resultModel = new ResultModel();

            using (StreamReader sr = new StreamReader(resultXml))
            {
                IBlastParser blastParser = new BlastXmlParser();
                List<BlastResult> blastres = blastParser.Parse(sr.BaseStream).ToList();


                if (blastres.Count > 0)
                {
                    BlastXmlMetadata meta = blastres[0].Metadata;

                    int i = 0;

                    foreach (Hit hit in blastres[0].Records[0].Hits)
                    {
                        string contig = hit.Def;
                        for (int j = 0; j < hit.Hsps.Count; j++)
                        {
                            Hsp hsp = hit.Hsps[j];


                            BlastHitModel bhit = new BlastHitModel();
                            bhit.seqID = Convert.ToInt64(hit.Accession);
                            bhit.Contig = contig;
                            bhit.Descr = hit.Def;
                            bhit.Score = hsp.BitScore.ToString("N2");
                            bhit.Evalue = hsp.EValue.ToString("E2");
                            bhit.HitStart = hsp.HitStart.ToString();
                            bhit.HitEnd = hsp.HitEnd.ToString();
                            bhit.Align = hsp.AlignmentLength.ToString();
                            bhit.Frame = hsp.QueryFrame > 0 ? "+" : "-";
                            bhit.Frame += @"/";
                            bhit.Frame += hsp.HitFrame > 0 ? "+" : "-";
                            resultModel.Blasts.Add(bhit);
                        }

                        i++;
                    }
                }
            }

            using (StreamReader sr = new StreamReader(resultTxt, Encoding.UTF8, true))
            {
                resultModel.Txt = await sr.ReadToEndAsync();
            }

            Directory.Delete(workTempFolder, true);

            if (!string.IsNullOrEmpty(resultModel.Txt))
            {
                resultModel.Txt = RemoveBlastnHeader(resultModel.Txt);
                resultModel.Txt = RemovePathUsingRegex(resultModel.Txt);
            }

            return View("NspResult", resultModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            var properties = new Dictionary<string, string>
            {
                {"nsp", e.Message}
            };

            return RedirectToAction("Index");
        }
    }

    private string RemoveBlastnHeader(string text)
    {
        string pattern = @"BLASTN 2\.14\.1\+.*?(?=Database:)";
        return Regex.Replace(text, pattern, "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }

    private string RemovePathUsingRegex(string text)
    {
        text = text.Replace("\\\r\nreference.fasta", "\\reference.fasta");

        string pattern = @"(Database:\s*)(?:.+?\\temp\\[^\\]+\\)?reference\.fasta";

        return Regex.Replace(text, pattern, "Database: reference.fasta", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }
}