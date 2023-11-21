using UnityEngine;

public class ChangeArmor : MonoBehaviour
{
    [SerializeField] GameObject curGameObject;
    public void changeArmor(string name)
    {
        if (curGameObject == null || curGameObject.CompareTag(name)) return;
        GameObject NewObj;
        NewObj = transform.Find(name).gameObject;
        NewObj.SetActive(true);
        curGameObject.SetActive(false);
        curGameObject = NewObj;
    }
}
