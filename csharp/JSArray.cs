using System;
using System.Collections;
using System.Collections.Generic;

namespace JSLib
{
    public class JSArray<T> : IEnumerable<T>
    {
        private List<T> _raw;

        public JSArray(int arrayLength = 0)
        {
            _raw = new List<T>(arrayLength);
        }

        public JSArray(params T[] items)
        {
            _raw.AddRange(items);
        }

        public delegate bool EveryCallback(T value, int index, JSArray<T> array);
        public delegate bool FilterCallback(T value, int index, JSArray<T> array);
        public delegate bool FindCallback(T value, int index, JSArray<T> array);
        public delegate bool FindAllCallback(T value, int index, JSArray<T> array);
        public delegate bool FindIndexCallback(T value, int index, JSArray<T> array);
        public delegate bool FindLastCallback(T value, int index, JSArray<T> array);
        public delegate bool FindIndexIndexCallback(T value, int index, JSArray<T> array);
        public delegate void ForEachCallback(T value, int index, JSArray<T> array);
        public delegate object MapCallback(T value, int index, JSArray<T> array);
        public delegate object ReduceCallback(object previousValue, T value, int index, JSArray<T> array);
        public delegate object ReduceRightCallback(object previousValue, T value, int index, JSArray<T> array);
        public delegate bool RemoveAllCallback(T value, int index, JSArray<T> array);
        public delegate bool SomeCallback(T value, int index, JSArray<T> array);
        public delegate int SortCallback(T a, T b);


        /// <summary>
        /// Adds a singular item to the array
        /// </summary>
        /// <param name="item">The item to add to the array</param>
        public void Add(T item) => _raw.Add(item);

        /// <summary>
        /// Adds the elements of the specified collection to the end of the array.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of this array</param>
        public void AddRange(IEnumerable<T> collection) => _raw.AddRange(collection);

        /// <summary>
        /// Removes all items from this Array
        /// </summary>
        public void Clear() => _raw.Clear();

        /// <summary>
        /// Combines two or more JS Arrays
        /// </summary>
        /// <param name="items">Additional items to add to the end of array1</param>
        /// <returns></returns>
        public JSDynamicArray Concat(params object[] items)
        {
            JSDynamicArray returnArray = new JSDynamicArray(_raw);

            returnArray.AddRange(items);

            return returnArray;
        }


        /// <summary>
        /// Returns the this object after copying a section of the array identified by start and end to the same array starting at position target
        /// </summary>
        /// <param name="target">If target is negative, it is treated as length+target where length is the length of the array.</param>
        /// <param name="start">If start is negative, it is treated as length+start. If end is negative, it is treated as length+end.</param>
        /// <param name="end">If not specified, length of the this object is used as its default value.</param>
        public JSArray<T> CopyWithin(int target, int start, int end = int.MaxValue)
        {
            if (target < 0)
                target = this.Length + target;

            else if (end == int.MaxValue)
                end = this.Length;

            if (start < 0)
            {
                if (end < 0)
                    start = this.Length + end;
                else
                    start = this.Length + start;
            }

            JSArray<T> targetArray = this.Slice(start, end);

            this.Splice(target, 0, targetArray.ToArray());

            return this;
        }

        /// <summary>
        /// Returns an iterable of key, value pairs for every entry in the array
        /// </summary>
        /// <returns></returns>
        public IterableIterator<int, object> Entries() => null;

