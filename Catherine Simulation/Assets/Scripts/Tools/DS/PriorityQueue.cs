using System;
using System.Collections.Generic;

namespace Tools.DS
{

    public class PriorityQueue<T>
    {
        private List<T> _heap;
        private IComparer<T> _comparer;

        public int Count => _heap.Count;

        public PriorityQueue() : this(Comparer<T>.Default)
        {
        }

        public PriorityQueue(IComparer<T> comparer)
        {
            _heap = new List<T>();
            _comparer = comparer;
        }

        public void Enqueue(T item)
        {
            _heap.Add(item);
            int currentIndex = _heap.Count - 1;

            while (currentIndex > 0)
            {
                int parentIndex = (currentIndex - 1) / 2;

                if (_comparer.Compare(_heap[currentIndex], _heap[parentIndex]) >= 0)
                    break;

                Swap(currentIndex, parentIndex);
                currentIndex = parentIndex;
            }
        }

        public T Dequeue()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Priority queue is empty.");

            T firstItem = _heap[0];
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            lastIndex--;

            int currentIndex = 0;

            while (true)
            {
                int leftChildIndex = currentIndex * 2 + 1;
                int rightChildIndex = currentIndex * 2 + 2;

                if (leftChildIndex > lastIndex)
                    break;

                int smallerChildIndex = leftChildIndex;

                if (rightChildIndex <= lastIndex && _comparer.Compare(_heap[rightChildIndex], _heap[leftChildIndex]) < 0)
                    smallerChildIndex = rightChildIndex;

                if (_comparer.Compare(_heap[currentIndex], _heap[smallerChildIndex]) <= 0)
                    break;

                Swap(currentIndex, smallerChildIndex);
                currentIndex = smallerChildIndex;
            }

            return firstItem;
        }

        public T Peek()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Priority queue is empty.");

            return _heap[0];
        }

        private void Swap(int indexA, int indexB)
        {
            (_heap[indexA], _heap[indexB]) = (_heap[indexB], _heap[indexA]);
        }
    }
}