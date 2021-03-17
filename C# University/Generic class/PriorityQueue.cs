using System;
using System.Collections;
using System.Collections.Generic;

namespace lab09_a
{
  /*  public interface IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            while (true)
            {
                yield return i;
            }
        }
    }*/

    public interface IPriorityQueue<T>
    {
        void Put(T item);
        T Get();
        int Count { get; }
        T Peek { get; }
    }


    // Tutaj należy umieścić cały kod z laboratorium
    public class MinPriorityQueue<T> : IPriorityQueue<T>, IEnumerable<T> where T : notnull
    {
        
        Node head;
        int count;
        public class Node
        {
            public Node(T e, Node n)
            {
                elem = e;
                next = n;
            }
            public T elem { get; set; }
            public Node next { get; set; }
        }
        public void Put(T item)
        {
            if (head == null)
            {
                head = new Node(item, null);
                count = 1;
                return;
            }
            Node pnew = new Node(item, head);
            head = pnew;
            count++;
        }
        public int Count
        {
            get
            {
                return count;
            }
        }
        public T Peek
        {
            get
            {
                if (count < 1) { throw new InvalidOperationException(); }
                Node p = head;
                T temp = head.elem;
                while (p != null)
                {
                    if (Comparer<T>.Default.Compare(p.elem, temp) < 0) { temp = p.elem; }
                    p = p.next;
                }
                return temp;
            }
        }

        public T Get()
        {
            if(count < 1) { throw new InvalidOperationException(); }
            Node p = head;
            Node prev = null;
            Node prevT = null;
            T temp = head.elem;
            while (p != null)
            {
                if (Comparer<T>.Default.Compare(p.elem, temp) < 0) 
                { 
                    temp = p.elem;
                    prevT = prev;
                }
                prev = p;
                p = p.next;
            }
            if(prevT != null) 
            {
                prevT.next = prevT.next.next;
            }
            else
            {
                head = head.next;
            }
            if(count > 0) { count--; }
            return temp;
        }


        public IEnumerator<T> GetEnumerator()
        {
           for(Node item = head; item != null; item = item.next)
           {
                yield return item.elem;
           }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }

    static class PriorityQueueExtensions
    {
        public static bool Exist<T>(this MinPriorityQueue<T> queue, T item)
        {
            foreach (var elem in queue)
            {
                if (item.Equals(elem)) { return true; }
            }

            return false;
        }

        public static T MaxItem<T>(this MinPriorityQueue<T> queue)
        {
            T max = queue.Peek;
            foreach (var elem in queue)
            {
                if (Comparer<T>.Default.Compare(elem, max) > 0) { max = elem; }
            }

            return max;
        }
    }

}
