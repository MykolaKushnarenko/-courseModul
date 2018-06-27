using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDEmpare.Algorithm
{
    class Heskel: IAlgorithm
    {
        private List<string> _codeMain;
        private List<string> _codeChild;
        public double Result { get; set; }

        public Heskel(List<string> main, List<string> child)
        {
            _codeMain = main;
            _codeChild = child;
        }
        public double CompareRes()
        {
            Dictionary<string, double> dictHeslCodeFirst = new Dictionary<string, double>();
            Dictionary<string, double> dictHeslCodeSecond = new Dictionary<string, double>();
            GetGrumHeskel(_codeMain, dictHeslCodeFirst);
            GetGrumHeskel(_codeChild, dictHeslCodeSecond);
            Result = 0;
            foreach (var key in dictHeslCodeSecond.Keys)
            {
                if (dictHeslCodeFirst.ContainsKey(key))
                {
                    Result += dictHeslCodeFirst[key];
                }
            }
            return Result;
        }
        private void GetGrumHeskel(List<string> from, Dictionary<string, double> to)
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
    }
}
