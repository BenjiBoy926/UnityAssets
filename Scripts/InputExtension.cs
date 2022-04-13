using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputExtension
{
    public static bool GetAnyKeys(params KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
            if (Input.GetKey(key))
                return true;
        return false;
    }
    public static bool GetAnyKeysDown(params KeyCode[] keys)
    {
        // Array where the bool at that position is true if that key just went down
        // and none of the other keys were being held down
        bool[] keyDownAndOthersNotHeld = new bool[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            keyDownAndOthersNotHeld[i] = Input.GetKeyDown(keys[i]);

            for (int j = 0; j < keys.Length; j++)
            {
                if (i != j)
                {
                    keyDownAndOthersNotHeld[i] &= !Input.GetKey(keys[j]);
                }
            }
        }

        // All keys went up if any element in the array is true
        bool result = false;
        for (int i = 0; i < keyDownAndOthersNotHeld.Length; i++)
        {
            result |= keyDownAndOthersNotHeld[i];
        }
        return result;
    }
    public static bool GetAllKeysUp(params KeyCode[] keys)
    {
        // Array where the bool at that position is true if that key just went up
        // and none of the other keys were being held down
        bool[] keyUpAndOthersNotHeld = new bool[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            keyUpAndOthersNotHeld[i] = Input.GetKeyUp(keys[i]);

            for (int j = 0; j < keys.Length; j++)
            {
                if (i != j)
                {
                    keyUpAndOthersNotHeld[i] &= !Input.GetKey(keys[j]);
                }
            }
        }

        // All keys went up if any element in the array is true
        bool result = false;
        for (int i = 0; i < keyUpAndOthersNotHeld.Length; i++)
        {
            result |= keyUpAndOthersNotHeld[i];
        }
        return result;
    }
}
