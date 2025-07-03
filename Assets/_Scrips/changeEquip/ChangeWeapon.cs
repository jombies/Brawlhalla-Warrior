using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] GameObject _curGameObject;
    public void changeWeapon(string name)
    {
        if (string.IsNullOrEmpty(name) || _curGameObject == null)
            return;
        if (_curGameObject.name == name) return;

        GameObject newObj = transform.Find(name).gameObject;

        if (newObj == null) {
            Debug.LogWarning($"Không tìm thấy weapons: {name}");
            return;
        }

        newObj.SetActive(true);
        if (_curGameObject != null)
            _curGameObject.SetActive(false);

        _curGameObject = newObj;
    }
}
