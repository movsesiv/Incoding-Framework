namespace Incoding.Maybe
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class MaybeEnumerable
    {
        #region Factory constructors

        public static void DoEach<TInput>(this IEnumerable<TInput> source, Action<TInput> action)
        {
            source.DoEach((input, i) => action(input));
        }

        public static void DoEach<TInput>(this IEnumerable<TInput> source, Action<TInput, int> action)
        {
            if (source == null)
                return;

            int index = 0;
            foreach (var input in source)
            {
                action(input, index);
                index++;
            }
        }

        public static IEnumerable<TResult> WithEach<TInput, TResult>(this IEnumerable<TInput> source, Func<TInput, TResult> evaluator)
        {
            if (source == null)
                return null;

            return source.Select(evaluator);
        }

        #endregion
    }
}