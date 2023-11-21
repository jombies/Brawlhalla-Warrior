using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] GameObject curGameObject;
    public void changeWeapon(string name)
    {
        if (curGameObject == null || curGameObject.CompareTag(name)) return;
        GameObject NewObj;
        NewObj = transform.Find(name).gameObject;
        NewObj.SetActive(true);
        curGameObject.SetActive(false);
        curGameObject = NewObj;
    }
}