        /// <summary>
        /// Determines whether all the members of an array satisfy the specified test.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The every method calls the callbackfn function for each element in the array until the callbackfn returns a value which is coercible to the Boolean value false, or until the end of the array.</param>
        /// <returns></returns>
        public bool Every(EveryCallback callbackfn)
        {
            for (int index = 0; index < this.Length; index++)
            {
                if (!callbackfn(this[index], index, this))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the this object after filling the section identified by start and end with value
        /// </summary>
        /// <param name="value">value to fill array section with</param>
        /// <param name="start">index to start filling the array at. If start is negative, it is treated as length+start where length is the length of the array.</param>
        /// <param name="end">index to stop filling the array at. If end is negative, it is treated as length+end.</param>
        /// <returns></returns>
        public JSArray<T> Fill(T value, int start = 0, int end = int.MaxValue)
        {
            if (start < 0)
                start = this.Length + start;

            if (end < 0)
                end = this.Length + end;

            else if (end == int.MaxValue)
                end = this.Length;

            for (int index = start; index < end; index++)
                this[index] = value;

            return this;
        }

        /// <summary>
        /// Returns the elements of an array that meet the condition specified in a callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The filter method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public JSArray<T> Filter(FilterCallback callbackfn)
        {
            JSArray<T> filteredArray = new JSArray<T>();

            for (int index = 0; index < this.Length; index++)
            {
                if (callbackfn(this[index], index, this))
                    filteredArray.Push(this[index]);
            }

            return filteredArray;
        }

        /// <summary>
        /// Returns the value of the first element in the array where predicate is true, and undefined otherwise.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending order, until it finds one where predicate returns true. If such an element is found, find immediately returns that element value. Otherwise, find returns null.</param>
        /// <returns></returns>
        public T Find(FindCallback predicate)
        {
            for (int index = 0; index < this.Length; index++)
            {
                if (predicate(this[index], index, this))
                    return this[index];
            }

            return default(T);
        }

        /// <summary>
        /// Returns an array of all elements in the array where predicate is true.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending order, until it finds one where predicate returns true. If such an element is found, find immediately returns that element value. Otherwise, find returns null.</param>
        /// <returns></returns>
        public JSArray<T> FindAll(FindAllCallback predicate)
        {
            JSArray<T> result = new JSArray<T>();

            for (int index = 0; index < this.Length; index++)
            {
                if (predicate(this[index], index, this))
                    result.Push(this[index]);
            }

            return result;
        }

        /// <summary>
        /// Returns the index of the first element in the array where predicate is true, and -1 otherwise.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending order, until it finds one where predicate returns true. If such an element is found, findIndex immediately returns that element index. Otherwise, findIndex returns -1.</param>
        /// <returns></returns>
        public int FindIndex(FindIndexCallback predicate)
        {
            for (int index = 0; index < this.Length; index++)
            {
                if (predicate(this[index], index, this))
                    return index;
            }

            return -1;
        }

        /// <summary>
        /// Returns the value of the last element in the array where predicate is true, and undefined otherwise if the predicate is never true.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending order, until it finds one where predicate returns true. If such an element is found, find immediately returns that element value. Otherwise, find returns null.</param>
        /// <returns></returns>
        public T FindLast(FindLastCallback predicate)
        {
            T lastResult = default(T);

            for (int index = 0; index < this.Length; index++)
            {
                if (predicate(this[index], index, this))
                    lastResult = this[index];
            }

            return lastResult;
        }

        /// <summary>
        /// Returns the index of the last element in the array where predicate is true, and -1 otherwise if the predicate is never true.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending order, until it finds one where predicate returns true. If such an element is found, findIndex immediately returns that element index. Otherwise, findIndex returns -1.</param>
        /// <returns></returns>
        public int FindLastIndex(FindIndexCallback predicate)
        {
            int lastResult = -1;
            for (int index = 0; index < this.Length; index++)
            {
                if (predicate(this[index], index, this))
                    lastResult = index;
            }

            return lastResult;
        }

        /// <summary>
        /// Performs the specified action for each element in an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. forEach calls the callbackfn function one time for each element in the array.</param>
        public void ForEach(ForEachCallback callbackfn)
        {
            for (int index = 0; index < this.Length; index++)
                callbackfn(this[index], index, this);
        }

        /// <summary>
        /// Determines whether an array includes a certain element, returning true or false as appropriate.
        /// </summary>
        /// <param name="searchElement">The element to search for.</param>
        /// <param name="fromIndex">The position in this array at which to begin searching for searchElement.</param>
        /// <returns></returns>
        public bool Includes(T searchElement, int fromIndex = 0)
        {
            for (int index = fromIndex; index < this.Length; index++)
            {
                if (EqualityComparer<T>.Default.Equals(this[index], searchElement))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the index of the first occurrence of a value in an array.
        /// </summary>
        /// <param name="searchElement">The value to locate in the array.</param>
        /// <param name="fromIndex">The array index at which to begin the search. If fromIndex is omitted, the search starts at index 0.</param>
        /// <returns></returns>
        public int IndexOf(T searchElement, int fromIndex = 0)
        {
            for (int index = fromIndex; index < this.Length; index++)
            {
                if (EqualityComparer<T>.Default.Equals(this[index], searchElement))
                    return index;
            }

            return -1;
        }

        /// <summary>
        /// Exact same as doing .Splice(index, 0, item)
        /// </summary>
        /// <param name="index">The index to add the item to</param>
        /// <param name="item">The item to add to the array</param>
        public void Insert(int index, T item) => this.Splice(index, 0, item);

        /// <summary>
        /// Exact same as doing .Splice(index, 0, ...collection)
        /// </summary>
        /// <param name="index">The index to add the item to</param>
        /// <param name="collection">The collection to add to the array</param>
        public void InsertRange(int index, IEnumerable<T> collection) => this.Splice(index, 0, new List<T>(collection).ToArray());

        /// <summary>
        /// Adds all the elements of an array separated by the specified separator string.
        /// </summary>
        /// <param name="seperator">A string used to separate one element of an array from the next in the resulting String. If omitted, the array elements are separated with a comma.</param>
        /// <returns></returns>
        public string Join(string seperator = ",")
        {
            string result = "";

            for (int index = 0; index < this.Length; index++)
            {
                if (index > 0)
                    result += seperator;

                result += this[index].ToString();
            }

            return result;
        }

        /// <summary>
        /// Returns an iterable of keys in the array
        /// </summary>
        /// <returns></returns>
        public int[] Keys()
        {
            List<int> result = new List<int>();

            for (int index = 0; index < this.Length; index++)
                result.Add(index);

            return result.ToArray();
        }

        /// <summary>
        /// Returns the index of the last occurrence of a specified value in an array.
        /// </summary>
        /// <param name="searchElement">The value to locate in the array.</param>
        /// <param name="fromIndex">The array index at which to begin the search. If fromIndex is omitted, the search starts at the last index in the array.</param>
        /// <returns></returns>
        public int LastIndexOf(T searchElement, int fromIndex = 0)
        {
            int lastIndex = -1;

            for (int index = fromIndex; index < this.Length; index++)
            {
                if (EqualityComparer<T>.Default.Equals(this[index], searchElement))
                    lastIndex = index;
            }

            return lastIndex;
        }

        /// <summary>
        /// Gets or sets the length of the array. This is a number one higher than the highest element defined in an array.
        /// </summary>
        public int Length => _raw.Count;

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public JSDynamicArray Map(MapCallback callbackfn)
        {
            JSDynamicArray newArray = new JSDynamicArray();

            for (int index = 0; index < this.Length; index++)
                newArray.Push(callbackfn(this[index], index, this));

            return newArray;
        }

        /// <summary>
        /// Removes the last element from an array and returns it.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (this.Length > 0)
                return this.RemoveAt(this.Length - 1);

            return default(T);
        }

        /// <summary>
        /// Appends new elements to an array, and returns the new length of the array.
        /// </summary>
        /// <param name="items">New elements of the Array.</param>
        /// <returns></returns>
        public int Push(params T[] items)
        {
            _raw.AddRange(items);
            return this.Length;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public object Reduce(ReduceCallback callbackfn) => Reduce(callbackfn, null);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public object Reduce(ReduceCallback callbackfn, object initialValue)
        {
            object result = initialValue;

            for (int index = 0; index < this.Length; index++)
                result = callbackfn(result, this[index], index, this);

            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public object ReduceRight(ReduceRightCallback callbackfn) => ReduceRight(callbackfn, null);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public object ReduceRight(ReduceRightCallback callbackfn, object initialValue)
        {
            object result = initialValue;

            for (int index = 0; index < this.Length; index++)
                result = callbackfn(result, this[this.Length - index - 1], index, this);

            return result;
        }


        /// <summary>
        /// Inherited from System.Collections.List. Removes the first instance of given item from the array
        /// </summary>
        /// <param name="item">The item to remove from the array</param>
        /// <returns></returns>
        public bool Remove(T item) => _raw.Remove(item);

        /// <summary>
        /// Same thing as reassigning this array to a filter function with an inverted output
        /// </summary>
        /// <returns></returns>
        public void RemoveAll(RemoveAllCallback predicate)
        {
            List<T> newArray = new List<T>();

            for (int index = 0; index < this.Length; index++)
            {
                if (!predicate(this[index], index, this))
                    newArray.Add(this[index]);
            }

            _raw = newArray;
        }

        /// <summary>
        /// Inherited from System.Collections.List. Removes the item at the index specified from the array.
        /// </summary>
        /// <param name="index">The index of the item to remove</param>
        /// <returns></returns>
        public T RemoveAt(int index)
        {
            T value = this[index];
            _raw.RemoveAt(index);

            return value;
        }

        /// <summary>
        /// Reverses the elements in an Array.
        /// </summary>
        /// <returns></returns>
        public JSArray<T> Reverse()
        {
            JSArray<T> returnArray = new JSArray<T>();

            for (int index = this.Length - 1; index > 0; index++)
                returnArray.Push(this[index]);

            return returnArray;
        }

        /// <summary>
        /// Removes the first element from an array and returns it.
        /// </summary>
        /// <returns></returns>
        public T Shift()
        {
            if (this.Length > 0)
                return this.RemoveAt(0);

            return default(T);
        }

        /// <summary>
        /// Returns a section of an array.
        /// </summary>
        /// <param name="start">The beginning of the specified portion of the array.</param>
        /// <param name="end">The end of the specified portion of the array. This is exclusive of the element at the index 'end'.</param>
        /// <returns></returns>
        public JSArray<T> Slice(int start = 0, int end = int.MaxValue)
        {
            if (end == int.MaxValue)
                end = this.Length;

            JSArray<T> resultArray = new JSArray<T>();

            for (int index = start; index < end; index++)
                resultArray.Push(this[index]);

            return resultArray;
        }

        /// <summary>
        /// Determines whether the specified callback function returns true for any element of an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The some method calls the callbackfn function for each element in the array until the callbackfn returns a value which is coercible to the Boolean value true, or until the end of the array.</param>
        /// <returns></returns>
        public bool Some(SomeCallback callbackfn)
        {
            for (int index = 0; index < this.Length; index++)
            {
                if (!callbackfn(this[index], index, this))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Sorts an array.
        /// </summary>
        /// <returns></returns>
        public JSArray<T> Sort()
        {
            _raw.Sort();

            return this;
        }

        /// <summary>
        /// Sorts an array.
        /// </summary>
        /// <param name="comparefn">Function used to determine the order of the elements. It is expected to return a negative value if first argument is less than second argument, zero if they're equal and a positive value otherwise. If omitted, the elements are sorted in ascending, ASCII character order.</param>
        /// <returns></returns>
        public JSArray<T> Sort(IComparer<T> comparefn)
        {
            _raw.Sort(comparefn);

            return this;
        }


        /// <summary>
        /// Removes elements from an array and, if necessary, inserts new elements in their place, returning the deleted elements.
        /// </summary>
        /// <param name="start">The zero-based location in the array from which to start removing elements.</param>
        /// <param name="deleteCount">The number of elements to remove.</param>
        /// <returns></returns>
        public JSArray<T> Splice(int start, int deleteCount = 0)
        {
            JSArray<T> deletedElements = new JSArray<T>();

            for (int index = 0; index < deleteCount; index++)
            {
                deletedElements.Push(this[index]);

                _raw.RemoveAt(start);
            }

            return deletedElements;
        }

        /// <summary>
        /// Removes elements from an array and, if necessary, inserts new elements in their place, returning the deleted elements.
        /// </summary>
        /// <param name="start">The zero-based location in the array from which to start removing elements.</param>
        /// <param name="deleteCount">The number of elements to remove.</param>
        /// <param name="items">Elements to insert into the array in place of the deleted elements.</param>
        /// <returns></returns>
        public JSArray<T> Splice(int start, int deleteCount = 0, params T[] items)
        {
            JSArray<T> deletedElements = this.Splice(start, deleteCount);

            for (int index = 0; index < items.Length; index++)
            {
                _raw.Insert(index, items[index]);
            }

            return deletedElements;
        }
        
        /// <summary>
        /// Returns the C# Array representation of this array
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return _raw.ToArray();
        }

        /// <summary>
        /// Returns the C# List representation of this array
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            List<T> returnList = new List<T>();
            returnList.AddRange(_raw);

            return returnList;
        }

        /// <summary>
        /// Returns a string representation of an array. The elements are converted to string using their toLocalString methods.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use JSArray.ToString instead")]
        public string ToLocaleString()
        {
            return this.ToString();
        }

        /// <summary>
        /// Returns a string representation of an array.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Join();
        }

        /// <summary>
        /// Inserts new elements at the start of an array.
        /// </summary>
        /// <param name="items">Elements to insert at the start of the Array.</param>
        /// <returns></returns>
        public int Unshift(params T[] items)
        {
            _raw.InsertRange(0, items);

            return this.Length;
        }

        /// <summary>
        /// Returns an iterable of values in the array
        /// </summary>
        /// <returns></returns>
        public T[] Values()
        {
            List<T> result = new List<T>();

            for (int index = 0; index < this.Length; index++)
                result.Add(this[index]);

            return result.ToArray();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_raw).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)_raw).GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                return _raw[index];
            }
            set
            {
                _raw[index] = value;
            }
        }
    }
}
