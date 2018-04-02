using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    Int32 currentItemCount;

    public Int32 Count
    {
        get { return currentItemCount; }
    }

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        Int32 itemAind = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAind;
    }

    void SortUp(T item)
    {
        Int32 parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
                break;
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void SortDown(T item)
    {
        Int32 childIndexL = item.HeapIndex * 2 + 1;
        Int32 childIndexR = item.HeapIndex * 2 + 2;
        Int32 swapIndex = 0;
        while (true)
        {
            childIndexL = item.HeapIndex * 2 + 1;
            childIndexR = item.HeapIndex * 2 + 2;
            if (childIndexL < currentItemCount)
            {
                swapIndex = childIndexL;
                if (childIndexR < currentItemCount)
                {
                    if (items[childIndexL].CompareTo(items[childIndexR]) < 0)
                        swapIndex = childIndexR;
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                    Swap(item, items[swapIndex]);
                else
                    return;
            }
            else
                return;
        }
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    Int32 HeapIndex { get; set; }
}
