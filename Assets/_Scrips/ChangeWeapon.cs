using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] GameObject curGameObject;
    public void GetnameWeapon(string o)
    {
        if (curGameObject == null || curGameObject.CompareTag(o)) return;

        GameObject NewObj;
        NewObj = transform.Find(o).gameObject;

        NewObj.SetActive(true);

        curGameObject.SetActive(false);
        curGameObject = NewObj;
    }
}
