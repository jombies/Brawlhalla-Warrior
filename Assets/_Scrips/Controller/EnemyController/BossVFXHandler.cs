using UnityEngine;

public class BossVFXHandler : MonoBehaviour
{
    public GameObject skillChargeVFX;
    public GameObject recoveryVFX;
    public GameObject enrageVFX;
    public GameObject deathVFX;

    GameObject vfx;

    public void PlaySkillCharge()
    {
        if (skillChargeVFX != null)
            SpawnVFX(skillChargeVFX);
    }

    public void PlayRecovery()
    {
        if (recoveryVFX != null) {
            vfx = ObjectPoolManager.Instance.InstantiateFromPool(recoveryVFX, transform.position, Quaternion.identity);
            vfx.GetComponent<PooledObject>().AutoReturnTime = 1f;
            vfx.transform.SetParent(transform);
            vfx.transform.localPosition = Vector3.zero;
            vfx.transform.localRotation = Quaternion.identity;
        }

    }

    public void PlayEnrage()
    {
        if (enrageVFX != null)
            SpawnVFX(enrageVFX);
    }

    public void PlayDeath()
    {
        if (deathVFX != null)
            SpawnVFX(deathVFX);
    }

    private void SpawnVFX(GameObject vfxPrefab)
    {
        vfx = ObjectPoolManager.Instance.InstantiateFromPool(vfxPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
        vfx.GetComponent<PooledObject>().AutoReturnTime = 2f; // Set auto return time for the VFX
        vfx.transform.SetParent(transform);
        vfx.transform.localPosition = Vector3.zero;
        vfx.transform.localRotation = Quaternion.identity;
    }
}
