using UnityEngine;

public class ChangeHelmet : MonoBehaviour
{
    [SerializeField] GameObject curGameObject;
    public void changeHelmet(string name)
    {
        if (name == null || curGameObject.CompareTag(name)) return;
        GameObject newObj;
        newObj = transform.Find(name).gameObject;
        newObj.SetActive(true);
        curGameObject.SetActive(false);
        curGameObject = newObj;
    }
}
