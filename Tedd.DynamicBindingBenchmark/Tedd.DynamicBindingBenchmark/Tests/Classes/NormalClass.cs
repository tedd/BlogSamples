using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tedd.DynamicBindingBenchmark.Tests.Classes
{
    public class NormalClass
    {
        private readonly int _i;
        public NormalClass(int i)
        {
            _i = i;
        }

        public int Method()
        {
            return _i;
        }
    }
}
