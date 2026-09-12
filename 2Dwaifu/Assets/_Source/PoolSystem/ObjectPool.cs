using System;
using System.Collections.Generic;

namespace PoolSystem
{
    public sealed class ObjectPool<T> where T : class
    {
        private readonly Func<T> _create;
        private readonly Action<T> _reset;
        private readonly Queue<T> _available = new();
        private readonly HashSet<T> _active = new();

        public int ActiveCount => _active.Count;
        public int AvailableCount => _available.Count;

        public ObjectPool(Func<T> create, Action<T> reset, int initialSize)
        {
            _create = create ?? throw new ArgumentNullException(nameof(create));
            _reset = reset ?? throw new ArgumentNullException(nameof(reset));
            if (initialSize < 0)
                throw new ArgumentOutOfRangeException(nameof(initialSize));

            for (int i = 0; i < initialSize; i++)
                _available.Enqueue(CreateItem());
        }

        public T Get()
        {
            T item = _available.Count > 0 ? _available.Dequeue() : CreateItem();
            _active.Add(item);
            return item;
        }

        public bool Release(T item)
        {
            if (item == null || !_active.Remove(item))
                return false;

            _reset(item);
            _available.Enqueue(item);
            return true;
        }

        public void ReleaseAll()
        {
            // Returning an item changes the active set, so iterate a snapshot.
            T[] items = new T[_active.Count];
            _active.CopyTo(items);
            foreach (T item in items)
                Release(item);
        }

        private T CreateItem()
        {
            T item = _create();
            if (item == null)
                throw new InvalidOperationException("The pool factory returned null.");

            _reset(item);
            return item;
        }
    }
}
