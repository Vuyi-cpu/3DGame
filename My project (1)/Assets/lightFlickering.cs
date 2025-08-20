using System.Xml.Serialization;
using UnityEngine;

public class lightFlickering : MonoBehaviour
{
   public Light Light;

    public float minIntensity = 0.5f;
    public float maxIntensity = 5f;
    public float flickerSpeed = 0.5f;

    private void Start()
    {
        Light = GetComponent<Light>();
        InvokeRepeating("Flicker", 0f, flickerSpeed);

    }

    private void Flicker()
    {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        Light.intensity = randomIntensity;
    }

}
