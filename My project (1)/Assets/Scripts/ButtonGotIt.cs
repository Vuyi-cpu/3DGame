using UnityEngine;

public class ButtonGotIt : MonoBehaviour
{
    public GameObject tutorial;
    public void gotIt()
    {
        tutorial.SetActive(false);
    }
}
