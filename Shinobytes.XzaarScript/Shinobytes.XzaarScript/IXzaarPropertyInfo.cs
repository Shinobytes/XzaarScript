namespace Shinobytes.XzaarScript
{
    public interface IXzaarPropertyInfo
    {
        XzaarType PropertyType { get; }
        object GetValue(object obj, object[] index);
        void SetValue(object obj, object value, object[] index);

        XzaarMethodInfo GetGetMethod();
        XzaarMethodInfo GetSetMethod();
    }
}