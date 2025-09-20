using UnityEngine;

public class RotatorSwing : MonoBehaviour
{
    public Transform sword;
    public float swingAngle = 90f;      // how far it swings
    public float swingSpeed = 360f;     // degrees per second
    public bool isSwinging = false;


    private bool swingingForward = true;
    private float startRotation;
    private float targetRotation;

    void Update()
    {

        if (isSwinging)
        {
            HandleSwing();
        }
    }

    public void StartSwing()
    {
        isSwinging = true;
        swingingForward = true;

        startRotation = sword.localEulerAngles.x;
        targetRotation = startRotation - swingAngle;
    }

    void HandleSwing()
    {
        float step = swingSpeed * Time.deltaTime;

        if (swingingForward)
        {
            // Rotate towards target
            sword.localEulerAngles = new Vector3(
                Mathf.MoveTowardsAngle(-sword.localEulerAngles.x, targetRotation, step),
                sword.localEulerAngles.y,
                sword.localEulerAngles.z
            );

            if (Mathf.Approximately(sword.localEulerAngles.x, targetRotation))
            {
                // Reached max swing start returning
                swingingForward = false;
                targetRotation = startRotation;
            }
        }
        else
        {
            // Rotate back to start
            sword.localEulerAngles = new Vector3(
                Mathf.MoveTowardsAngle(-sword.localEulerAngles.x, targetRotation, step),
                sword.localEulerAngles.y,
                sword.localEulerAngles.z
            );

            if (sword.localEulerAngles.x == targetRotation)
            {
                // Swing complete
                isSwinging = false;
            }
        }
    }
}
