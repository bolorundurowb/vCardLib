namespace vCardLib.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Slice<T>(this T[] source, int start)
        {
            var len = source.Length - start;
            var res = new T[len];
            for (var i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }
    }
}
