using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class DoorRotate : MonoBehaviour
{

    public float open = 90f;
    public float speed = 2f;
    public int mult;
    bool isopen = false;
    private Quaternion rotationshut;
    private Quaternion rotationopen;
    private Coroutine coroutine;
    public GameObject interaction_Info_UI;
    TextMeshProUGUI interaction_text;
    public Transform handle1;
    public Transform handle2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotationshut = transform.rotation;
        rotationopen = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -open + mult * 35f, 0));
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.distance < 5)
        {
           if (hit.transform == handle1 || hit.transform == handle2)
            {
                
                    interaction_text.text = "[E] to interact.";
                    interaction_Info_UI.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (coroutine != null) StopCoroutine(coroutine);
                        coroutine = StartCoroutine(MoveDoor());
                    }
                
            }


        }
        else { interaction_Info_UI.SetActive(false); }
    }

    private IEnumerator MoveDoor()
    {
        Quaternion endRotate;
        if (isopen)
        {
            endRotate = rotationshut;
        }
        else
        {
            endRotate = rotationopen;
        }

        isopen = !isopen;
        while (Quaternion.Angle(transform.rotation, endRotate) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, endRotate, Time.deltaTime * speed);
            yield return null;
        }
        transform.rotation = endRotate;
    }
}
