using NsaIpa.Web.Core;

namespace NsaIpa.Web.Models.NsaIpi;

public class NsaResultModel
{
    public NsaResultModel()
    {
        A = new List<string>();
        B = new List<string>();
        AHB = new List<string>();
        BHA = new List<string>();
    }

    public int NsbCount { get; set; }

    public string None0
    {
        get
        {
            if (!string.IsNullOrEmpty(None2))
            {
                string[] s = None2.Split(',');

                List<string> numbers = new List<string>();

                foreach (string ff in s)
                    numbers.Add(ff.Replace("(", "").Replace(")", "").Replace("'", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace("{", "").Trim());

                List<string> numbersR = new List<string>();

                foreach (string number in numbers)
                {
                    if (!numbersR.Contains(number))
                    {
                        numbersR.Add(number);
                    }
                }

                var anumbersR = numbersR.ToArray();

                Array.Sort(anumbersR, new AlphanumComparatorFast());

                return string.Join(",", anumbersR);
            }

            return string.Empty;
        }
    }
    public int None1 { get; set; }
    public string None2 { get; set; }

    public string Bcd0
    {
        get
        {
            if (!string.IsNullOrEmpty(Bcd2))
            {
                string[] s = Bcd2.Split(',');

                List<string> numbers = new List<string>();

                foreach (string ff in s)
                    numbers.Add(ff.Replace("(", "").Replace(")", "").Replace("'", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace("{", "").Trim());

                List<string> numbersR = new List<string>();

                foreach (string number in numbers)
                {
                    if (!numbersR.Contains(number))
                    {
                        numbersR.Add(number);
                    }
                }

                var anumbersR = numbersR.ToArray();

                Array.Sort(anumbersR, new AlphanumComparatorFast());

                return string.Join(",", anumbersR);
            }

            return string.Empty;
        }
    }
    public int Bcd1 { get; set; }
    public string Bcd2 { get; set; }

    public string Abc0
    {
        get
        {
            if (!string.IsNullOrEmpty(Abc2))
            {
                string[] s = Abc2.Split(',');

                List<string> numbers = new List<string>();

                foreach (string ff in s)
                    numbers.Add(ff.Replace("(", "").Replace(")", "").Replace("'", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace("{", "").Trim());

                List<string> numbersR = new List<string>();

                foreach (string number in numbers)
                {
                    if (!numbersR.Contains(number))
                    {
                        numbersR.Add(number);
                    }
                }

                var anumbersR = numbersR.ToArray();

                Array.Sort(anumbersR, new AlphanumComparatorFast());

                return string.Join(",", anumbersR);
            }

            return string.Empty;
        }
    }

    public int Abc1 { get; set; }
    public string Abc2 { get; set; }

    public string Cd0
    {
        get
        {
            if (!string.IsNullOrEmpty(Cd2))
            {
                string[] s = Cd2.Split(',');

                List<string> numbers = new List<string>();

                foreach (string ff in s)
                    numbers.Add(ff.Replace("(", "").Replace(")", "").Replace("'", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace("{", "").Trim());

                List<string> numbersR = new List<string>();

                foreach (string number in numbers)
                {
                    if (!numbersR.Contains(number))
                    {
                        numbersR.Add(number);
                    }
                }

                var anumbersR = numbersR.ToArray();

                Array.Sort(anumbersR, new AlphanumComparatorFast());

                return string.Join(",", anumbersR);
            }

            return string.Empty;
        }
    }

    public int Cd1 { get; set; }
    public string Cd2 { get; set; }

    public string Ab0
    {
        get
        {
            if (!string.IsNullOrEmpty(Ab2))
            {
                string[] s = Ab2.Split(',');

                List<string> numbers = new List<string>();

                foreach (string ff in s)
                    numbers.Add(ff.Replace("(", "").Replace(")", "").Replace("'", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace("{", "").Trim());

                List<string> numbersR = new List<string>();

                foreach (string number in numbers)
                {
                    if (!numbersR.Contains(number))
                    {
                        numbersR.Add(number);
                    }
                }

                var anumbersR = numbersR.ToArray();

                Array.Sort(anumbersR, new AlphanumComparatorFast());

                return string.Join(",", anumbersR);
            }

            return string.Empty;
        }
    }

    public int Ab1 { get; set; }
    public string Ab2 { get; set; }

    public string Bc0
    {
        get
        {
            if (!string.IsNullOrEmpty(Bc2))
            {
                string[] s = Bc2.Split(',');

                List<string> numbers = new List<string>();

                foreach (string ff in s)
                    numbers.Add(ff.Replace("(", "").Replace(")", "").Replace("'", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace("{", "").Trim());

                List<string> numbersR = new List<string>();

                foreach (string number in numbers)
                {
                    if (!numbersR.Contains(number))
                    {
                        numbersR.Add(number);
                    }
                }

                var anumbersR = numbersR.ToArray();

                Array.Sort(anumbersR, new AlphanumComparatorFast());

                return string.Join(",", anumbersR);
            }

            return string.Empty;
        }
    }
    public int Bc1 { get; set; }
    public string Bc2 { get; set; }

    public string C0
    {
        get
        {
            if (!string.IsNullOrEmpty(C2))
            {
                string[] s = C2.Split(',');

                List<string> numbers = new List<string>();

                foreach (string ff in s)
                    numbers.Add(ff.Replace("(", "").Replace(")", "").Replace("'", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace("{", "").Trim());

                List<string> numbersR = new List<string>();

                foreach (string number in numbers)
                {
                    if (!numbersR.Contains(number))
                    {
                        numbersR.Add(number);
                    }
                }

                var anumbersR = numbersR.ToArray();

                Array.Sort(anumbersR, new AlphanumComparatorFast());

                return string.Join(",", anumbersR);
            }

            return string.Empty;
        }
    }
    public int C1 { get; set; }
    public string C2 { get; set; }

    public string B0
    {
        get
        {
            if (!string.IsNullOrEmpty(B2))
            {
                string[] s = B2.Split(',');

                List<string> numbers = new List<string>();

                foreach (string ff in s)
                    numbers.Add(ff.Replace("(", "").Replace(")", "").Replace("'", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace("{", "").Trim());

                List<string> numbersR = new List<string>();

                foreach (string number in numbers)
                {
                    if (!numbersR.Contains(number))
                    {
                        numbersR.Add(number);
                    }
                }

                var anumbersR = numbersR.ToArray();

                Array.Sort(anumbersR, new AlphanumComparatorFast());

                return string.Join(",", anumbersR);
            }

            return string.Empty;
        }
    }
    public int B1 { get; set; }
    public string B2 { get; set; }

    public IList<string> A { get; set; }
    public IList<string> B { get; set; }
    public IList<string> AHB { get; set; }
    public IList<string> BHA { get; set; }
}