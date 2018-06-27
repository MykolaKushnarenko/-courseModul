using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDEmpare.Algorithm
{
    class WShiling : IAlgorithm
    {
        private List<string> _codeMain;
        private List<string> _codeChild;
        public double Result { get; set; }

        public WShiling(List<string> main, List<string> child)
        {
            _codeChild = child;
            _codeMain = main;
        }
        public double CompareRes()
        {
            SortedSet<string> setA = new SortedSet<string>();
            SortedSet<string> setB = new SortedSet<string>();
            SortedSet<int> hashSetA = new SortedSet<int>();
            SortedSet<int> hashSetAv2 = new SortedSet<int>();
            SortedSet<int> hashSetB = new SortedSet<int>();
            GetGrumWShil(setA, _codeMain);
            GetGrumWShil(setB, _codeChild);
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
            Result =((double)y / x) * 100;
            return Result;
        }

        private void GetGrumWShil(ICollection<string> set, List<string> dirList)
        {
            for (int i = 0; i < dirList.Count - 3; i++)
            {
                List<string> ls = new List<string>();
                for (int j = 0; j < 3; ++j) ls.Add(dirList[i + j]);
                string s = string.Join(" ", ls.ToArray());
                set.Add(s);
            }
        }
        private void TransInHash(SortedSet<string> set, SortedSet<int> hashSet)
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
    }
}
