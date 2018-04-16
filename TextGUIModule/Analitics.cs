using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NormalText;

namespace TextGUIModule
{
    class Analitics
    {
        private  List<string> compliteCodeFirst = new List<string>();
        private  List<string> compliteCodeSecond = new List<string>();
        private Normal parsText;
        public void Accect(string language, string path)
        {
            parsText = new Normal(path);
            if (language == "C#")
            {
                TokkingSharp sharp = new TokkingSharp();
                compliteCodeFirst = parsText.normalize();
                sharp.Tokening(compliteCodeFirst);
            }
            else if (language== "Java")
            {
                TokkingJava java = new TokkingJava();
                compliteCodeFirst = parsText.normalize();
                java.Tokening(compliteCodeFirst);
            }
            else if (language == "C++")
            {
                TokkingСPP cpp = new TokkingСPP();
                compliteCodeFirst = parsText.normalize();
                cpp.Tokening(compliteCodeFirst);
            }
            
        }

        public void SetCodeMain(string code)
        {
            compliteCodeFirst.Clear();
            for (int i = 0; i < code.Length; i++)
            {
                compliteCodeFirst.Add(code[i].ToString());
            }
        }
        public void SetCodeChild(string code)
        {
            compliteCodeSecond.Clear();
            for (int i = 0; i < code.Length; i++)
            {
                compliteCodeSecond.Add(code[i].ToString());
            }
        }
        public List<string> InserToDB()
        {
            List<string> grams = new List<string>();
            for (int i = 0; i < compliteCodeFirst.Count - 2; i++)
            {
                string threeGram = compliteCodeFirst[i] + compliteCodeFirst[i + 1] + compliteCodeFirst[i + 2];
                grams.Add(threeGram);
                
            }

            return grams;
        }

        public string FileName(string path)
        {
            string result;

            result = Path.GetFileName(path);
            return result;
        }
        public string GetVersion(string path)
        {
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(path);
            return myFileVersionInfo.FileVersion;
        }
        public string GetHash(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                SHA256Managed sha = new SHA256Managed();
                byte[] hash = sha.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
        public string GetNormalizeCode()
        {
            string normalJoin = string.Join("", compliteCodeFirst.ToArray());
            return normalJoin;
        }
        private  void TransInHash(SortedSet<string> set, SortedSet<int> hashSet)
        {
            for (int i = 0; i < set.Count; i++)
            {
                int hash = 0;
                string k = set.ElementAt(i).Replace(" ", "");
                int n = 1;
                for (int j = 0; j < k.Length; j++)
                {
                    hash += (int)Math.Pow((k[j] * 31), k.Length - j - 1);
                    n++;
                }
                hashSet.Add(hash);
            }
        }
        private  void GetGrumWShil(ICollection<string> set, List<string> dirList)
        {
            for (int i = 0; i < dirList.Count - 3; i++)
            {
                List<string> ls = new List<string>();
                for (int j = 0; j < 3; ++j) ls.Add(dirList[i + j]);
                string s = string.Join(" ", ls.ToArray());
                set.Add(s);
            }
        }

        public  double AlgVShiling()
        {
            SortedSet<string> setA = new SortedSet<string>();
            SortedSet<string> setB = new SortedSet<string>();
            SortedSet<int> hashSetA = new SortedSet<int>();
            SortedSet<int> hashSetAv2 = new SortedSet<int>();
            SortedSet<int> hashSetB = new SortedSet<int>();
            GetGrumWShil(setA, compliteCodeFirst);
            GetGrumWShil(setB, compliteCodeSecond);
            TransInHash(setA, hashSetA);
            TransInHash(setB, hashSetB);

            for (int i = 0; i < hashSetA.Count; i++)
            {
                int num = hashSetA.ElementAt(i);
                hashSetAv2.Add(num);
            }

            hashSetA.UnionWith(hashSetB);
            hashSetAv2.IntersectWith(hashSetB);
            int x = hashSetA.Count;
            int y = hashSetAv2.Count;
            return ((double)y / x) * 100;
        }
        public double AlgVarnFish()
        {
            string oneCodeText = string.Join("", compliteCodeFirst.ToArray());
            string twoCodeText = string.Join("", compliteCodeSecond.ToArray());
            int n = oneCodeText.Length;
            int m = twoCodeText.Length;
            int sum = 0;
            int[,] res = new int[m + 1, n + 1];
            for (int i = 0; i < m + 1; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    if (i == 0)
                    {
                        res[i, j] = j;
                    }
                    else if (j == 0)
                    {
                        res[i, j] = i;
                    }
                    else
                    {
                        sum = twoCodeText[i - 1] == oneCodeText[j - 1] ? 0 : 1;
                        res[i, j] = Math.Min((res[i - 1, j] + 1), Math.Min((res[i, j - 1] + 1), res[i - 1, j - 1] + sum));
                    }

                }
            }

            //float s = 100 * (((n + m) - res[m, n]) / n + m);
            return (1 - ((double)res[m, n] / Math.Max(m, n))) * 100;

        }

        private  void GetGrumHeskel(List<string> from, Dictionary<string, double> to)
        {
            double cof = (float)100 / (from.Count - 2);
            for (int i = 0; i < from.Count - 2; i++)
            {
                string threeGram = from[i] + from[i + 1] + from[i + 2];

                if (!to.ContainsKey(threeGram))
                {
                    to.Add(threeGram, cof);
                }
                else
                {
                    to[threeGram] = to[threeGram] + cof;
                }
            }
        }
        public double AlgHeskel()
        {
            Dictionary<string, double> dictHeslCodeFirst = new Dictionary<string, double>();
            Dictionary<string, double> dictHeslCodeSecond = new Dictionary<string, double>();
            GetGrumHeskel(compliteCodeFirst, dictHeslCodeFirst);
            GetGrumHeskel(compliteCodeSecond, dictHeslCodeSecond);
            double result = 0;
            foreach (var key in dictHeslCodeSecond.Keys)
            {
                if (dictHeslCodeFirst.ContainsKey(key))
                {
                    result += dictHeslCodeFirst[key];
                }
            }


            return result;
        }
    }
}
