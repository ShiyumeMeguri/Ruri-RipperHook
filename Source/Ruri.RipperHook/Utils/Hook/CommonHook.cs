namespace Ruri.RipperHook.HookUtils;
/// <summary>
/// 这个类的目的是把一些非常常用的Hook简单化 避免重复代码 同时也可以避免更新导致的重复修改
/// </summary>
public class CommonHook
{
    protected void SetPrivateField(Type type, string name, object newValue)
    {
        type.GetField(name, ReflectionExtensions.PrivateInstanceBindFlag()).SetValue(this, newValue);
    }
    protected object GetPrivateField(Type type, string name)
    {
        return type.GetField(name, ReflectionExtensions.PrivateInstanceBindFlag()).GetValue(this);
    }

}