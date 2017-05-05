namespace Shinobytes.XzaarScript
{
    public interface IXzaarFieldInfo
    {
        XzaarType FieldType { get; }
        object GetValue(object obj);
        void SetValue(object obj, object value);
    }
}