using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] GameObject _curGameObject;
    public void changeWeapon(string name)
    {
        if (name == string.Empty && _curGameObject.CompareTag(name)) return;
        GameObject newObj = transform.Find(name).gameObject;
        newObj.SetActive(true);
        _curGameObject.SetActive(false);
        _curGameObject = newObj;
    }
}
