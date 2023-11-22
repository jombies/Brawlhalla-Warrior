using UnityEngine;

public class ChangeArmor : MonoBehaviour
{
    [SerializeField] GameObject curGameObject;
    public void changeArmor(string name)
    {
        if (name == string.Empty || curGameObject.CompareTag(name)) return;
        GameObject NewObj = transform.Find(name).gameObject;
        NewObj.SetActive(true);
        curGameObject.SetActive(false);
        curGameObject = NewObj;
    }
}
