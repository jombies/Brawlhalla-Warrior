using System.Collections;
using UnityEngine;

public class EnemyHitFlasht : MonoBehaviour
{
    public Renderer enemyRenderer;
    public Color flashColor = Color.white;
    public float flashDuration = 0.1f;

    private Material enemyMaterial;
    private Color originalColor;

    private void Awake()
    {
        if (enemyRenderer == null)
            enemyRenderer = GetComponentInChildren<Renderer>();


        enemyMaterial = enemyRenderer.material;
        if (originalColor == null)
            Debug.LogWarning("EnemyHitFlasht: Renderer is missing on " + gameObject.name);
        originalColor = enemyMaterial.color;
    }
    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(DoFlash());
    }

    IEnumerator DoFlash()
    {
        enemyMaterial.color = flashColor * 5;
        yield return new WaitForSeconds(flashDuration);
        enemyMaterial.color = originalColor;
    }
}
