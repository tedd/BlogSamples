using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using Tedd.DynamicBindingBenchmark.Tests.Classes;
using Tedd.DynamicBindingBenchmark.Tests.Interfaces;

namespace Tedd.DynamicBindingBenchmark.Tests
{
    [Config(typeof(TestConfig1))]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    [RPlotExporter, HtmlExporter, AsciiDocExporter]
    [InliningDiagnoser, MemoryDiagnoser]
    [BaselineColumn, RankColumn, AllStatisticsColumn, ConfidenceIntervalErrorColumn, MaxColumn]
    [Outliers(OutlierMode.All), EvaluateOverhead, GcForce]
    [DisassemblyDiagnoser(true, true, true, false, 1, true)]
    //[ShortRunJob]
    public class CallTests
    {

        [GlobalSetup]
        public void GlobalSetup()
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            // Steady results by not letting other stuff affect it too much
            currentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            // Localizing cache. Assuming HT, taking two last cores (which should share L1)
            currentProcess.ProcessorAffinity = (IntPtr)((1 << (Environment.ProcessorCount - 2)) | (1 << (Environment.ProcessorCount - 1)));
        }

        public Interface1 _interface1_1 = new Interface1Class(0);
        public Interface1 _interface1_Self = new Interface1Class(0);

        public Interface1 _interface10_1 = new Interface10Class(0);
        public Interface10 _interface10_10 = new Interface10Class(0);
        public Interface10Class _interface10_Self = new Interface10Class(0);


        public BaseClass1Class _baseClass1_1 = new InheritedClass1(0);
        public InheritedClass1 _baseClass1_Self = new InheritedClass1(0);

        public BaseClass1Class _baseClass10_1 = new InheritedClass10(0);
        public BaseClass10Class _baseClass10_10 = new InheritedClass10(0);
        public InheritedClass10 _baseClass10_Self = new InheritedClass10(0);

        public BaseClass1ClassVirtual _baseClassVirtualOverride1_1 = new InheritedClassVirtual1Override(0);
        public BaseClass1ClassVirtual _baseClassVirtualNotOverride1_1 = new InheritedClassVirtual1NotOverride(0);
        public BaseClass10ClassVirtual _baseClassVirtualNotOverride10_10 = new InheritedClassVirtual10NotOverride(0);
        public InheritedClassVirtual1Override _baseClassVirtualOverride1_Self = new InheritedClassVirtual1Override(0);
        public InheritedClassVirtual1NotOverride _baseClassVirtualNotOverride10_Self = new InheritedClassVirtual1NotOverride(0);

        public BaseClass1ClassVirtualAbstract _baseClassVirtualAbstractOverride1_1 = new InheritedClassVirtualAbstract1Override(0);
        public InheritedClassVirtualAbstract1Override _baseClassVirtualAbstractOverride1_Self = new InheritedClassVirtualAbstract1Override(0);

        public NormalClass _normalClass = new NormalClass(0);
        public StaticClass _staticClass = new StaticClass(0);
        public List<NormalClass> _normalClassRef = new List<NormalClass>();
        public Func<int> _normalClassLambda;
        public Func<int> _normalClassExpressionTree;
        public delegate int MethodDelegate();
        public MethodDelegate _normalClassDelegate;
        public dynamic _normalClassDynamic;
        //public Func<int> _normalClassReflection;
        public NormalClass _normalClassReflectionClass;
        public MethodInfo _normalClassReflectionMethodInfo;
        //public DynamicMethod _normalClassDynamicMethod;
        //public NormalClass _normalClassDynamicMethodClass;
        //private MethodDelegate _normalClassDynamicMethodDelegated;

        public CallTests()
        {
            {
                var nc = new NormalClass(0);
                _normalClassRef.Add(nc);
                _normalClassLambda = () => nc.Method();
            }
            {
                var nc = new NormalClass(0);
                _normalClassRef.Add(nc);
                Expression<Func<int>> et = () => nc.Method();
                _normalClassExpressionTree = et.Compile();
            }
            {
                var nc = new NormalClass(0);
                _normalClassRef.Add(nc);
                _normalClassDelegate = nc.Method;
            }
            {
                var nc = new NormalClass(0);
                _normalClassRef.Add(nc);
                _normalClassDynamic = nc;
            }
            {
                var nc = new NormalClass(0);
                _normalClassRef.Add(nc);
                _normalClassReflectionClass = nc;
                _normalClassReflectionMethodInfo = nc.GetType().GetMethod("Method");
            }

            //GenerateDynamicMethod();
        }
        //private void GenerateDynamicMethod()
        //{
        //    _normalClassDynamicMethodClass = new NormalClass(0);
        //    _normalClassDynamicMethod = new DynamicMethod("CallMethod",
        //        typeof(int),
        //        new Type[] { },
        //        this.GetType().Module);

