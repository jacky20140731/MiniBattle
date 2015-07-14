//#define _NX_DEQUE_CHECK_

using System;
using System.Collections.Generic;
//using System.Linq;

namespace th.nx
{
    public /*sealed*/ class Deque<T> : IList<T>, System.Collections.IList
    {
        public Deque()
                : this(DefaultCapacity)
        {
        }

        public Deque(int capacity)
        {
#if _NX_DEQUE_CHECK_
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException("capacity", "Capacity must be greater than 0.");
#endif  // _NX_DEQUE_CHECK_

            _circularArray = new T[capacity];
            _offset = 0;
            _count = 0;
        }

        public Deque(IEnumerable<T> collection)
        {
#if _NX_DEQUE_CHECK_
            if (collection == null)
                throw new ArgumentException("collection", "collection is null");
#endif  // _NX_DEQUE_CHECK_

            IList<T> list = new List<T>();

            var enumer = collection.GetEnumerator();
            while (enumer.MoveNext())
                list.Add(enumer.Current);

            if (list.Count > 0)
                _circularArray = new T[list.Count];
            else
                _circularArray = new T[DefaultCapacity];

            _offset = 0;
            _count = 0;

            insertWithList(0, list);
        }

        public int Capacity
        {
            get { return _circularArray.Length; }
            set {
#if _NX_DEQUE_CHECK_
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("capacity", "Capacity must be greater than 0.");
#endif  // _NX_DEQUE_CHECK_

                if (value != this.Capacity)
                {
                    T[] newArray = new T[value];
                    _count = Math.Min(value, _count);

                    for (int i = 0; i < _count; ++i)
                        newArray[i] = _circularArray[mapIndex(i)];

                    _circularArray = newArray;
                    _offset = 0;
                }
            }
        }

        /**
         * Queue
         * Sets the capacity to the actual number of elements in the Queue.
         */
        public void TrimToSize()
        {
            if (this.Capacity > _count)
            {
                int newCapacity = (_count > 0 ? _count : DefaultCapacity);
                if (this.Capacity > newCapacity)
                    this.Capacity = newCapacity;
            }
        }

        /**
         * Queue
         * Adds an object to the end of the Queue.
         */
        public void Enqueue(T e)
        {
            Add(e);
        }

        /**
         * Queue
         * Removes and returns the object at the beginning of the Queue.
         */
        public T Dequeue()
        {
#if _NX_DEQUE_CHECK_
            checkIndex(0);
#endif  // _NX_DEQUE_CHECK_

            T e = _circularArray[_offset];
            RemoveAt(0);
            return e;
        }

        /**
         * Queue
         * RReturns the object at the beginning of the Queue without removing it.
         */
        public T Peek()
        {
#if _NX_DEQUE_CHECK_
            checkIndex(0);
#endif  // _NX_DEQUE_CHECK_
            
            return _circularArray[_offset];
        }


        /********* From IList<T> *********/

        public T this [int index]
        {
            get {
#if _NX_DEQUE_CHECK_
                checkIndex(index);
#endif  // _NX_DEQUE_CHECK_

                return _circularArray[mapIndex(index)];
            }
            set {
#if _NX_DEQUE_CHECK_
                if (checkIndex(index))
#endif  // _NX_DEQUE_CHECK_
                    _circularArray[mapIndex(index)] = value;
            }
        }

        public int IndexOf (T e)
        {
            int i = 0;
            var comparer = EqualityComparer<T>.Default;

            for (i = 0; i < _count; ++i)
            {
                if (comparer.Equals(e, _circularArray[mapIndex(i)]))
                    break;
            }

            return ((i < _count) ? i : -1);
        }

        public void Insert (int index, T e)
        {
#if _NX_DEQUE_CHECK_
            if (index == _count || checkIndex(index))
#endif  // _NX_DEQUE_CHECK_
                insertAt(index, e);
        }

        public void RemoveAt (int index)
        {
#if _NX_DEQUE_CHECK_
            if (checkIndex(index))
#endif  // _NX_DEQUE_CHECK_
                removeRange(index, 1);
        }


        /********* From ICollection<T> *********/

        public int Count
        {
            get { return _count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add (T e)
        {
            Insert(_count, e);
        }

        public void Clear ()
        {
            _count = 0;

            if (this.Capacity != DefaultCapacity)
                this.Capacity = DefaultCapacity;
            else
                this.Capacity = DefaultCapacity + 1;
        }

        public bool Contains (T e)
        {
            return IndexOf(e) >= 0;
        }

        public void CopyTo (T[] array, int arrayOffset)
        {
#if _NX_DEQUE_CHECK_
            if (array == null)
                throw new ArgumentNullException("array", "Array is null");

            checkRangeArguments(array.Length, arrayOffset, _count);
#endif  // _NX_DEQUE_CHECK_

            for (int i = 0; i < _count; ++i)
                array[arrayOffset + i] = _circularArray[mapIndex(i)];
        }

        public bool Remove (T e)
        {
            int index = IndexOf(e);
            if (index >= 0)
                RemoveAt(index);

            return (index >= 0);
        }


        /********* From IEnumerable<T> *********/

        public IEnumerator<T> GetEnumerator ()
        {
            for (int i = 0; i != _count; ++i)
                yield return _circularArray[mapIndex(i)];
        }



        /********* From System.Collections.IList *********/

        public bool IsFixedSize
        {
            get {return false;}
        }
        
        //public bool IsReadOnly { get; }

        /*public*/ object System.Collections.IList.this [int index]
        {
            get {
                return this[index];
            }
            set {
#if _NX_DEQUE_CHECK_
                if (!isObjectInheritsT(value))
                    throw new ArgumentException("Item is not of the correct type.", "value");
#endif  // _NX_DEQUE_CHECK_

                this[index] = (T)value;
            }
        }

        /*public*/ int System.Collections.IList.Add (object e)
        {
#if _NX_DEQUE_CHECK_
            if (!isObjectInheritsT(e))
                throw new ArgumentException("Item is not of the correct type.", "value");
#endif  // _NX_DEQUE_CHECK_

            Add((T)e);
            return _count - 1;
        }
        
        //public void Clear ();

        /*public*/ bool System.Collections.IList.Contains (object e)
        {
            bool isContains = false;

#if _NX_DEQUE_CHECK_
            if (isObjectInheritsT(e))
#endif  // _NX_DEQUE_CHECK_
                isContains = Contains((T)e);

            return isContains;
        }

        /*public*/ int System.Collections.IList.IndexOf (object e)
        {
            int i = -1;

#if _NX_DEQUE_CHECK_
            if (isObjectInheritsT(e))
#endif  // _NX_DEQUE_CHECK_
                i = IndexOf((T)e);

            return i;
        }

        /*public*/ void System.Collections.IList.Insert (int index, object e)
        {
#if _NX_DEQUE_CHECK_
            if (isObjectInheritsT(e))
#endif  // _NX_DEQUE_CHECK_
                Insert(index, (T)e);
        }

        /*public*/ void System.Collections.IList.Remove (object e)
        {
#if _NX_DEQUE_CHECK_
            if (isObjectInheritsT(e))
#endif  // _NX_DEQUE_CHECK_
                Remove((T)e);
        }
        
        //public void RemoveAt (int index);


        /********* From System.Collections.ICollection *********/

        //public int Count { get; }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        public void CopyTo (Array array, int arrayOffset)
        {
#if _NX_DEQUE_CHECK_
            if (array == null)
                throw new ArgumentNullException("array", "Destination array cannot be null.");

            checkRangeArguments(array.Length, arrayOffset, _count);
#endif  // _NX_DEQUE_CHECK_

            for (int i = 0; i < _count; ++i)
                array.SetValue(_circularArray[mapIndex(i)], arrayOffset + i);
        }


        /********* From System.Collections.IEnumerable *********/

        //
        /*public*/ System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }



        /********* Private *********/

#if _NX_DEQUE_CHECK_
        //
        private static bool isObjectInheritsT(object obj)
        {
            bool isInherits = false;
            
            if (obj is T)
            {
                isInherits = true;
            }
            else if (obj == null)
            {
                var type = typeof(T);
                
                if ((type.IsClass && !type.IsPointer)  // classes, arrays, and delegates
                        || (type.IsInterface)  // interfaces
                        || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))  // nullable value types
                    isInherits = true;
            }
            
            return isInherits;
        }

        //
        private static void checkRangeArguments(int destLength, int offset, int count)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "Invalid offset " + offset);
            else if (count < 0)
                throw new ArgumentOutOfRangeException("count", "Invalid count " + count);
            else if (destLength < (offset + count))
                throw new ArgumentException("Invalid offset (" + offset + ") or count + (" + count + ") for source length " + destLength);
        }

        //
        private bool checkIndex(int index)
        {
            bool valid = false;

            if (index >= 0 && index < _count)
                valid = true;
            //*
            else
                throw new ArgumentOutOfRangeException("index", "index out of range");
            //*/

            return valid;
        }

