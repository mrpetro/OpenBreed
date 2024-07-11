using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Common.Tools.Collections
{
    public class IdMap<T>
    {
        #region Private Fields

        private object _itemsLock = new object();

        private readonly List<T> items = new List<T>();

        private readonly Stack freeIdCache = new Stack();

        #endregion Private Fields

        #region Public Constructors

        public IdMap()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<T> Items => items.Where(item => item is not null);

        public int Count
        {
            get
            {
                return items.Count - freeIdCache.Count;
            }
        }

        #endregion Public Properties

        #region Public Indexers

        public T this[int id]
        {
            get
            {
                if (id == -1)
                    return default(T);

                return items[id];
            }
        }

        #endregion Public Indexers

        #region Public Methods

        public bool TryGetValue(int id, out T value)
        {
            lock (_itemsLock)
            {
                if (id < 0 || id > items.Count - 1)
                {
                    value = default;
                    return false;
                }

                if (freeIdCache.Count == 0)
                {
                    value = items[id];
                    return true;
                }

                if (freeIdCache.Contains(id))
                {
                    value = default;
                    return false;
                }

                value = items[id];
                return true;
            }
        }

        public int NewId()
        {
            lock (_itemsLock)
            {
                if (freeIdCache.Count == 0)
                    return items.Count;

                return (int)freeIdCache.Peek();
            }
        }

        public bool Insert(int id, T item)
        {
            lock (_itemsLock)
            {
                if(freeIdCache.Count == 0 && id == items.Count)
                {
                    items.Add(item);
                    return true;
                }

                if (id != (int)freeIdCache.Peek())
                    return false;

                freeIdCache.Pop();
                items[id] = item;
                return true;
            }
        }

        public int Add(T item)
        {
            lock (_itemsLock)
            {
                if (freeIdCache.Count == 0)
                {
                    items.Add(item);
                    return items.Count - 1;
                }

                int freeId = (int)freeIdCache.Pop();
                items[freeId] = item;
                return freeId;
            }
        }

        public void RemoveById(int id)
        {
            lock (_itemsLock)
            {
                if (id < items.Count - 1)
                {
                    freeIdCache.Push(id);
                    items[id] = default(T);
                }
                else
                    items.RemoveAt(id);
            }
        }

        #endregion Public Methods
    }
}