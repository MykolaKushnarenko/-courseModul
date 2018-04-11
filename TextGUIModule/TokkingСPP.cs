using System;

namespace TextGUIModule
{
    class TokkingСPP: ATokkining
    {
        private static string patternV = "String|int|bool|float|signed char|unsigned char|signed short int"
                                       + "unsigned short int|signed long int|unsigned long int|float|double|long double";

        private static string patternK = "alignas|alignof|and|and_eq|asm|auto|bitand|bitor|break|const|"
                                       + "case|catch|class|compl|const|constexpr|const_cast|continue|"
                                       + "false|default|delete|do|dynamic_cast|implicit|else|"
                                       + "enum|explicit|export|extern|namespace|false|friend|goto|if|inline|"
                                       + "mutable|new|private|noexcept|public|not|not_eq|operator|"
                                       + "or|or_eq|protected|register|static|reinterpret_cast|signed|sizeof|static_assert"
                                       + "switch|template|this|thread_local|true|try|typedef|typeid|typename"
                                       + "union|unsigned|using|virtual|void|xor|xor_eq|static_cast|struct";
        private static string patternC = "for|do|while";
        private static string patternOSec = "(\\^=|[\\|]=|%=|&=|/=|\\*=|\\-=|\\+=|>>|<<|([|][|])|&&|<=|"
                                          + " >=|!=|[=]{2}|]|[\\-]{2}|[\\+]{2})";
        private static string patternpO = "([\\+]|[-]|[\\*]|[\\/]|[=]|[%]|[>]|[<]|[!]|[~]|[&]|[|]|[/^]|[(]|[)]|[{]|[}])";
        private static string patternOTh = "(>=|<=)";
        private static string patternN = "-?[0-9]+[.]?[0-9]*";
        private static string patternW = "\\w+";

        public TokkingСPP() : base(patternV,patternK,patternC,patternOSec, patternpO, patternOTh, patternN, patternW)
        {
        }
    }
}
