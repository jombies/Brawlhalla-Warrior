
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string Name = "new Item";
    public Sprite icon = null;
    public bool DefaultItem = false;
}
