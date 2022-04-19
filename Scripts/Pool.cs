using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : class
{
    #region Private Fields
    private List<T> items = new List<T>();
    private int currentIndex = 0;

    private Func<T> generator;
    private Predicate<T> isUsable;
    private Action<T> makeUsable;
    #endregion

    #region Constructors
    public Pool(int initialItems, Func<T> generator, Predicate<T> isUsable, Action<T> makeUsable)
    {
        this.generator = generator;
        this.isUsable = isUsable;
        this.makeUsable = makeUsable;

        // Use the generator to generate the initial items
        for (int i = 0; i < initialItems; i++)
        {
            GenerateItem();
        }

    }
    #endregion

    #region Public Methods
    public T Get()
    {
        T current = items[currentIndex];

        // If this object is null we need to generate a new one
        if (current is null)
            current = generator.Invoke();

        // If the current item is not usable then 
        // add a new one to the pool of items
        if (!isUsable.Invoke(current))
        {
            current = GenerateItem();
            currentIndex = items.Count - 1;
        }

        // Update the index of the current item
        UpdateCurrentIndex();

        // Return the current item
        return current;
    }
    #endregion

    #region Private Methods
    private T GenerateItem()
    {
        T item = generator.Invoke();
        makeUsable.Invoke(item);
        items.Add(item);

        // Check to make sure the condition of the action is met
        if (!isUsable.Invoke(item))
            Debug.LogWarning($"Item '{item}' is not usable " +
                $"after invoking 'makeUsable' on it");

        return item;
    }
    private void UpdateCurrentIndex()
    {
        int start = currentIndex;
        currentIndex = (currentIndex + 1) % items.Count;

        // Continue incrementing the index until it hits an item that is usable
        // or until it loops back around to the start
        while (currentIndex != start && !isUsable.Invoke(items[currentIndex]))
        {
            currentIndex = (currentIndex + 1) % items.Count;
        }
    }
    #endregion
}
