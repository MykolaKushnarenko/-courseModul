using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextGUIModule;

namespace CoDEmpare.Tokkin
{
    class TokkinFactory
    {
        private readonly string _lang;
        public TokkinFactory(){}

        public TokkinFactory(string language)
        {
            _lang = language;
        }

        public ATokkining Create()
        {
            switch (_lang)
            {
                case "C#":
                    return new TokkingSharp();
                    
                case "Java":
                    return new TokkingJava();
                    
                case "C++":
                    return new TokkingСPP();
                    
                default:
                    return null;
            }
        }
    }
}
