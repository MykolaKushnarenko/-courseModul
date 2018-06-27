using System;

namespace TextGUIModule
{
    class TokkingJava: ATokkining
    {
        private const string _patternV = "bool|char|float|short|int|long|double|string";

        private const string _patternK = "asm|else|new|this|auto|enum|operator|throw|explicit|private|"
                                         + " true|break|export|protected|try|case|extern|public|typedef|catch|false|"
                                         + "register|typeid|reinterpret_cast|typename|class|return|union|const|friend|"
                                         + "unsigned|const_cast|goto|signed|using|continue|if|sizeof|virtual|default|"
                                         + "inline|static|void|delete|static_cast|volatile|struct|wchar_t|mutable|switch|"
                                         + "dynamic_cast|namespace|template|HashMap|ArrayList";
        private const string _patternC = "for|do|while";

        private const string _patternOSec = "(\\^=|[\\|]=|%=|&=|/=|\\*=|\\-=|\\+=|>>|<<|([|][|])|&&|<=|"
                                           + " >=|!=|[=]{2}|]|[\\-]{2}|[\\+]{2})";
        private const string _patternpO = "([\\+]|[-]|[\\*]|[\\/]|[=]|[%]|[>]|[<]|[!]|[~]|[&]|[|]|[/^]|[(]|[)]|[{]|[}])";
        private const string _patternOTh = "(>=|<=)";
        private const string _patternN = "-?[0-9]+[.]?[0-9]*";
        private const string _patternW = "\\w+";

        public TokkingJava() : base(_patternV, _patternK, _patternC, _patternOSec, _patternpO, _patternOTh, _patternN, _patternW)
        {
        }
    }
}
