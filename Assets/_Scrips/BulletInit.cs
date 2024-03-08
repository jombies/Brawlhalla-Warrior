using UnityEngine;

public class BulletInit : MonoBehaviour
{
    int damage;
    public int Damage => damage;

    public void InitDamage(int Dmg)
    {
        damage = Dmg;
    }
}
