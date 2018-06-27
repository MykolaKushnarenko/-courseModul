using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDEmpare.Algorithm
{
    class AlgorithmFactory
    {
        private readonly int _numberOfAlg = 0;
        private readonly List<string> _main;
        private readonly List<string> _child;
        public AlgorithmFactory(int numberOf, List<string> main, List<string> child)
        {
            _numberOfAlg = numberOf;
            this._main = main;
            this._child = child;
        }

        public IAlgorithm Create()
        {
            switch (_numberOfAlg)
            {
                case 0:
                    return new Heskel(this._main, this._child);
                case 1:
                    return new LevenshteinDistance(this._main, this._child);
                case 2:
                    return new WShiling(this._main, this._child);
                default:
                    return null;
            }
        }
    }
}
