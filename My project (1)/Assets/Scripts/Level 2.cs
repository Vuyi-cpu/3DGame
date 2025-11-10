using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.ComponentModel;

public class Level2 : MonoBehaviour
{
    [Header("UI")]
    public GameObject level;
    public GameObject requirement;
    public Shop shop;
    public GameObject done;
    public bool BossDead;

    private bool hasPaused = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (shop.EnemiesKilled/50 >= 0 && BossDead)
            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
               
            }
            else
            {
                 return;
            }
        }
    }

}


