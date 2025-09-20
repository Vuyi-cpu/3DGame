using UnityEngine;

public class RotatorSwing : MonoBehaviour
{
    public GameObject sword;
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

        startRotation = sword.transform.localEulerAngles.x;
        targetRotation = startRotation - swingAngle;
    }

    void HandleSwing()
    {
        float step = swingSpeed * Time.deltaTime;

        if (swingingForward)
        {
            // Rotate towards target
            sword.transform.localEulerAngles = new Vector3(
                Mathf.MoveTowardsAngle(-sword.transform.localEulerAngles.x, targetRotation, step),
                sword.transform.localEulerAngles.y,
                sword.transform.localEulerAngles.z
            );

            if (Mathf.Approximately(sword.transform.localEulerAngles.x, targetRotation))
            {
                // Reached max swing start returning
                swingingForward = false;
                targetRotation = startRotation;
            }
        }
        else
        {
            // Rotate back to start
            sword.transform.localEulerAngles = new Vector3(
                Mathf.MoveTowardsAngle(-sword.transform.localEulerAngles.x, targetRotation, step),
                sword.transform.localEulerAngles.y,
                sword.transform.localEulerAngles.z
            );

            if (sword.transform.localEulerAngles.x == targetRotation)
            {
                // Swing complete
                isSwinging = false;
            }
        }
    }
}
