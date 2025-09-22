using UnityEditor.ShaderGraph;
using UnityEngine;

public class weaponswitch : MonoBehaviour
{
    public int selectedweapon = 0;
    InteractableObject weapon;
    PlayerControls controls;

    void Start()
    {

    }

    private void Awake()
    {
        weapon = FindFirstObjectByType<InteractableObject>();
        controls = new PlayerControls();
        controls.Player.Switch.performed += ctx =>
        {
              if (weapon.scytheEquipped == true || weapon.swordEquipped == true)
             {
            int previousselectedweapon = selectedweapon;
            
                if (selectedweapon >= transform.childCount - 1)
                    selectedweapon = 0;
                else
                    selectedweapon++;
            

            if (previousselectedweapon != selectedweapon)
            {
                selectWeapon();
            }
             }


        };
    }


    // Update is called once per frame
    void Update()
    {
   
    }

    void selectWeapon()
    {

        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedweapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
