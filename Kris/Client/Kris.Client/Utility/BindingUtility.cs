namespace Kris.Client.Utility;

public static class BindingUtility
{
    // Source: https://medium.com/nerd-for-tech/converter-parameter-binding-how-to-bind-complex-values-at-xamarin-maui-a23b6c45ab31
    public static object GetPropertyValue(object src, string propertyName)
    {
        if (propertyName.Contains('.'))
        {
            var splitIndex = propertyName.IndexOf('.');
            var parent = propertyName.Substring(0, splitIndex);
            var child = propertyName.Substring(splitIndex + 1);
            var obj = src?.GetType().GetProperty(parent)?.GetValue(src, null);
            return GetPropertyValue(obj, child);
        }

        return src?.GetType().GetProperty(propertyName)?.GetValue(src, null);
    }
}
