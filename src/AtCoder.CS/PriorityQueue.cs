using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.CS
{
    public class PriorityQueue<T> : IEnumerable<T>
    {
        private readonly SortedDictionary<T, int> _data;

        public PriorityQueue() : this(null, null)
        {
        }

        public PriorityQueue(IComparer<T> comparer = null)
            : this(null, comparer)
        {
        }

        public PriorityQueue(int count, IComparer<T> comparer = null)
            : this(Enumerable.Repeat(default(T), count), comparer)
        {
        }

        public PriorityQueue(IEnumerable<T> data = null, IComparer<T> comparer = null)
        {
            comparer ??= Comparer<T>.Default;
            var list = data?.ToList() ?? new List<T>();
            var dict = new Dictionary<T, int>();
            foreach (var val in list)
            {
                if (!dict.ContainsKey(val)) dict[val] = 0;
                dict[val]++;
            }

            _data = new SortedDictionary<T, int>(dict, comparer);
        }

        public void Enqueue(T item)
        {
            if (!_data.ContainsKey(item)) _data[item] = 0;
            _data[item]++;
        }

        public T Dequeue()
        {
            if (!_data.Any()) throw new InvalidOperationException();
            var key = Peek();
            if (_data[key] > 1) _data[key]--;
            else _data.Remove(key);
            return key;
        }

        public T Peek()
        {
            if (!_data.Any()) throw new InvalidOperationException();
            using var e = _data.GetEnumerator();
            e.MoveNext();
            var (ret, _) = e.Current;
            return ret;
        }

        public bool TryDequeue(out T result)
        {
            if (_data.Any())
            {
                result = Dequeue();
                return true;
            }

            result = default;
            return false;
        }

        public bool TryPeek(out T result)
        {
            if (_data.Any())
            {
                result = Peek();
                return true;
            }

            result = default;
            return false;
        }

        public void Clear() => _data.Clear();
        public bool Contains(T item) => _data.ContainsKey(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            GetFlatData().ToList().CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() => GetFlatData().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<T> GetFlatData() => _data.SelectMany(x => Enumerable.Repeat(x.Key, x.Value));
    }
}