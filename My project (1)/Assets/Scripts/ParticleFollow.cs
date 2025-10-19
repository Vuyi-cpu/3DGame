using UnityEngine;

public class ParticleFollow : MonoBehaviour
{
    public Transform followTarget;

    void LateUpdate()
    {
        transform.position = followTarget.position;

        transform.rotation = Quaternion.identity;
    }

}
