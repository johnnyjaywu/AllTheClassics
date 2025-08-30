using System;
using System.Collections.Generic;

namespace ContentContent.Core
{
	public static class ArrayExtensions
	{
		public static void Add<T>(ref T[] array, T item)
		{
			if (array == null)
			{
				array = new T[] { item };
				return;
			}

			Array.Resize(ref array, array.Length + 1);
			array[array.Length - 1] = item;
		}

		public static void RemoveAt<T>(ref T[] array, int index)
		{
			if (index < 0 || index >= array.Length)
				throw new ArgumentOutOfRangeException($"The passed index {index} is out of bounds of the array with length {array.Length}");

			var newArray = new T[array.Length - 1];

			int j = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (i == index)
					continue;

				newArray[j] = array[i];
				j++;
			}

			array = newArray;
		}

		public static bool Remove<T>(ref T[] array, T item)
		{
			int index = Array.IndexOf(array, item);

			if (index == -1)
				return false;

			RemoveAt(ref array, index);
			return true;
		}
	}

	/// <summary>
	/// Generic EqualityComparer for an array of <typeparamref name="T"/> that allows for quick equality check of arrays
	/// and enables using them as keys in collections (provides GetHashCode method).
	/// Note that elements of the array must correctly pass equality check for this (override <see cref="object.Equals"/>).
	/// </summary>
	/// <typeparam name="T">The type of array elements.</typeparam>
	public class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
	{
		private readonly EqualityComparer<T> _elementComparer;

		/// <summary>
		/// Initializes a new instance of the <see cref="ArrayEqualityComparer{T}"/> class.
		/// </summary>
		/// <param name="customElementComparer">
		/// Custom element comparer. Uses <see cref="EqualityComparer&lt;T>.Default"/> by default.
		/// </param>
		public ArrayEqualityComparer(EqualityComparer<T> customElementComparer = null)
		{
			_elementComparer = customElementComparer ?? EqualityComparer<T>.Default;
		}

		public bool Equals(T[] first, T[] second)
		{
			if (first == second)
				return true;

			if (first == null || second == null)
				return false;

			if (first.Length != second.Length)
				return false;

			for (int i = 0; i < first.Length; i++)
			{
				if (!_elementComparer.Equals(first[i], second[i]))
				{
					return false;
				}
			}

			return true;
		}

		public int GetHashCode(T[] array)
		{
			unchecked
			{
				int hash = 19;

				foreach (T element in array)
				{
					hash = hash * 37 + _elementComparer.GetHashCode(element);
				}

				return hash;
			}
		}
	}
}
