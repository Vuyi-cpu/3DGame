using UnityEngine;

public class BulletHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);

        }
        else if (!other.CompareTag("Shooter"))
        {
            Destroy(gameObject);
        }

    }

 
}
