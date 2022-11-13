using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQLHelper.Enums_Structs
{
    public struct Predicate
    {
        public string Column;
        public string Value;
        public NodeType NodeType;
    }
}