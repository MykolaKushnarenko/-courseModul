using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDEmpare.Algorithm
{
    class LevenshteinDistance: IAlgorithm
    {
        
        public float _result { get; set; }
        private List<string> _codeMain;
        private List<string> _codeChild;
        public LevenshteinDistance(List<string> main, List<string> child)
        {
            _codeMain = main;
            _codeChild = child;
        }
        public float CompareRes()
        {
            string oneCodeText = string.Join("", _compliteCodeMain.ToArray());
            string twoCodeText = string.Join("", _compliteCodeChild.ToArray());
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
    }
}
