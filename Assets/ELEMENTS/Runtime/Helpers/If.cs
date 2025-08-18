using System.Collections;
using System.Collections.Generic;
using HolospaceAPI.Client;
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
            return property.Select(v => v == null ? equalTo == null : v.Equals(equalTo));
        }

        public static Observable<bool> NotEqual<T>(Observable<T> property, T equalTo)
        {
            return property.Select(v => !(v == null ? equalTo == null : v.Equals(equalTo)));
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

        public static Observable<bool> True(Observable<bool> property)
        {
            return property.Select(p => p);
        }

        public static Observable<bool> False(Observable<bool> property)
        {
            return property.Select(p => !p);
        }

        public static Observable<bool> Not(Observable<bool> property)
        {
            return False(property);
        }

        public static Observable<bool> Empty<T>(Observable<T[]> property)
        {
            return property.Select(v => v == null || v.Length == 0);
        }

        public static Observable<bool> NotEmpty<T>(Observable<T[]> property)
        {
            return property.Select(v => v is { Length: > 0 });
        }

        public static Observable<bool> Empty<T>(Observable<ICollection<T>> property)
        {
            return property.Select(v => v == null || v.Count == 0);
        }

        public static Observable<bool> NotEmpty<T>(Observable<ICollection<T>> property)
        {
            return property.Select(v => v is { Count: > 0 });
        }

        public static Observable<bool> Empty<TK, TV>(Observable<Dictionary<TK, TV>> property)
        {
            return property.Select(v => v == null || v.Count == 0);
        }

        public static Observable<bool> NotEmpty<TK, TV>(Observable<Dictionary<TK, TV>> property)
        {
            return property.Select(v => v is { Count: > 0 });
        }
    }
}