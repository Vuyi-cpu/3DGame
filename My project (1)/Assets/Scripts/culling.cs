using UnityEngine;
using System.Collections.Generic;

public class DistanceCulling : MonoBehaviour
{
    [Header("Culling Settings")]
    public Transform playerCamera;
    public float renderDistance = 20f;

    private List<Renderer> renderers = new List<Renderer>();

    void Start()
    {
        if (!playerCamera)
            playerCamera = Camera.main.transform;

        // Collect ALL renderers in this object and its children
        renderers.AddRange(GetComponentsInChildren<Renderer>());
    }

    void Update()
    {
        if (playerCamera == null) return;

        float dist = Vector3.Distance(playerCamera.position, transform.position);
        bool shouldRender = dist < renderDistance;

        // Enable/disable all child renderers
        foreach (Renderer rend in renderers)
        {
            if (rend != null)
                rend.enabled = shouldRender;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Optional: Draw range for visual debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, renderDistance);
    }
}

