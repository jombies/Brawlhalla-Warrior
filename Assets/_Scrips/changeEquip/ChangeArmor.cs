using UnityEngine;

public class ChangeArmor : MonoBehaviour
{
    [SerializeField] GameObject _curGameObject;

    public void changeArmor(string name)
    {
        if (string.IsNullOrEmpty(name) || _curGameObject == null)
            return;
        if (_curGameObject.name == name) return;

        GameObject newObj = transform.Find(name).gameObject;

        if (newObj == null) {
            Debug.LogWarning($"Không tìm thấy armor: {name}");
            return;
        }

        newObj.SetActive(true);
        if (_curGameObject != null)
            _curGameObject.SetActive(false);

        _curGameObject = newObj;
    }
}
