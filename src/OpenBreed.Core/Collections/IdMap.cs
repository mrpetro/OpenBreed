using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Core.Collections
{
    public class IdMap<T>
    {
        #region Private Fields

        private readonly List<T> items = new List<T>();

        private readonly Stack freeIdCache = new Stack();

        #endregion Private Fields

        #region Public Constructors

        public IdMap()
        {
            Items = new ReadOnlyCollection<T>(items);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<T> Items { get; }

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