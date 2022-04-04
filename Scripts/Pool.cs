using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T>
{
    #region Private Fields
    private List<T> items = new List<T>();
    private int currentIndex = 0;

    private Func<T> generator;
    private Predicate<T> usable;
    #endregion

    #region Constructors
    public Pool(int initialItems, Func<T> generator, Predicate<T> usable)
    {
        this.generator = generator;
        this.usable = usable;

        // Use the generator to generat the initial items
        for (int i = 0; i < initialItems; i++)
        {
            items.Add(generator.Invoke());
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

        // Apparently unity's null check is different
        // so we have to do a specific check for unity objects
        if (current is UnityEngine.Object)
        {
            UnityEngine.Object obj = current as UnityEngine.Object;

            if (obj == null)
                current = generator.Invoke();
        }

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
