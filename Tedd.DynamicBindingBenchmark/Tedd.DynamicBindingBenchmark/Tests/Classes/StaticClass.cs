// Login: tedd
// Username: Tedd Hansen
// Created: -- 
// Modified date: 2018-12-26 16:12
namespace Tedd.DynamicBindingBenchmark.Tests.Classes
{
    public class StaticClass
    {
        private static int _i;
        public StaticClass(int i)
        {
            _i = i;
        }

        public static int Method()
        {
            return _i;
        }
    }
}