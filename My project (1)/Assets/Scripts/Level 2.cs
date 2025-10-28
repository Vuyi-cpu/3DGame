using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Level2 : MonoBehaviour
{
    [Header("UI")]
    public GameObject level;
    public GameObject requirement;
    public Shop shop; 

    private bool hasPaused = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (shop.neuronCount >= 50)
            {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentSceneIndex + 1);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                
                StartCoroutine(ShowRequirementCoroutine());
            }
        }
    }

    private IEnumerator ShowRequirementCoroutine()
    {
        requirement.SetActive(true);
        yield return new WaitForSeconds(2f); 
        requirement.SetActive(false);
    }
}


