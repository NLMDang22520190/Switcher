using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly Queue<T> objects = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;
    private int initialSize;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.initialSize = initialSize;
        for (int i = 0; i < initialSize; i++)
        {
            AddObject();
        }
    }

    private void AddObject()
    {
        var newObject = Object.Instantiate(prefab, parent);
        newObject.gameObject.SetActive(false);
        objects.Enqueue(newObject);
    }

    public T Get()
    {
        if (objects.Count == 0)
        {
            // Add additional objects if the pool is empty
            for (int i = 0; i < initialSize; i++)
            {
                AddObject();
            }
        }

        var obj = objects.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }
}
