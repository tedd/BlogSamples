using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tedd.DynamicBindingBenchmark.Tests.Interfaces;

namespace Tedd.DynamicBindingBenchmark.Tests.Classes
{
    public class Interface10Class:Interface10
    {
        private readonly int _i;
        public Interface10Class(int i)
        {
            _i = i;
        }

        public int Method()
        {
            return _i;
        }
    }
    public class Interface1Class:Interface1
    {
        private readonly int _i;
        public Interface1Class(int i)
        {
            _i = i;
        }

        public int Method()
        {
            return _i;
        }
    }
}
