using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tedd.DynamicBindingBenchmark.Tests.Classes
{
    public abstract class BaseClass1ClassVirtualAbstract
    {
        public readonly int _i;
        public BaseClass1ClassVirtualAbstract(int i)
        {
            _i = i;
        }

        public abstract int Method();
    }

    public class InheritedClassVirtualAbstract1Override : BaseClass1ClassVirtualAbstract
    {
        public InheritedClassVirtualAbstract1Override(int i) : base(i) { }
        public override int Method()
        {
            return _i;
        }
    }
    

}
