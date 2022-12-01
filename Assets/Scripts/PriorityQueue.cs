using System.Collections.Generic;

public class PriorityQueue<TElement> where TElement : class
{
    private struct QueueObject
    {
        public TElement Element { get; private set; }
        public float Priority { get; private set; }
        public QueueObject(TElement element, float priority)
        {
            this.Element = element;
            this.Priority = priority;
        }
    }
    private List<QueueObject> _pq = new List<QueueObject>();
    public bool IsEmpty() => (_pq.Count == 0);
    public TElement Top() => IsEmpty() ? default(TElement) : _pq[0].Element;
    public TElement Pop()
    {
        if (IsEmpty()) { return default(TElement); }
        TElement obj = Top();
        _pq.RemoveAt(0);
        return obj;
    }
    public void Insert(TElement obj, float prio)
    {
        int i = 0;
        while(i < _pq.Count && _pq[i].Priority < prio) { i++; }
        BinaryInsert(new QueueObject(obj, prio));
    }

    public bool Remove(TElement obj)
    {
        for (int i = 0; i < _pq.Count; i++)
        {
            if (_pq[i].Element == obj)
            {
                _pq.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    private void BinaryInsert(QueueObject insertObject)
    {
        int minIndex = 0;
        int maxIndex = _pq.Count - 1;
        int midIndex = 0;
        float newPrio = insertObject.Priority;

        while (minIndex <= maxIndex)
        {
            midIndex = (minIndex + maxIndex) / 2;
            if (_pq[midIndex].Priority == newPrio)
            {
                _pq.Insert(midIndex, insertObject);
                return;
            }
            if (_pq[midIndex].Priority < newPrio)
            {
                minIndex = midIndex + 1;
            }
            else
            {
                maxIndex = midIndex - 1;
            }
        }

        _pq.Insert(minIndex, insertObject);
        return;
    }

}
