using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tedd.DynamicBindingBenchmark.Tests.Interfaces
{
    public interface Interface2:Interface1 {  }
    public interface Interface3:Interface2 {  }
    public interface Interface4:Interface3 {  }
    public interface Interface5:Interface4 {  }
    public interface Interface6:Interface5 {  }
    public interface Interface7:Interface6 {  }
    public interface Interface8:Interface7 {  }
    public interface Interface9:Interface8 {  }
    public interface Interface10:Interface9 { }
    public interface Interface1 { int Method(); }
}
