using R3;

namespace ELEMENTS.Scripts.Helpers
{
    public static class If
    {
        public static Observable<T> Ternary<T>(Observable<bool> condition, T trueValue, T falseValue)
        {
            return condition.Select(v => v ? trueValue : falseValue);
        }

        public static Observable<bool> Equal<T>(Observable<T> property, T equalTo)
        {
            return property.Select(v => v.Equals(equalTo));
        }

        public static Observable<bool> NotEqual<T>(Observable<T> property, T equalTo)
        {
            return property.Select(v => !v.Equals(equalTo));
        }

        public static Observable<bool> NullOrEmpty(Observable<string> property)
        {
            return property.Select(string.IsNullOrEmpty);
        }

        public static Observable<bool> NotNullOrEmpty(Observable<string> property)
        {
            return property.Select(v => !string.IsNullOrEmpty(v));
        }

        public static Observable<bool> Null<T>(Observable<T> property)
        {
            return property.Select(v => v == null);
        }

        public static Observable<bool> NotNull<T>(Observable<T> property)
        {
            return property.Select(v => v != null);
        }
    }
}