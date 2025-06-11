using UnityEngine;

public class BossVFXHandler : MonoBehaviour
{
    public GameObject skillChargeVFX;
    public GameObject recoveryVFX;

    GameObject vfx;

    public void PlaySkillCharge()
    {
        if (skillChargeVFX != null)
            SpawnVFX(skillChargeVFX);
    }

    public void PlayRecovery()
    {
        if (recoveryVFX != null) {
            vfx = ObjectPoolManager.Instance.Spawn(recoveryVFX, transform.position, Quaternion.identity);
            vfx.GetComponent<PooledObject>().SetAutoReturnTime(1.5f);
            vfx.transform.SetParent(transform);
            vfx.transform.localPosition = Vector3.zero;
            vfx.transform.localRotation = Quaternion.identity;
        }

    }

    private void SpawnVFX(GameObject vfxPrefab)
    {
        vfx = ObjectPoolManager.Instance.Spawn(skillChargeVFX, transform.position, Quaternion.identity);
        vfx.GetComponent<PooledObject>().SetAutoReturnTime(1.5f);
        vfx.transform.SetParent(transform);
        vfx.transform.localPosition = Vector3.zero;
        vfx.transform.localRotation = Quaternion.identity;
    }
}
