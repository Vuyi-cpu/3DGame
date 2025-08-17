using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public void DoFov(float endValue)
    {
        GetComponent<Camera>().fieldOfView = endValue;
    }
}
