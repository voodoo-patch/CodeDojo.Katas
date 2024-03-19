using System.Reflection;
using Castle.DynamicProxy;
using Moq;

namespace TestProject1;

public class MyMock<T> : Mock<T>, IDisposable where T : class
{
    void PopulateFactoryReferences()
    {
        // Moq tries ridiculously hard to protect their internal structures - pretty much every class that could be of interest to us is marked internal
        // All below code is basically serving one simple purpose = to swap a `ProxyGenerator` field on the `ProxyFactory.Instance` singleton
        // all types are internal so reflection it is
        // I will invite you to make this a bit cleaner by obtaining the `_generatorFieldInfo` value once and caching it for later
        var moqAssembly = Assembly.Load(nameof(Moq));
        var proxyFactoryType = moqAssembly.GetType("Moq.ProxyFactory");
        var castleProxyFactoryType = moqAssembly.GetType("Moq.CastleProxyFactory");     
        var proxyFactoryInstanceProperty = proxyFactoryType.GetProperty("Instance");
        _generatorFieldInfo = castleProxyFactoryType.GetField("generator", BindingFlags.NonPublic | BindingFlags.Instance);     
        _castleProxyFactoryInstance = proxyFactoryInstanceProperty.GetValue(null);
        _originalProxyFactory = _generatorFieldInfo.GetValue(_castleProxyFactoryInstance);//save default value to restore it later
    }

    public MyMock(T targetInstance) {       
        PopulateFactoryReferences();
        // this is where we do the trick!
        _generatorFieldInfo.SetValue(_castleProxyFactoryInstance, new MyProxyGenerator(targetInstance));
    }

    private FieldInfo _generatorFieldInfo;
    private object _castleProxyFactoryInstance;
    private object _originalProxyFactory;

    public void Dispose()
    {
        // you will notice I opted to implement IDisposable here. 
        // My goal is to ensure I restore the original value on Moq's internal static class property in case you will want to mix up this class with stock standard implementation
        // there are probably other ways to ensure reference is restored reliably, but I'll leave that as another challenge for you to tackle
        _generatorFieldInfo.SetValue(_castleProxyFactoryInstance, _originalProxyFactory);
    }
}
class MyProxyGenerator : ProxyGenerator
{
    object _target;

    public MyProxyGenerator(object target) {
        _target = target; // this is the missing piece, we'll have to pass it on to Castle proxy
    }
    // this method is 90% taken from the library source. I only had to tweak two lines (see below)
    public override object CreateClassProxy(Type classToProxy, Type[] additionalInterfacesToProxy, ProxyGenerationOptions options, object[] constructorArguments, params IInterceptor[] interceptors)
    {
        if (classToProxy == null)
        {
            throw new ArgumentNullException("classToProxy");
        }
        if (options == null)
        {
            throw new ArgumentNullException("options");
        }
        if (!classToProxy.GetTypeInfo().IsClass)
        {
            throw new ArgumentException("'classToProxy' must be a class", "classToProxy");
        }
        CheckNotGenericTypeDefinition(classToProxy, "classToProxy");
        CheckNotGenericTypeDefinitions(additionalInterfacesToProxy, "additionalInterfacesToProxy");
        Type proxyType = CreateClassProxyTypeWithTarget(classToProxy, additionalInterfacesToProxy, options); // these really are the two lines that matter
        List<object> list =  BuildArgumentListForClassProxyWithTarget(_target, options, interceptors);       // these really are the two lines that matter
        if (constructorArguments != null && constructorArguments.Length != 0)
        {
            list.AddRange(constructorArguments);
        }
        return CreateClassProxyInstance(proxyType, list, classToProxy, constructorArguments);
    }
}