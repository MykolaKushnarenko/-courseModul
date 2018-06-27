using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextGUIModule
{
    public class NormalTestTwo
    {
        private string filePath = (string)null;
        private List<string> divStr;
        private List<string> compliteText;
        private StringBuilder compileSt;

        public NormalTestTwo(string path)
        {
            if (!File.Exists(path))
                return;
            this.filePath = path;
            this.divStr = new List<string>();
            this.compliteText = new List<string>();
            this.compileSt = new StringBuilder();
        }

        public List<string> normalize()
        {
            int index = 0;
            using (StreamReader streamReader = new StreamReader(this.filePath, Encoding.Default))
            {
                string str1;
                while ((str1 = streamReader.ReadLine()) != null)
                {
                    string str2 = str1.Replace("[", " ( ").Replace("]", " ) ").Replace("<", " < ").Replace(">", " > ").Replace("(", " ( ").Replace(")", " ) ").Replace(",", "").Replace(".", "").Replace("++", " ++").Replace("--", " --").Replace(";", "").Replace("'", "").Replace("\t", "").Replace("\"", "");
                    this.divStr = new List<string>((IEnumerable<string>)str2.Split(' '));
                    bool flag = true;
                    while (flag && str2 != "" && str2.Replace(" ", "") != "")
                    {
                        if (this.divStr[index] == "using" || str2.Contains("//") || (this.divStr[index] == "#include" || this.divStr[index] == "import") || this.divStr[index] == "package")
                            flag = false;
                        else if (this.ClearSpace(this.divStr))
                        {
                            this.SplitList(this.divStr, this.compliteText);
                            break;
                        }
                    }
                }
                return this.compliteText;
            }
        }

        private bool ClearSpace(List<string> listForClear)
        {
            bool flag = false;
            for (int index = 0; index < listForClear.Count; ++index)
            {
                if (listForClear[index] == "" || listForClear[index] == " " || listForClear[index] == "{" || listForClear[index] == "}")
                {
                    listForClear.RemoveAt(index);
                    --index;
                    flag = ((flag ? 1 : 0) | 1) != 0;
                }
                else if (listForClear[index].Contains<char>('{') || listForClear[index].Contains<char>('}'))
                {
                    for (int startIndex = 0; startIndex < listForClear[index].Length; ++startIndex)
                    {
                        if (listForClear[index][startIndex] == '{' || listForClear[index][startIndex] == '}')
                        {
                            listForClear[index].Remove(startIndex, startIndex + 1);
                            flag = ((flag ? 1 : 0) | 1) != 0;
                            --index;
                        }
                    }
                }
                else if (!listForClear[index].Contains<char>('{') || !listForClear[index].Contains<char>('}'))
                    flag = ((flag ? 1 : 0) | 1) != 0;
            }
            return flag;
        }

        private void SplitList(List<string> listFrom, List<string> listTo)
        {
            listFrom.ForEach((Action<string>)(s =>
            {
                listTo.Add(s);
                this.compileSt.Append(s);
            }));
        }
    }
}
