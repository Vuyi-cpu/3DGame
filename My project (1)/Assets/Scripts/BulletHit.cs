using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public GameObject bullet;
    void OnCollisionEnter(Collision collision)
    {
        bullet.SetActive(false);
    }
}
