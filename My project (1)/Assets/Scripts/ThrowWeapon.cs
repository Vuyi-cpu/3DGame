using UnityEngine;
using UnityEngine.UIElements;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] GameObject scythe; //Game object reference to scythe. Used to move scythe
    [SerializeField] Transform scytheLocation; //Original scythe location
    //[SerializeField] Transform scytheRotation; //Original scythe rotation - commented cuz idk original rotation rn
    [SerializeField] float scytheDistance; //How far can we throw this thing
    [SerializeField] float throwSpeed; //Throw speed of this thing
    [SerializeField] LayerMask layerMask; //Layer mask for raycast check. Looking for environment layer

    bool isThrown; //Bool that gets set when thrown
    public bool isReturning; //Bool that get set after hitting the middle point
    public bool holdingScythe;

    [SerializeField] Vector3 throwPosition; //This is where the scythe is traveling to.
    Rotator rotator; //Rotator on scythe object. Gets turned on when thrown. And off when not.

    [SerializeField] float damage; //How much damage does this object do?
    Enemystate enemy; //Any enemy object hit if any

    private void Start()
    {
        holdingScythe = false;
        //rotator.enabled = false; - commented because it gives an error
    }

    void Update()
    {
        if (holdingScythe)
        {
            //If we press left mouse down
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //If isThrown or isReturning is true, go away, else check distance
                if (isThrown || isReturning) return;
                CheckDistance();
            }

            //If isThrown is true
            if (isThrown)
            {
                //Set new position to move towards and apply to scythe transform.
                Vector3 newPos = Vector3.MoveTowards(scythe.transform.position, throwPosition, throwSpeed * Time.deltaTime);
                scythe.transform.position = newPos;

                //Turn on the scythe's mesh collider
                scythe.GetComponent<MeshCollider>().enabled = true;

                //If the scythe's position is equal to the throw position
                if (scythe.transform.position == throwPosition)
                {
                    //If there is an enemy object
                    if (enemy != null)
                    {
                        //Damage the enemy object
                        enemy.currentHealth = enemy.currentHealth - damage;
                        enemy = null;
                    }
                    //Reset both throw variables
                    isThrown = false;
                    isReturning = true;
                }
            }

            //Is isReturning
            if (isReturning)
            {
                //Set the new position back to the scythe's original position
                Vector3 newPos = Vector3.MoveTowards(scythe.transform.position, scytheLocation.position, throwSpeed * Time.deltaTime);
                scythe.transform.position = newPos;

                //If scythe's position is equal to original scythe location
                if (scythe.transform.position == scytheLocation.position)
                {
                    //Set isReturning to false, turn off rotator, set parent and rotation
                    isReturning = false;
                    rotator.enabled = false;
                    scythe.transform.parent = scytheLocation;
                    //scythe.transform.rotation = scytheRotation.rotation;
                }
            }
        }
    }

    //Called from update, Checks distance then throws scythe
    void CheckDistance()
    {
        //Raycast hit
        RaycastHit hit;

        //If raycast hit goes from scythe location forward times scythe distance and looking for the environment layer mask
        if (Physics.Raycast(scytheLocation.transform.position, scytheLocation.transform.forward, out hit, scytheDistance, layerMask))
        {
            //If health object is not null, set that reference
            if (hit.transform.GetComponentInParent<Enemystate>() != null)
            {
                enemy = hit.transform.GetComponentInParent<Enemystate>();
            }

            //Set throw postion to hit.point, set parent to null, turn rotator on an set isthrown to true so that it starts to travel.
            throwPosition = hit.point;
            scythe.transform.parent = null;
            rotator.enabled = true;
            isThrown = true;
        }
        //If the raycast does not hit anything, cast out by scythe distance
        else
        {
            //Set throw position to scythe.forward times boomerangdistance
            throwPosition = scytheLocation.forward * scytheDistance;
            //Set parent to null, enable rotator, and set isthrowm to true so it starts to travel
            scythe.transform.parent = null;
            rotator.enabled = true;
            isThrown = true;
        }
    }
}


