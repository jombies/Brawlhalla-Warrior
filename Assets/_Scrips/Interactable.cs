using UnityEngine;


public class Interactable : MonoBehaviour
{
    float radius = 2.0f;

    bool isFoscus = false;
    Transform Player;

    private void Update()
    {
        if (isFoscus)
        {
            float distance = Vector3.Distance(Player.transform.position, transform.position);
            if (distance <= radius)
            {
                Debug.Log("da tuong tac");
            }
        }
    }

    public void OnFusused(Transform playerTransform)
    {
        isFoscus = true;
        Player = playerTransform;
    }
    public void Ondesfocused(Transform playerTransform)
    {
        isFoscus = false;
        Player = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