        //    var fieldInfo = typeof(CallTests).GetField(nameof(_normalClassDynamicMethodClass), BindingFlags.Instance|BindingFlags.Public);
        //    var methodInfo = typeof(NormalClass).GetMethod("Method");

        //    var il = _normalClassDynamicMethod.GetILGenerator();

        //    il.Emit(OpCodes.Ldarg_0);
        //    il.Emit(OpCodes.Ldfld, fieldInfo);
        //    il.Emit(OpCodes.Callvirt, methodInfo);
        //    //il.Emit(OpCodes.Pop);
        //    il.Emit(OpCodes.Ret);

        //    _normalClassDynamicMethodDelegated = (MethodDelegate)_normalClassDynamicMethod.CreateDelegate(typeof(MethodDelegate));
        //}

        [Benchmark(Description = "*1 int")] public void Call_Interface1_1() => _interface1_1.Method();
        [Benchmark(Description = "Self->1 int")] public void Call_Interface1_Self() => _interface1_Self.Method();
        [Benchmark(Description = "1 int->10 int")] public void Call_Interface10_1() => _interface10_1.Method();
        [Benchmark(Description = "*10 int")] public void Call_Interface10_10() => _interface10_10.Method();
        [Benchmark(Description = "Self->10 int")] public void Call_Interface10_Self() => _interface10_Self.Method();
        [Benchmark(Description = "*1 base")] public void Call_BaseClass1_1() => _baseClass1_1.Method();
        [Benchmark(Description = "Self->1 base")] public void Call_BaseClass1_Self() => _baseClass1_Self.Method();
        [Benchmark(Description = "*1 base->10 base")] public void Call_BaseClass10_1() => _baseClass10_1.Method();
        [Benchmark(Description = "*10 base")] public void Call_BaseClass10_10() => _baseClass10_10.Method();
        [Benchmark(Baseline = true, Description = "*Normal")] public void Call_NormalClass() => _normalClass.Method();
        [Benchmark(Description = "*Static")] public void Call_StaticClass() => StaticClass.Method();
        [Benchmark(Description = "*Lambda")] public void Call_NormalClassLambda() => _normalClassLambda.Invoke();
        [Benchmark(Description = "*Delegate")] public void Call_NormalClassDelegate() => _normalClassDelegate.Invoke();
        [Benchmark(Description = "*Reflection")] public void Call_NormalClassReflection() => _normalClassReflectionMethodInfo.Invoke(_normalClassReflectionClass, null);
        [Benchmark(Description = "*Dynamic")] public void Call_NormalClassDynamic() => _normalClassDynamic.Method();
        [Benchmark(Description = "*Expression Tree")] public void Call_NormalClassExpressionTree() => _normalClassExpressionTree.Invoke();
        //[Benchmark(Description = "*DynamicMethod")] public void Call_NormalDynamicMethod() => _normalClassDynamicMethodDelegated.Invoke();

        [Benchmark(Description = "*1 base->virt override")] public void Call_BaseClass1_1Override() => _baseClassVirtualOverride1_1.Method();
        [Benchmark(Description = "*1 base->virt no override")] public void Call_BaseClass1_1NotOverride() => _baseClassVirtualNotOverride1_1.Method();
        [Benchmark(Description = "*10 base->virt no override")] public void Call_BaseClass10_10NotOverride() => _baseClassVirtualNotOverride10_10.Method();
        [Benchmark(Description = "*Self->virt override")] public void Call_BaseClass1_SelfNotOverride() => _baseClassVirtualOverride1_Self.Method();
        [Benchmark(Description = "Self->10 base virt override")] public void Call_BaseClass10_SelfNotOverride() => _baseClassVirtualNotOverride10_Self.Method();

        [Benchmark(Description = "*1 base->virt abs override")] public void Call_BaseClass1_1OverrideAbstract() => _baseClassVirtualAbstractOverride1_1.Method();
        [Benchmark(Description = "*Self->virt abs override")] public void Call_BaseClass1_SelfNotOverrideAbstract() => _baseClassVirtualAbstractOverride1_Self.Method();

    }
}
