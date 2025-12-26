using UnityEngine;
using System.Collections;

public class FadeAndDestroy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float delayBeforeDestroy = 0f;

    IEnumerator Start()
    {
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (!spriteRenderer)
            {
                Destroy(gameObject);
                yield break;
            }
        }

        Color startColor = spriteRenderer.color;
        
        yield return null;
        
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(startColor.a, 0f, progress);
            spriteRenderer.color = newColor;
            
            yield return null;
        }
        
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        if (delayBeforeDestroy > 0)
            yield return new WaitForSeconds(delayBeforeDestroy);
        
        Destroy(gameObject);
    }
}