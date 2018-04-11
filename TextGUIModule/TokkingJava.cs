using System;

namespace TextGUIModule
{
    class TokkingJava: ATokkining
    {
        private static string patternV = "bool|char|float|short|int|long|double|string";

        private static string patternK = "asm|else|new|this|auto|enum|operator|throw|explicit|private|"
                                         + " true|break|export|protected|try|case|extern|public|typedef|catch|false|"
                                         + "register|typeid|reinterpret_cast|typename|class|return|union|const|friend|"
                                         + "unsigned|const_cast|goto|signed|using|continue|if|sizeof|virtual|default|"
                                         + "inline|static|void|delete|static_cast|volatile|struct|wchar_t|mutable|switch|"
                                         + "dynamic_cast|namespace|template|HashMap|ArrayList";
        private static string patternC = "for|do|while";

        private static string patternOSec = "(\\^=|[\\|]=|%=|&=|/=|\\*=|\\-=|\\+=|>>|<<|([|][|])|&&|<=|"
                                           + " >=|!=|[=]{2}|]|[\\-]{2}|[\\+]{2})";
        private static string patternpO = "([\\+]|[-]|[\\*]|[\\/]|[=]|[%]|[>]|[<]|[!]|[~]|[&]|[|]|[/^]|[(]|[)]|[{]|[}])";
        private static string patternOTh = "(>=|<=)";
        private static string patternN = "-?[0-9]+[.]?[0-9]*";
        private static string patternW = "\\w+";

        public TokkingJava() : base(patternV, patternK, patternC, patternOSec, patternpO, patternOTh, patternN, patternW)
        {
        }
    }
}
