namespace JSLib
{
    public class JSDynamicArray : JSArray<object>
    {
        public JSDynamicArray(int arrayLength = 0) : base(arrayLength)
        { }

        public JSDynamicArray(params object[] items) : base(items)
        { }
    }
}
