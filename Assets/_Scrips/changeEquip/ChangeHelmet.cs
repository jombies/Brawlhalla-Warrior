using UnityEngine;
public class ChangeHelmet : MonoBehaviour
{
    [SerializeField] GameObject _curGameObject;
    public void changeHelmet(string name)
    {
        if (name == string.Empty && _curGameObject.CompareTag(name)) return;
        GameObject newObj = transform.Find(name).gameObject;
        newObj.SetActive(true);
        _curGameObject.SetActive(false);
        _curGameObject = newObj;
    }
}
