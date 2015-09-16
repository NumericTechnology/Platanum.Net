/*
 * Copyright 2011-2015 Numeric Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;

namespace KPComponents.Generic
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public abstract class StateManagedCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IStateManager,
        IList, ICollection
        where T : class, IStateManagedItem, new()
    {
        #region Fields

        private List<T> items = new List<T>();
        private bool saveAll;
        private bool tracking;

        #endregion

        #region Methods

        #endregion

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            items.Insert(index, item);
            if (this.tracking)
            {
                this.saveAll = true;
            }
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            if (this.tracking)
            {
                this.saveAll = true;
            }
        }

        public T this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
            if (this.tracking)
            {
                this.saveAll = true;
            }
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }

        #endregion

        #region IStateManager Members

        public bool IsTrackingViewState
        {
            get { return tracking; }
        }

        public bool IsSaveAll
        {
            get { return this.saveAll; }
        }

        public void LoadViewState(object state)
        {
            if (state != null)
            {
                Pair p = state as Pair;
                if (p != null)
                {
                    int count = (int)p.First;
                    object[] savedItems = (object[])p.Second;
                    foreach (object savedState in savedItems)
                    {
                        T item = new T();
                        item.TrackViewState();
                        item.LoadViewState(savedState);
                        items.Add(item);
                    }
                }
            }
        }

        public object SaveViewState()
        {
            object[] saveList = new object[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                T item = items[i];
                SetItemDirty(item);
                saveList[i] = item.SaveViewState();
            }
            return new Pair(items.Count, saveList);
        }

        public void TrackViewState()
        {
            this.tracking = true;
            foreach (IStateManager item in this.items)
            {
                item.TrackViewState();
            }
        }

        public void SetDirty()
        {
            foreach (T item in items)
            {
                SetItemDirty(item);
            }
        }

        protected virtual void SetItemDirty(T item)
        {
            item.SetDirty();
        }

        #endregion

        #region IList Members

        int IList.Add(object value)
        {
            Add(value as T);
            return Count - 1;
        }

        void IList.Clear()
        {
            Clear();
        }

        bool IList.Contains(object value)
        {
            return Contains(value as T);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf(value as T);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, value as T);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            Remove(value as T);
        }

        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = value as T; }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return null; }
        }

        #endregion
    }
}