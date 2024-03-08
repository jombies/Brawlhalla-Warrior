using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public abstract class Item : ScriptableObject
{
    public string Name = "new Item";
    public Sprite Icon = null;
    public bool DefaultItem = false;

    public abstract void Use();

    public void RemoveItem()
    {
        Inventory.Instance.Remove(this);
    }
}

