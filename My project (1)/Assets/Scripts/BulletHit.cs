using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public AudioSource audio;
    private void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
          audio.Play();

        }
        else if (!other.CompareTag("Shooter"))
        {
            Destroy(gameObject);
        }

    }

 
}
