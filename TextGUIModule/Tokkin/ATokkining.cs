using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace TextGUIModule
{
    abstract class ATokkining
    {
        private readonly string _patternV = null;
        private readonly string _patternK = null;
        private readonly string _patternC = null;
        private readonly string _patternOSec = null;
        private readonly string _patternO = null;
        private readonly string _patternOTh = null;
        private readonly string _patternN = null;
        private readonly string _patternW = null;

        public ATokkining(string patternV, string patternK,string patternC, string patternOSec, string patternO, string patternOTh, string patternN, string patternW)
        {
            this._patternV = patternV;
            this._patternK = patternK;
            this._patternC = patternC;
            this._patternOSec = patternOSec;
            this._patternO = patternO;
            this._patternOTh = patternOTh;
            this._patternN = patternN;
            this._patternW = patternW;
        }
        private bool IsLowCase(int index, List<string> divStr)
        {
            bool isLower = false;
            Regex test = new Regex(@"[\d(){}<>=+-/*]");
            Match isOn = test.Match(divStr[index]);
            if (isOn.Success)
            {
                isLower |= true;
            }
            else
            {
                for (int k = 0; k < divStr[index].Length; k++)
                {
                    if (char.IsLower(divStr[index][k]))
                    {
                        isLower |= true;
                    }
                }
            }

            return isLower;
        }
        public void Tokening(List<string> divStr)
        {
            bool isLower = false;
            string res = null;
            for (int i = 0; i < divStr.Count; i++)
            {
                isLower = IsLowCase(i, divStr);
                for (int j = 0; j < 8; j++)
                {
                    if (isLower == true)
                    {
                        string target;
                        Regex regex;
                        switch (j)
                        {

                            case 0:
                                target = "V";
                                regex = new Regex(_patternV);
                                res = regex.Replace(divStr[i], target);
                                divStr[i] = res;
                                break;
                            case 1:
                                target = "K";
                                regex = new Regex(_patternK);
                                res = regex.Replace(divStr[i], target);
                                divStr[i] = res;
                                break;
                            case 2:
                                target = "C";
                                regex = new Regex(_patternC);
                                res = regex.Replace(divStr[i], target);
                                divStr[i] = res;
                                break;
                            case 3:
                                target = "O";
                                regex = new Regex(_patternOSec);
                                res = regex.Replace(divStr[i], target);
                                divStr[i] = res;
                                break;
                            case 4:
                                target = "O";
                                regex = new Regex(_patternO);
                                res = regex.Replace(divStr[i], target);
                                divStr[i] = res;
                                break;
                            case 5:
                                target = "O";
                                regex = new Regex(_patternOTh);
                                res = regex.Replace(divStr[i], target);
                                divStr[i] = res;
                                break;
                            case 6:
                                target = "N";
                                regex = new Regex(_patternN);
                                res = regex.Replace(divStr[i], target);
                                divStr[i] = res;
                                break;
                            case 7:
                                if (isLower)
                                {
                                    target = "W";
                                    regex = new Regex(_patternW);
                                    res = regex.Replace(divStr[i], target);
                                    divStr[i] = res;
                                }
                                break;
                        }
                        isLower = IsLowCase(i, divStr);
                    }
                }
            }
        }
    }
}
