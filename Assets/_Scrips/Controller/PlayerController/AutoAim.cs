using UnityEngine;

public class AutoAim : MonoBehaviour
{
    public Transform target;
    public float range = 10f;

    void Update()
    {
        FindClosestEnemy();
        AimAndShoot();
    }

    void FindClosestEnemy()
    {
        // Find all enemies within range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("BossEnemy"))
            {
                float distance = (hitCollider.transform.position - transform.position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.transform;
                }
            }
        }

        target = closestEnemy;
    }

    void AimAndShoot()
    {
        if (target != null)
        {
            // Aim at the target
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0f;
            transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 5);
        }
    }
}