#endif  // _NX_DEQUE_CHECK_

        //
        private int mapIndex(int index)
        {
            return ((index + _offset) % this.Capacity);
        }

        private void removeRange(int index, int n)
        {
            int from, to;
            int k = _count - index - n;

            if (index < k)
            {
                // 从前向后移动
                from = index - 1;
                to = index + n - 1;

                while (from >= 0)
                    _circularArray[mapIndex(to--)] = _circularArray[mapIndex(from--)];

                /* g.s.c
                for (int i = 0; i < n; ++i)
                    _circularArray[mapIndex(i)] = (T)null;
                //*/

                _offset = mapIndex(n);
            }
            else
            {
                // 从后向前移动
                from = index + n;
                to = index;

                while (from < _count)
                    _circularArray[mapIndex(to++)] = _circularArray[mapIndex(from++)];

                /* g.s.c
                k = _count - 1 - n;
                for (int i = _count - 1; i > k; --i)
                    _circularArray[mapIndex(i)] = (T)null;
                //*/
            }

            _count -= n;
        }

        private void insertAt(int index, T e)
        {
            ensureCapacity(_count + 1);

            int k = _count - index;
            int from, to;

            if (index < k)
            {
                // 往前移动，留出空当
                _offset -= 1;
                if (_offset < 0)
                    _offset += this.Capacity;

                from = 1;
                to = 0;
                k = index + 1;

                while (from < k)
                    _circularArray[mapIndex(to++)] = _circularArray[mapIndex(from++)];
            }
            else 
            {
                // 往后移动，留出空当
                from = _count - 1;
                to = from + 1;

                while(from >= index)
                    _circularArray[mapIndex(to--)] = _circularArray[mapIndex(from--)];
            }

            _circularArray[mapIndex(index)] = e;

            _count += 1;
        }

        private void insertWithList(int index, IList<T> list)
        {
            ensureCapacity(_count + list.Count);            
            
            int k = _count - index;
            int from, to;
            
            if (index < k)
            {
                // 往前移动，留出空当
                _offset -= list.Count;
                if (_offset < 0)
                    _offset += this.Capacity;
                
                from = list.Count;
                to = 0;
                k = index + list.Count;
                
                while (from < k)
                    _circularArray[mapIndex(to++)] = _circularArray[mapIndex(from++)];
            }
            else 
            {
                // 往后移动，留出空当
                from = _count - 1;
                to = from + list.Count;
                
                while(from >= index)
                    _circularArray[mapIndex(to--)] = _circularArray[mapIndex(from--)];
            }
            
            for (int i = 0; i < list.Count; ++i)
                _circularArray[mapIndex(index + i)] = list[i];

            _count += list.Count;
        }

        private void ensureCapacity(int newCount)
        {
            int newCapacity = calcuNewCapacity(newCount);
            if (newCapacity > this.Capacity)
            {
                T[] newArray = new T[newCapacity];
                
                for (int i = 0; i < _count; ++i)
                    newArray[i] = _circularArray[mapIndex(i)];
                
                _circularArray = newArray;
                _offset = 0;
            }
        }

        private int calcuNewCapacity(int newCount)
        {
            int capacity = this.Capacity;
            while (capacity < newCount)
                capacity <<= 1;

            return capacity;
        }


        private const int DefaultCapacity = 8;

        private T[] _circularArray;
        private int _offset;
        private int _count;
    }
}




