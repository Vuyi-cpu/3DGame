using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public AudioSource audio;
    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Environment") || other.CompareTag("door")) Destroy(gameObject);
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
          audio.Play();

        }

    }

 
}
