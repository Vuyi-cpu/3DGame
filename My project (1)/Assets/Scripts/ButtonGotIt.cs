using UnityEngine;

public class ButtonGotIt : MonoBehaviour
{
    public GameObject tutorial;
    private void gotIt()
    {
        tutorial.SetActive(false);
    }
}