#if _FROM_OPEN_SRC_

using System;
using System.Collections.Generic;
using System.Linq;

namespace th.nx
{
    public /*sealed*/ class Deque<T> : IList<T>, System.Collections.IList
    {
        /// <summary>
        /// The default capacity.
        /// </summary>
        private const int DefaultCapacity = 8;

        /// <summary>
        /// The circular buffer that holds the view.
        /// </summary>
        private T[] _buffer;

        /// <summary>
        /// The offset into <see cref="buffer"/> where the view begins.
        /// </summary>
        private int _offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deque&lt;T&gt;"/> class with the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity. Must be greater than <c>0</c>.</param>
        public Deque(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentOutOfRangeException("capacity", "Capacity must be greater than 0.");
            _buffer = new T[capacity];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Deque&lt;T&gt;"/> class with the elements from the specified collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public Deque(IEnumerable<T> collection)
        {
            int count = collection.Count();
            if (count > 0)
            {
                _buffer = new T[count];
                DoInsertRange(0, collection, count);
            }
            else
            {
                _buffer = new T[DefaultCapacity];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Deque&lt;T&gt;"/> class.
        /// </summary>
        public Deque()
            : this(DefaultCapacity)
        {
        }

        #region GenericListImplementations

        /// <summary>
        /// Gets a value indicating whether this list is read-only. This implementation always returns <c>false</c>.
        /// </summary>
        /// <returns>true if this list is read-only; otherwise, false.</returns>
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in this list.</exception>
        /// <exception cref="T:System.NotSupportedException">This property is set and the list is read-only.</exception>
        public T this[int index]
        {
            get
            {
                CheckExistingIndexArgument(this.Count, index);
                return DoGetItem(index);
            }

            set
            {
                CheckExistingIndexArgument(this.Count, index);
                DoSetItem(index, value);
            }
        }

        /// <summary>
        /// Inserts an item to this list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into this list.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in this list.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// This list is read-only.
        /// </exception>
        public void Insert(int index, T item)
        {
            CheckNewIndexArgument(Count, index);
            DoInsert(index, item);
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in this list.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// This list is read-only.
        /// </exception>
        public void RemoveAt(int index)
        {
            CheckExistingIndexArgument(Count, index);
            DoRemoveAt(index);
        }

        /// <summary>
        /// Determines the index of a specific item in this list.
        /// </summary>
        /// <param name="item">The object to locate in this list.</param>
        /// <returns>The index of <paramref name="item"/> if found in this list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            int ret = 0;
            foreach (var sourceItem in this)
            {
                if (comparer.Equals(item, sourceItem))
                    return ret;
                ++ret;
            }

            return -1;
        }

        /// <summary>
        /// Adds an item to the end of this list.
        /// </summary>
        /// <param name="item">The object to add to this list.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// This list is read-only.
        /// </exception>
        void ICollection<T>.Add(T item)
        {
            DoInsert(Count, item);
        }

        /// <summary>
        /// Determines whether this list contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in this list.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in this list; otherwise, false.
        /// </returns>
        bool ICollection<T>.Contains(T item)
        {
            return this.Contains(item, null);
        }

        /// <summary>
        /// Copies the elements of this list to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from this slice. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// </exception>
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "Array is null");

            int count = this.Count;
            CheckRangeArguments(array.Length, arrayIndex, count);
            for (int i = 0; i != count; ++i)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from this list.
        /// </summary>
        /// <param name="item">The object to remove from this list.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from this list; otherwise, false. This method also returns false if <paramref name="item"/> is not found in this list.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// This list is read-only.
        /// </exception>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;

            DoRemoveAt(index);
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            int count = this.Count;
            for (int i = 0; i != count; ++i)
            {
                yield return DoGetItem(i);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
        #region ObjectListImplementations

        /// <summary>
        /// Returns whether or not the type of a given item indicates it is appropriate for storing in this container.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns><c>true</c> if the item is appropriate to store in this container; otherwise, <c>false</c>.</returns>
        private bool ObjectIsT(object item)
        {
            if (item is T)
            {
                return true;
            }

            if (item == null)
            {
                var type = typeof(T);
                if (type.IsClass && !type.IsPointer)
                    return true; // classes, arrays, and delegates
                if (type.IsInterface)
                    return true; // interfaces
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return true; // nullable value types
            }

            return false;
        }

        int System.Collections.IList.Add(object value)
        {
            if (!ObjectIsT(value))
                throw new ArgumentException("Item is not of the correct type.", "value");
            AddToBack((T)value);
            return Count - 1;
        }

        bool System.Collections.IList.Contains(object value)
        {
            if (!ObjectIsT(value))
                throw new ArgumentException("Item is not of the correct type.", "value");
            return this.Contains((T)value);
        }

        int System.Collections.IList.IndexOf(object value)
        {
            if (!ObjectIsT(value))
                throw new ArgumentException("Item is not of the correct type.", "value");
            return IndexOf((T)value);
        }

        void System.Collections.IList.Insert(int index, object value)
        {
            if (!ObjectIsT(value))
                throw new ArgumentException("Item is not of the correct type.", "value");
            Insert(index, (T)value);
        }

        bool System.Collections.IList.IsFixedSize
        {
            get { return false; }
        }

        bool System.Collections.IList.IsReadOnly
        {
            get { return false; }
        }

        void System.Collections.IList.Remove(object value)
        {
            if (!ObjectIsT(value))
                throw new ArgumentException("Item is not of the correct type.", "value");
            Remove((T)value);
        }

        object System.Collections.IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                if (!ObjectIsT(value))
                    throw new ArgumentException("Item is not of the correct type.", "value");
                this[index] = (T)value;
            }
        }

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array", "Destination array cannot be null.");
            CheckRangeArguments(array.Length, index, Count);

            for (int i = 0; i != Count; ++i)
            {
                try
                {
                    array.SetValue(this[i], index + i);
                }
                catch (InvalidCastException ex)
                {
                    throw new ArgumentException("Destination array is of incorrect type.", ex);
                }
            }
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get { return this; }
        }

        #endregion
        #region GenericListHelpers

        /// <summary>
        /// Checks the <paramref name="index"/> argument to see if it refers to a valid insertion point in a source of a given length.
        /// </summary>
        /// <param name="sourceLength">The length of the source. This parameter is not checked for validity.</param>
        /// <param name="index">The index into the source.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index to an insertion point for the source.</exception>
        private static void CheckNewIndexArgument(int sourceLength, int index)
        {
            if (index < 0 || index > sourceLength)
            {
                throw new ArgumentOutOfRangeException("index", "Invalid new index " + index + " for source length " + sourceLength);
            }
        }

        /// <summary>
        /// Checks the <paramref name="index"/> argument to see if it refers to an existing element in a source of a given length.
        /// </summary>
        /// <param name="sourceLength">The length of the source. This parameter is not checked for validity.</param>
        /// <param name="index">The index into the source.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index to an existing element for the source.</exception>
        private static void CheckExistingIndexArgument(int sourceLength, int index)
        {
            if (index < 0 || index >= sourceLength)
            {
                throw new ArgumentOutOfRangeException("index", "Invalid existing index " + index + " for source length " + sourceLength);
            }
        }

        /// <summary>
        /// Checks the <paramref name="offset"/> and <paramref name="count"/> arguments for validity when applied to a source of a given length. Allows 0-element ranges, including a 0-element range at the end of the source.
        /// </summary>
        /// <param name="sourceLength">The length of the source. This parameter is not checked for validity.</param>
        /// <param name="offset">The index into source at which the range begins.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <exception cref="ArgumentOutOfRangeException">Either <paramref name="offset"/> or <paramref name="count"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">The range [offset, offset + count) is not within the range [0, sourceLength).</exception>
        private static void CheckRangeArguments(int sourceLength, int offset, int count)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "Invalid offset " + offset);
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "Invalid count " + count);
            }

            if (sourceLength - offset < count)
            {
                throw new ArgumentException("Invalid offset (" + offset + ") or count + (" + count + ") for source length " + sourceLength);
            }
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        private bool IsEmpty
        {
            get { return Count == 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is at full capacity.
        /// </summary>
        private bool IsFull
        {
            get { return Count == Capacity; }
        }

        /// <summary>
        /// Gets a value indicating whether the buffer is "split" (meaning the beginning of the view is at a later index in <see cref="buffer"/> than the end).
        /// </summary>
        private bool IsSplit
        {
            get
            {
                // Overflow-safe version of "(offset + Count) > Capacity"
                return _offset > (Capacity - Count);
            }
        }

        /// <summary>
        /// Gets or sets the capacity for this deque. This value must always be greater than zero, and this property cannot be set to a value less than <see cref="Count"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><c>Capacity</c> cannot be set to a value less than <see cref="Count"/>.</exception>
        public int Capacity
        {
            get
            {
                return _buffer.Length;
            }

            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "Capacity must be greater than 0.");

                if (value < Count)
                    throw new InvalidOperationException("Capacity cannot be set to a value less than Count");

                if (value == _buffer.Length)
                    return;

                // Create the new buffer and copy our existing range.
                T[] newBuffer = new T[value];
                if (IsSplit)
                {
                    // The existing buffer is split, so we have to copy it in parts
                    int length = Capacity - _offset;
                    Array.Copy(_buffer, _offset, newBuffer, 0, length);
                    Array.Copy(_buffer, 0, newBuffer, length, Count - length);
                }
                else
                {
                    // The existing buffer is whole
                    Array.Copy(_buffer, _offset, newBuffer, 0, Count);
                }

                // Set up to use the new buffer.
                _buffer = newBuffer;
                _offset = 0;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in this deque.
        /// </summary>
        /// <returns>The number of elements contained in this deque.</returns>
        public int Count { get; private set; }
        
        /// <summary>
        /// Applies the offset to <paramref name="index"/>, resulting in a buffer index.
        /// </summary>
        /// <param name="index">The deque index.</param>
        /// <returns>The buffer index.</returns>
        private int DequeIndexToBufferIndex(int index)
        {
            return (index + _offset) % Capacity;
        }

        /// <summary>
        /// Gets an element at the specified view index.
        /// </summary>
        /// <param name="index">The zero-based view index of the element to get. This index is guaranteed to be valid.</param>
        /// <returns>The element at the specified index.</returns>
        private T DoGetItem(int index)
        {
            return _buffer[DequeIndexToBufferIndex(index)];
        }

        /// <summary>
        /// Sets an element at the specified view index.
        /// </summary>
        /// <param name="index">The zero-based view index of the element to get. This index is guaranteed to be valid.</param>
        /// <param name="item">The element to store in the list.</param>
        private void DoSetItem(int index, T item)
        {
            _buffer[DequeIndexToBufferIndex(index)] = item;
        }

        /// <summary>
        /// Inserts an element at the specified view index.
        /// </summary>
        /// <param name="index">The zero-based view index at which the element should be inserted. This index is guaranteed to be valid.</param>
        /// <param name="item">The element to store in the list.</param>
        private void DoInsert(int index, T item)
        {
            EnsureCapacityForOneElement();

            if (index == 0)
            {
                DoAddToFront(item);
                return;
            }
            else if (index == Count)
            {
                DoAddToBack(item);
                return;
            }

            DoInsertRange(index, new[] { item }, 1);
        }

        /// <summary>
        /// Removes an element at the specified view index.
        /// </summary>
        /// <param name="index">The zero-based view index of the element to remove. This index is guaranteed to be valid.</param>
        private void DoRemoveAt(int index)
        {
            if (index == 0)
            {
                DoRemoveFromFront();
                return;
            }
            else if (index == Count - 1)
            {
                DoRemoveFromBack();
                return;
            }

            DoRemoveRange(index, 1);
        }

        /// <summary>
        /// Increments <see cref="offset"/> by <paramref name="value"/> using modulo-<see cref="Capacity"/> arithmetic.
        /// </summary>
        /// <param name="value">The value by which to increase <see cref="offset"/>. May not be negative.</param>
        /// <returns>The value of <see cref="offset"/> after it was incremented.</returns>
        private int PostIncrement(int value)
        {
            int ret = _offset;
            _offset += value;
            _offset %= Capacity;
            return ret;
        }

        /// <summary>
        /// Decrements <see cref="offset"/> by <paramref name="value"/> using modulo-<see cref="Capacity"/> arithmetic.
        /// </summary>
        /// <param name="value">The value by which to reduce <see cref="offset"/>. May not be negative or greater than <see cref="Capacity"/>.</param>
        /// <returns>The value of <see cref="offset"/> before it was decremented.</returns>
        private int PreDecrement(int value)
        {
            _offset -= value;
            if (_offset < 0)
                _offset += Capacity;
            return _offset;
        }

        /// <summary>
        /// Inserts a single element to the back of the view. <see cref="IsFull"/> must be false when this method is called.
        /// </summary>
        /// <param name="value">The element to insert.</param>
        private void DoAddToBack(T value)
        {
            _buffer[DequeIndexToBufferIndex(Count)] = value;
            ++Count;
        }

        /// <summary>
        /// Inserts a single element to the front of the view. <see cref="IsFull"/> must be false when this method is called.
        /// </summary>
        /// <param name="value">The element to insert.</param>
        private void DoAddToFront(T value)
        {
            _buffer[PreDecrement(1)] = value;
            ++Count;
        }

        /// <summary>
        /// Removes and returns the last element in the view. <see cref="IsEmpty"/> must be false when this method is called.
        /// </summary>
        /// <returns>The former last element.</returns>
        private T DoRemoveFromBack()
        {
            T ret = _buffer[DequeIndexToBufferIndex(Count - 1)];
            --Count;
            return ret;
        }

        /// <summary>
        /// Removes and returns the first element in the view. <see cref="IsEmpty"/> must be false when this method is called.
        /// </summary>
        /// <returns>The former first element.</returns>
        private T DoRemoveFromFront()
        {
            --Count;
            return _buffer[PostIncrement(1)];
        }

        /// <summary>
        /// Inserts a range of elements into the view.
        /// </summary>
        /// <param name="index">The index into the view at which the elements are to be inserted.</param>
        /// <param name="collection">The elements to insert.</param>
        /// <param name="collectionCount">The number of elements in <paramref name="collection"/>. Must be greater than zero, and the sum of <paramref name="collectionCount"/> and <see cref="Count"/> must be less than or equal to <see cref="Capacity"/>.</param>
        private void DoInsertRange(int index, IEnumerable<T> collection, int collectionCount)
        {
            // Make room in the existing list
            if (index < Count / 2)
            {
                // Inserting into the first half of the list

                // Move lower items down: [0, index) -> [Capacity - collectionCount, Capacity - collectionCount + index)
                // This clears out the low "index" number of items, moving them "collectionCount" places down;
                //   after rotation, there will be a "collectionCount"-sized hole at "index".
                int copyCount = index;
                int writeIndex = Capacity - collectionCount;
                for (int j = 0; j != copyCount; ++j)
                    _buffer[DequeIndexToBufferIndex(writeIndex + j)] = _buffer[DequeIndexToBufferIndex(j)];

                // Rotate to the new view
                this.PreDecrement(collectionCount);
            }
            else
            {
                // Inserting into the second half of the list

                // Move higher items up: [index, count) -> [index + collectionCount, collectionCount + count)
                int copyCount = Count - index;
                int writeIndex = index + collectionCount;
                for (int j = copyCount - 1; j != -1; --j)
                    _buffer[DequeIndexToBufferIndex(writeIndex + j)] = _buffer[DequeIndexToBufferIndex(index + j)];
            }

            // Copy new items into place
            int i = index;
            foreach (T item in collection)
            {
                _buffer[DequeIndexToBufferIndex(i)] = item;
                ++i;
            }

            // Adjust valid count
            Count += collectionCount;
        }

        /// <summary>
        /// Removes a range of elements from the view.
        /// </summary>
        /// <param name="index">The index into the view at which the range begins.</param>
        /// <param name="collectionCount">The number of elements in the range. This must be greater than 0 and less than or equal to <see cref="Count"/>.</param>
        private void DoRemoveRange(int index, int collectionCount)
        {
            if (index == 0)
            {
                // Removing from the beginning: rotate to the new view
                this.PostIncrement(collectionCount);
                Count -= collectionCount;
                return;
            }
            else if (index == Count - collectionCount)
            {
                // Removing from the ending: trim the existing view
                Count -= collectionCount;
                return;
            }

            if ((index + (collectionCount / 2)) < Count / 2)
            {
                // Removing from first half of list

                // Move lower items up: [0, index) -> [collectionCount, collectionCount + index)
                int copyCount = index;
                int writeIndex = collectionCount;
                for (int j = copyCount - 1; j != -1; --j)
                    _buffer[DequeIndexToBufferIndex(writeIndex + j)] = _buffer[DequeIndexToBufferIndex(j)];

                // Rotate to new view
                this.PostIncrement(collectionCount);
            }
            else
            {
                // Removing from second half of list

                // Move higher items down: [index + collectionCount, count) -> [index, count - collectionCount)
                int copyCount = Count - collectionCount - index;
                int readIndex = index + collectionCount;
                for (int j = 0; j != copyCount; ++j)
                    _buffer[DequeIndexToBufferIndex(index + j)] = _buffer[DequeIndexToBufferIndex(readIndex + j)];
            }

            // Adjust valid count
            Count -= collectionCount;
        }

        /// <summary>
        /// Doubles the capacity if necessary to make room for one more element. When this method returns, <see cref="IsFull"/> is false.
        /// </summary>
        private void EnsureCapacityForOneElement()
        {
            if (this.IsFull)
            {
                this.Capacity = this.Capacity * 2;
            }
        }

        /// <summary>
        /// Inserts a single element at the back of this deque.
        /// </summary>
        /// <param name="value">The element to insert.</param>
        public void AddToBack(T value)
        {
            EnsureCapacityForOneElement();
            DoAddToBack(value);
        }

        /// <summary>
        /// Inserts a single element at the front of this deque.
        /// </summary>
        /// <param name="value">The element to insert.</param>
        public void AddToFront(T value)
        {
            EnsureCapacityForOneElement();
            DoAddToFront(value);
        }

        /// <summary>
        /// Inserts a collection of elements into this deque.
        /// </summary>
        /// <param name="index">The index at which the collection is inserted.</param>
        /// <param name="collection">The collection of elements to insert.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index to an insertion point for the source.</exception>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            int collectionCount = collection.Count();
            CheckNewIndexArgument(Count, index);

            // Overflow-safe check for "this.Count + collectionCount > this.Capacity"
            if (collectionCount > Capacity - Count)
            {
                this.Capacity = checked(Count + collectionCount);
            }

            if (collectionCount == 0)
            {
                return;
            }

            this.DoInsertRange(index, collection, collectionCount);
        }

        /// <summary>
        /// Removes a range of elements from this deque.
        /// </summary>
        /// <param name="offset">The index into the deque at which the range begins.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Either <paramref name="offset"/> or <paramref name="count"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">The range [<paramref name="offset"/>, <paramref name="offset"/> + <paramref name="count"/>) is not within the range [0, <see cref="Count"/>).</exception>
        public void RemoveRange(int offset, int count)
        {
            CheckRangeArguments(Count, offset, count);

            if (count == 0)
            {
                return;
            }

            this.DoRemoveRange(offset, count);
        }

        /// <summary>
        /// Removes and returns the last element of this deque.
        /// </summary>
        /// <returns>The former last element.</returns>
        /// <exception cref="InvalidOperationException">The deque is empty.</exception>
        public T RemoveFromBack()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The deque is empty.");

            return this.DoRemoveFromBack();
        }

        /// <summary>
        /// Removes and returns the first element of this deque.
        /// </summary>
        /// <returns>The former first element.</returns>
        /// <exception cref="InvalidOperationException">The deque is empty.</exception>
        public T RemoveFromFront()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The deque is empty.");

            return this.DoRemoveFromFront();
        }

        /// <summary>
        /// Removes all items from this deque.
        /// </summary>
        public void Clear()
        {
            _offset = 0;
            this.Count = 0;
        }

        /*
        public T Dequeue();
        public void Enqueue(T item);
        public T Peek();
        public T[] ToArray();
        public void TrimExcess();
        //*/

        //[DebuggerNonUserCode]
        private sealed class DebugView
        {
            private readonly Deque<T> deque;

            public DebugView(Deque<T> deque)
            {
                this.deque = deque;
            }

            //[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T[] Items
            {
                get
                {
                    var array = new T[deque.Count];
                    ((ICollection<T>)deque).CopyTo(array, 0);
                    return array;
                }
            }
        }
    }
}

#endif  // _FROM_OPEN_SRC_
