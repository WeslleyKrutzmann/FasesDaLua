namespace System.Windows
{
    public static class DependencyObjectExtension
    {

        public static T GetValue<T>(this DependencyObject dependencyObject, DependencyProperty property)
        {
            var type = typeof(T);
            var value = dependencyObject.GetValue(property);

            if (type.IsEnum)
            {
                return (T)(object)Convert.ToInt32(value);
            }

            return (T)Convert.ChangeType(value, type);
        }
    }
}
