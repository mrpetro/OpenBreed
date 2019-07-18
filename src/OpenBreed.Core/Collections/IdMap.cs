using System.Collections;
using System.Collections.Generic;

namespace OpenBreed.Core.Collections
{
    public class IdMap<T>
    {
        #region Private Fields

        private readonly List<T> items = new List<T>();

        private readonly Stack freeIdCache = new Stack();

        #endregion Private Fields

        #region Public Indexers

        public int Count
        {
            get
            {
                return items.Count - freeIdCache.Count;
            }
        }

        public T this[int id]
        {
            get
            {
                return items[id];
            }
        }

        #endregion Public Indexers

        #region Public Methods

        public bool TryGetValue(int id, out T value)
        {
            if (id < 0 || id > items.Count - 1)
            {
                value = default(T);
                return false;
            }
            else
            {
                if (freeIdCache.Count == 0)
                {
                    value = items[id];
                    return true;
                }
                else
                {
                    if (freeIdCache.Contains(id))
                    {
                        value = default(T);
                        return false;
                    }
                    else
                    {
                        value = items[id];
                        return true;
                    }
                }
            }
        }

        public int Add(T item)
        {
            if (freeIdCache.Count == 0)
            {
                items.Add(item);
                return items.Count - 1;
            }
            else
            {
                int freeId = (int)freeIdCache.Pop();
                items[freeId] = item;
                return freeId;
            }
        }

        public void RemoveById(int id)
        {
            if (id < freeIdCache.Count - 1)
            {
                freeIdCache.Push(id);
                items[id] = default(T);
            }
            else
                items.RemoveAt(id);
        }

        #endregion Public Methods
    }
}