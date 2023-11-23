using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stat
{
    public int BaseValue;

    private List<int> modifiers = new List<int>();
    public int GetValue()
    {
        return BaseValue;
    }

    public void AddModifier(int value)
    {
        if (value != 0)
        {
            modifiers.Add(value);
        }
        //totalValue();
    }
    public void RemoveModifier(int value)
    {
        if (value != 0)
        {
            modifiers.Remove(value);
        }
        //totalValue();
    }
    public void totalValue()
    {
        int newValue = 0;
        foreach (int modifier in modifiers)
        {
            newValue += modifier;
        }
        BaseValue = newValue;
    }
}
