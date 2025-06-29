using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StatEquip
{
    public int BaseValue;

    private List<int> _equipment = new List<int>();

    public int Value => CalculateFinalValue();

    public void AddEquip(int value)
    {
        if (value != 0) {
            _equipment.Add(value);
        }
        //TotalValue();
    }
    public void RemoveEquip(int value)
    {
        if (value != 0) {
            _equipment.Remove(value);
        }
        //totalValue();
    }
    public void TotalValue()
    {
        int newValue = 0;
        foreach (int modifier in _equipment) {
            newValue = modifier;
        }
        BaseValue = +newValue;
    }
    private int CalculateFinalValue()
    {
        int finalValue = BaseValue;
        foreach (int mod in _equipment) {
            finalValue += mod;
        }
        return finalValue;
    }
}
