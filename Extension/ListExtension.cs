using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System 
{
    /// <summary>
    /// 对List集合的排除重复、交集、并集、差集的扩展，支持指定列，例如：p>=p.Guid
    /// </summary>
    public static class ListExtension
    {
        #region 扩展Distinct方法(返回序列中的非重复元素,支持指定列)
        /// <summary>
        /// 通过使用指定的列对值进行比较返回序列中的非重复元素。
        /// </summary>
        /// <param name="source">要从中移除重复元素的序列。</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数。</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> predicate)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(predicate));
        }
        /// <summary>
        /// 通过使用默认的相等比较器对值进行比较返回序列中的非重复元素。
        /// </summary>
        /// <param name="source">要从中移除重复元素的序列</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数</param>
        /// <param name="comparer">用于比较值的 System.Collections.Generic.IEqualityComparer。</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> predicate, IEqualityComparer<V> comparer)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(predicate, comparer));
        }
        #endregion

        #region 扩展Intersect方法(返回两个序列的交集,支持指定列)
        /// <summary>
        /// 通过使用指定的列对值进行比较生成两个序列的交集。
        /// </summary>
        /// <param name="first">一个 System.Collections.Generic.IEnumerable&lt;T&gt;，将返回其也出现在 second 中的非重复元素。</param>
        /// <param name="second">一个 System.Collections.Generic.IEnumerable&lt;T&gt;，将返回其也出现在第一个序列中的非重复元素。</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数。</param>
        /// <returns></returns>
        public static IEnumerable<T> Intersect<T, V>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, V> predicate)
        {
            return first.Intersect(second, new CommonEqualityComparer<T, V>(predicate));
        }
        #endregion

        #region 扩展Union方法(返回两个序列的并集,支持指定列)
        /// <summary>
        /// 通过使用指定的列生成两个序列的并集。
        /// </summary>
        /// <param name="first">一个 System.Collections.Generic.IEnumerable&lt;T&gt;，它的非重复元素构成联合的第一个集。</param>
        /// <param name="second">一个 System.Collections.Generic.IEnumerable&lt;T&gt;，它的非重复元素构成联合的第二个集。</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数。</param>
        /// <returns></returns>
        public static IEnumerable<T> Union<T, V>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, V> predicate)
        {
            return first.Union(second, new CommonEqualityComparer<T, V>(predicate));
        }
        #endregion

        #region 扩展Except方法(返回两个序列的差集,支持指定列)
        /// <summary>
        /// 通过使用指定的列生成两个序列的差集。
        /// </summary>
        /// <param name="first">一个 System.Collections.Generic.IEnumerable&lt;T&gt;，将返回其不在 second 中的元素。</param>
        /// <param name="second"> 一个 System.Collections.Generic.IEnumerable&lt;T&gt;，如果它的元素也出现在第一个序列中，则将导致从返回的序列中移除这些元素。</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数。</param>
        /// <returns></returns>
        public static IEnumerable<T> Except<T, V>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, V> predicate)
        {
            return first.Except(second, new CommonEqualityComparer<T, V>(predicate));
        }
        #endregion
    }
    #region 自定义比较器
    /// <summary>
    /// 自定义比较器
    /// </summary>
    public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        private Func<T, V> keySelector;
        private IEqualityComparer<V> comparer;

        public CommonEqualityComparer(Func<T, V> keySelector, IEqualityComparer<V> comparer)
        {
            this.keySelector = keySelector;
            this.comparer = comparer;
        }

        public CommonEqualityComparer(Func<T, V> keySelector)
            : this(keySelector, EqualityComparer<V>.Default)
        { }

        public bool Equals(T x, T y)
        {
            return comparer.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return comparer.GetHashCode(keySelector(obj));
        }
    }
    #endregion
}
