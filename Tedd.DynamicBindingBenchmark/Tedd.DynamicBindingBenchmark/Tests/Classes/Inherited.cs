using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tedd.DynamicBindingBenchmark.Tests.Classes
{
    public class BaseClass2Class : BaseClass1Class { public BaseClass2Class(int i) : base(i) { } }
    public class BaseClass3Class : BaseClass2Class { public BaseClass3Class(int i) : base(i) { } }
    public class BaseClass4Class : BaseClass3Class { public BaseClass4Class(int i) : base(i) { } }
    public class BaseClass5Class : BaseClass4Class { public BaseClass5Class(int i) : base(i) { } }
    public class BaseClass6Class : BaseClass5Class { public BaseClass6Class(int i) : base(i) { } }
    public class BaseClass7Class : BaseClass6Class { public BaseClass7Class(int i) : base(i) { } }
    public class BaseClass8Class : BaseClass7Class { public BaseClass8Class(int i) : base(i) { } }
    public class BaseClass9Class : BaseClass8Class { public BaseClass9Class(int i) : base(i) { } }
    public class BaseClass10Class : BaseClass9Class { public BaseClass10Class(int i) : base(i) { } }
    public class BaseClass1Class
    {
        private readonly int _i;
        public BaseClass1Class(int i)
        {
            _i = i;
        }

        public int Method()
        {
            return _i;
        }
    }

    public class InheritedClass1 : BaseClass1Class
    {
        public InheritedClass1(int i) : base(i) { }
    }
    public class InheritedClass10 : BaseClass10Class
    {
        public InheritedClass10(int i) : base(i) { }
    }  
}
