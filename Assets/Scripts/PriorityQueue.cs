using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<TElement> where TElement : class
{
    private struct QueueObject
    {
        public TElement element { get; private set; }
        public float priority { get; private set; }
        public QueueObject(TElement element, float priority)
        {
            this.element = element;
            this.priority = priority;
        }
    }
    private List<QueueObject> _pq = new List<QueueObject>();
    public bool IsEmpty() => (_pq.Count == 0);
    public TElement Top() => IsEmpty() ? default(TElement) : _pq[0].element;
    public TElement Pop()
    {
        if (IsEmpty()) { return default(TElement); }
        TElement obj = Top();
        _pq.RemoveAt(0);
        return obj;
    }
    public float BestPriority() => IsEmpty() ? float.MaxValue : _pq[0].priority;
    public void Insert(TElement obj, float prio) // Currently O(n) insert, could be made a binary insert to get O(log n)
    {
        int i = 0;
        while(i < _pq.Count && _pq[i].priority < prio) { i++; }
        _pq.Insert(i, new QueueObject(obj, prio));
    }

    public bool Remove(TElement obj)
    {
        for (int i = 0; i < _pq.Count; i++)
        {
            if (_pq[i].element == obj)
            {
                _pq.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public bool HasBetterPriority(PriorityQueue<TElement> otherQ)
    {
        return BestPriority() <= otherQ.BestPriority();
    }
}
