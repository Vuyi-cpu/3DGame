using UnityEngine;

public class BulletHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Environment") || other.CompareTag("door")) Destroy(gameObject);
    }
}
