using System;

namespace TextGUIModule
{
    class TokkingSharp : ATokkining
    {
        private const string _patternV = "string|int|bool|float|byte|sbyte|short|ushort|int|uint|long|ulong";
        private const string _patternK = "abstract|as|base|List|break|case|catch|checked|class|const|"
                                       + "continue|default|delegate|else|enum|even|explicit|extern|"
                                       + "false|finally|fixed|goto|if|implicit|in|"
                                       + "interface|internal|is|lock|namespace|new|null|object|operator|out|"
                                       + "override|params|private|protected|public|readonly|ref|return|"
                                       + "sealed|shor|sizeof|stackalloc|static|struct|switc|this|throw|true|try|"
                                       + "typeof|unchecked|unsafe|using|static|virtual|void|volatile|Dictionary";
        private const string _patternC = "for|do|while";
        private const string _patternOSec = "(\\^=|[\\|]=|%=|&=|/=|\\*=|\\-=|\\+=|>>|<<|([|][|])|&&|<=|"
                                  + " >=|!=|[=]{2}|]|[\\-]{2}|[\\+]{2})";
        private const string _patternpO = "([\\+]|[-]|[\\*]|[\\/]|[=]|[%]|[>]|[<]|[!]|[~]|[&]|[|]|[/^]|[(]|[)]|[{]|[}])";
        private const string _patternOTh = "(>=|<=)";
        private const string _patternN = "-?[0-9]+[.]?[0-9]*";
        private const string _patternW = "\\w+";

        public TokkingSharp() : base(_patternV, _patternK, _patternC, _patternOSec, _patternpO, _patternOTh, _patternN, _patternW)
        {
        }
    }
}
