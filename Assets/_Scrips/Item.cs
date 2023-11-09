using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string Name = "new Item";
    public Sprite icon = null;
    public bool DefaultItem = false;

    public virtual void Use()
    {
        // use item
        // someting happen

        Debug.Log("Used: " + name);
    }
    public void RemoveItem()
    {
        Inventory.Instance.Remove(this);
    }
}
