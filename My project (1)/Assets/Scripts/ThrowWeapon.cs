using UnityEngine;
using UnityEngine.UIElements;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] GameObject scythe; //Game object reference to scythe. Used to move scythe
    [SerializeField] Rigidbody scytheRb;
    [SerializeField] Transform scytheLocation; //Original scythe location
    //Transform scytheRotation; //Original scythe rotation - commented cuz idk original rotation rn
    [SerializeField] float scytheDistance; //How far can we throw this thing
    [SerializeField] float throwSpeed; //Throw speed of this thing
    [SerializeField] LayerMask layerMask; //Layer mask for raycast check. Looking for environment layer
    [SerializeField] private Transform player;

    public bool isThrown; //Bool that gets set when thrown
    public bool isReturning; //Bool that get set after hitting the middle point

    [SerializeField] Vector3 throwPosition; //This is where the scythe is traveling to.
    [SerializeField] Rotator rotator; //Rotator on scythe object. Gets turned on when thrown. And off when not.

    [SerializeField] float damage; //How much damage does this object do?
    Enemystate enemy; //Any enemy object hit if any
    private float currentReturnSpeed;
    private float returnAcceleration;

    private void Awake()
    {
        rotator.enabled = false;
    }

    void Update()
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
                scytheRb.isKinematic = false;
                scytheRb.useGravity = false;
                //Set new position to move towards and apply to scythe transform.
                Vector3 newPos = Vector3.MoveTowards(scythe.transform.position, throwPosition, throwSpeed * Time.deltaTime);
                scythe.transform.position = newPos;

                //If the scythe's position is equal to the throw position
                if (scythe.transform.position == throwPosition)
                {
                    //If there is an enemy object
                    /*if (enemy != null)
                    {
                        //Damage the enemy object
                        enemy.currentHealth = enemy.currentHealth - damage;
                        enemy = null;
                    }*/
                    //Reset both throw variables
                    isThrown = false;
                    isReturning = true;
                }
            }

            //Is isReturning
            if (isReturning)
            {
                scytheRb.isKinematic = true;
                currentReturnSpeed = throwSpeed+2;
                //currentReturnSpeed += (float)(throwSpeed + 0.5 * Time.deltaTime);
                //Set the new position back to the scythe's original position
                Vector3 newPos = Vector3.MoveTowards(scythe.transform.position, scytheLocation.position, currentReturnSpeed * Time.deltaTime);
                scythe.transform.position = newPos;

                //If scythe's position is equal to original scythe location
                if (scythe.transform.position == scytheLocation.position)
                {
                    //Set isReturning to false, turn off rotator, set parent and rotation
                    isReturning = false;
                    rotator.enabled = false;
                    scythe.transform.parent = scytheLocation;
                    scythe.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
    }

    //Called from update, Checks distance then throws scythe
    void CheckDistance()
    {
        
        {
            scytheLocation.localEulerAngles = new Vector3(0f, 0f, 0f);
            throwPosition = player.transform.position + scytheLocation.forward * scytheDistance;
            //Set parent to null, enable rotator, and set isthrown to true so it starts to travel
            scythe.transform.parent = null;
            rotator.enabled = true;
            isThrown = true;
            scytheLocation.localEulerAngles = new Vector3(63f, -195.3f, -267.07f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrown)
        {
            //Set throw postion to hit.point, set parent to null, turn rotator on an set isthrown to true so that it starts to travel.
            isThrown = false;
            isReturning = true;
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemystate enemy = collision.gameObject.GetComponentInParent<Enemystate>();
                if (enemy != null)
                {
                    enemy.currentHealth -= damage;
                    if (enemy.currentHealth <= 0)
                    {
                        Destroy(enemy.enemy);
                    }
                }
            }
        }
    }
}


