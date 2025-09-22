using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageHit : MonoBehaviour
{
    public float hitIntensity = 0.6f;  
    private Volume volume;
    private Vignette vignette;

    private Coroutine runningEffect;

    void Start()
    {
        volume = GetComponent<Volume>();

        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("No Vignette found in profile!");
            return;
        }

        vignette.active = false;
    }

 

    public void TriggerDamageEffect()
    {
        if (runningEffect != null)
            StopCoroutine(runningEffect);

        runningEffect = StartCoroutine(damageEffect());
    }

    private IEnumerator damageEffect()
    {
        vignette.active = true;


        float intensity = hitIntensity;
        vignette.intensity.value = intensity;

        while (intensity > 0f)
        {
            intensity -= 0.01f;
            if (intensity < 0f) intensity = 0f;

            vignette.intensity.value = intensity;
            yield return new WaitForSeconds(0.1f);
        }

        vignette.active = false;
        runningEffect = null;
    }
}
