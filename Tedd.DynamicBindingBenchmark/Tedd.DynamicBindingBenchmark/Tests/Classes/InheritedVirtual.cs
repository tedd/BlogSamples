using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tedd.DynamicBindingBenchmark.Tests.Classes
{
    public class BaseClass2ClassVirtual : BaseClass1ClassVirtual { public BaseClass2ClassVirtual(int i) : base(i) { } }
    public class BaseClass3ClassVirtual : BaseClass2ClassVirtual { public BaseClass3ClassVirtual(int i) : base(i) { } }
    public class BaseClass4ClassVirtual : BaseClass3ClassVirtual { public BaseClass4ClassVirtual(int i) : base(i) { } }
    public class BaseClass5ClassVirtual : BaseClass4ClassVirtual { public BaseClass5ClassVirtual(int i) : base(i) { } }
    public class BaseClass6ClassVirtual : BaseClass5ClassVirtual { public BaseClass6ClassVirtual(int i) : base(i) { } }
    public class BaseClass7ClassVirtual : BaseClass6ClassVirtual { public BaseClass7ClassVirtual(int i) : base(i) { } }
    public class BaseClass8ClassVirtual : BaseClass7ClassVirtual { public BaseClass8ClassVirtual(int i) : base(i) { } }
    public class BaseClass9ClassVirtual : BaseClass8ClassVirtual { public BaseClass9ClassVirtual(int i) : base(i) { } }
    public class BaseClass10ClassVirtual : BaseClass9ClassVirtual { public BaseClass10ClassVirtual(int i) : base(i) { } }
    public class BaseClass1ClassVirtual
    {
        public readonly int _i;
        public BaseClass1ClassVirtual(int i)
        {
            _i = i;
        }

        public virtual int Method()
        {
            return _i;
        }
    }

    public class InheritedClassVirtual1Override : BaseClass1ClassVirtual
    {
        public InheritedClassVirtual1Override(int i) : base(i) { }
        public override int Method()
        {
            return _i;
        }
    }
    
    public class InheritedClassVirtual1NotOverride : BaseClass1ClassVirtual
    {
        public InheritedClassVirtual1NotOverride(int i) : base(i) { }
    }
    public class InheritedClassVirtual10NotOverride : BaseClass10ClassVirtual
    {
        public InheritedClassVirtual10NotOverride(int i) : base(i) { }

    }
}
