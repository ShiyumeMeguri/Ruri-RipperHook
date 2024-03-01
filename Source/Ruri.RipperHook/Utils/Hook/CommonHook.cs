namespace Ruri.RipperHook.HookUtils;

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