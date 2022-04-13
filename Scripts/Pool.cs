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
    private Predicate<T> usable;
    private Action<T> makeUsable;
    #endregion

    #region Constructors
    public Pool(int initialItems, Func<T> generator, Predicate<T> usable, Action<T> makeUsable)
    {
        this.generator = generator;
        this.usable = usable;
        this.makeUsable = makeUsable;

        // Use the generator to generate the initial items
        for (int i = 0; i < initialItems; i++)
        {
            T item = generator.Invoke();
            makeUsable.Invoke(item);

            // Check to make sure the condition of the action is met
            if (!usable.Invoke(item))
                Debug.LogWarning($"Item '{item}' is not usable " +
                    $"after invoking 'makeUsable' on the item");

            items.Add(item);
        }

    }
    #endregion

    #region Public Methods
    public T Get()
    {
        T current = items[currentIndex];

        // If this object is null we need to generate a new one
        if (ReferenceEquals(current, null))
            current = generator.Invoke();

        // If the current item is not usable then 
        // add a new one to the pool of items
        if (!usable.Invoke(current))
        {
            current = generator.Invoke();
            items.Add(current);
            currentIndex = items.Count - 1;
        }

        // Update the index of the current item
        UpdateCurrentIndex();

        // Return the current item
        return current;
    }
    #endregion

    #region Private Methods
    private void UpdateCurrentIndex()
    {
        int start = currentIndex;
        currentIndex = (currentIndex + 1) % items.Count;

        // Continue incrementing the index until it hits an item that is usable
        // or until it loops back around to the start
        while (currentIndex != start && !usable.Invoke(items[currentIndex]))
        {
            currentIndex = (currentIndex + 1) % items.Count;
        }
    }
    #endregion
}
