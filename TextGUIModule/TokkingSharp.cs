using System;

namespace TextGUIModule
{
    class TokkingSharp : ATokkining
    {
        private static string patternV = "string|int|bool|float|byte|sbyte|short|ushort|int|uint|long|ulong";
        private static string patternK = "abstract|as|base|List|break|case|catch|checked|class|const|"
                                       + "continue|default|delegate|else|enum|even|explicit|extern|"
                                       + "false|finally|fixed|goto|if|implicit|in|"
                                       + "interface|internal|is|lock|namespace|new|null|object|operator|out|"
                                       + "override|params|private|protected|public|readonly|ref|return|"
                                       + "sealed|shor|sizeof|stackalloc|static|struct|switc|this|throw|true|try|"
                                       + "typeof|unchecked|unsafe|using|static|virtual|void|volatile|Dictionary";
        private static string patternC = "for|do|while";
        private static string patternOSec = "(\\^=|[\\|]=|%=|&=|/=|\\*=|\\-=|\\+=|>>|<<|([|][|])|&&|<=|"
                                  + " >=|!=|[=]{2}|]|[\\-]{2}|[\\+]{2})";
        private static string patternpO = "([\\+]|[-]|[\\*]|[\\/]|[=]|[%]|[>]|[<]|[!]|[~]|[&]|[|]|[/^]|[(]|[)]|[{]|[}])";
        private static string patternOTh = "(>=|<=)";
        private static string patternN = "-?[0-9]+[.]?[0-9]*";
        private static string patternW = "\\w+";

        public TokkingSharp() : base(patternV, patternK, patternC, patternOSec, patternpO, patternOTh, patternN, patternW)
        {
        }
    }
}
