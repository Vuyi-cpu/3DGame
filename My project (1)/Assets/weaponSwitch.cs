using UnityEngine;

public class weaponswitch : MonoBehaviour
{
    public int selectedweapon = 0;
    InteractableObject weapon;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      //  if (weapon.scytheEquipped == true && weapon.swordEquipped == true)
       // {
            int previousselectedweapon = selectedweapon;
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (selectedweapon >= transform.childCount - 1)
                    selectedweapon = 0;
                else
                    selectedweapon++;
            }

            if (previousselectedweapon != selectedweapon)
            {
                selectWeapon();
            }
       // }
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
}
