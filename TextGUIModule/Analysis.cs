using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using CoDEmpare.Algorithm;
using CoDEmpare.Tokkin;


namespace TextGUIModule
{
    class Analysis
    {
        private Normalization _normalize;
        private List<string> _compliteCodeMain = new List<string>();
        private List<string> _compliteCodeChild = new List<string>();
        private TokkinFactory _factory = null;
        private ATokkining _aTokkining = null;


        public void RunAnalysis(string language, string path)
        {
            _normalize = new Normalization(path);
            _compliteCodeMain = _normalize.Normal();
            _factory = new TokkinFactory(language);
            _aTokkining = _factory.Create();
            _aTokkining?.Tokening(_compliteCodeMain);
        }

        public void SetCodeMainFromDB(string code)
        {
            _compliteCodeMain.Clear();
            for (int i = 0; i < code.Length; i++)
            {
                _compliteCodeMain.Add(code[i].ToString());
            }
        }
        public void SetCodeChildFromDB(string code)
        {
            _compliteCodeChild.Clear();
            for (int i = 0; i < code.Length; i++)
            {
                _compliteCodeChild.Add(code[i].ToString());
            }
        }
        public List<string> InserToDB()
        {
            List<string> grams = new List<string>();
            for (int i = 0; i < _compliteCodeMain.Count - 2; i++)
            {
                string threeGram = _compliteCodeMain[i] + _compliteCodeMain[i + 1] + _compliteCodeMain[i + 2];
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
            string normalJoin = string.Join("", _compliteCodeMain.ToArray());
            return normalJoin;
        }

        public double ResultAlgorithm(int numberOfAlg)
        {
            AlgorithmFactory fuctory = new AlgorithmFactory(numberOfAlg, _compliteCodeMain, _compliteCodeChild);
            IAlgorithm algorithm = fuctory.Create();
            algorithm?.CompareRes();
            return algorithm.Result;
        }
    }
}
