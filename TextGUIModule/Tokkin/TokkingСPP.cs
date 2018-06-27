using System;

namespace TextGUIModule
{
    class TokkingСPP: ATokkining
    {
        private const string _patternV = "String|int|bool|float|signed char|unsigned char|signed short int"
                                       + "unsigned short int|signed long int|unsigned long int|float|double|long double";

        private const string _patternK = "alignas|alignof|and|and_eq|asm|auto|bitand|bitor|break|const|"
                                       + "case|catch|class|compl|const|constexpr|const_cast|continue|"
                                       + "false|default|delete|do|dynamic_cast|implicit|else|"
                                       + "enum|explicit|export|extern|namespace|false|friend|goto|if|inline|"
                                       + "mutable|new|private|noexcept|public|not|not_eq|operator|"
                                       + "or|or_eq|protected|register|static|reinterpret_cast|signed|sizeof|static_assert"
                                       + "switch|template|this|thread_local|true|try|typedef|typeid|typename"
                                       + "union|unsigned|using|virtual|void|xor|xor_eq|static_cast|struct";
        private const string _patternC = "for|do|while";
        private const string _patternOSec = "(\\^=|[\\|]=|%=|&=|/=|\\*=|\\-=|\\+=|>>|<<|([|][|])|&&|<=|"
                                          + " >=|!=|[=]{2}|]|[\\-]{2}|[\\+]{2})";
        private const string _patternpO = "([\\+]|[-]|[\\*]|[\\/]|[=]|[%]|[>]|[<]|[!]|[~]|[&]|[|]|[/^]|[(]|[)]|[{]|[}])";
        private const string _patternOTh = "(>=|<=)";
        private const string _patternN = "-?[0-9]+[.]?[0-9]*";
        private const string _patternW = "\\w+";

        public TokkingСPP() : base(_patternV,_patternK,_patternC,_patternOSec, _patternpO, _patternOTh, _patternN, _patternW)
        {
        }
    }
}
