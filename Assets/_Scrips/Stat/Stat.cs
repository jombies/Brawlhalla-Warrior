using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stat
{
    public int BaseValue;

    private List<int> _modifiers = new List<int>();

    public int Value => CalculateFinalValue();

    public void AddModifier(int value)
    {
        if (value != 0) {
            _modifiers.Add(value);
        }
        //TotalValue();
    }
    public void RemoveModifier(int value)
    {
        if (value != 0) {
            _modifiers.Remove(value);
        }
        //totalValue();
    }
    public void TotalValue()
    {
        int newValue = 0;
        foreach (int modifier in _modifiers) {
            newValue = modifier;
        }
        BaseValue = +newValue;
    }
    private int CalculateFinalValue()
    {
        int finalValue = BaseValue;
        foreach (int mod in _modifiers) {
            finalValue += mod;
        }
        return finalValue;
    }
}
