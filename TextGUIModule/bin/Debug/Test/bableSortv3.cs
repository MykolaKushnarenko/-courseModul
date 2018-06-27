using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetsForCourse
{
    class Program
    {
        static void sort(double[] mas, int sz)
        {
            for (int i = sz - 1; i >= 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    {
                        int z = 0;

                        z = mas[j];

                        mas[j] = mas[j + 1];

                        mas[j + 1] = z;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            int[] a = { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 30 };
            int s = 12;
            sort(a , s);
            for (int i = 0; i < s; ++i)
            {
                Console.WriteLine(a[i]);
            }
        }
    }
}
