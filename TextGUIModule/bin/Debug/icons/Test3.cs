using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace olimp
{
    class Program
    {
       
        public void Main(string[] args)
        {
            int a = Int32.TryParse("12");
            int c = Int32.TryParse("32");
            int b = mult(a,c);
        }

        public int multyplay(int one,int two)
        {
            return one * two;
        }
    }
}
