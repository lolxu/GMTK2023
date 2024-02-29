using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNonPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform dragging = null;
    private Vector3 offset;
    [SerializeField] private LayerMask movableLayers;
    public bool isDragging = false;
    private Collider2D currentCollider;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            // Cast our own ray.
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
                float.PositiveInfinity, movableLayers);
            if (hit)
            {
                currentCollider = hit.collider;
                currentCollider.isTrigger = true;
                // If we hit, record the transform of the object we hit.
                dragging = hit.transform;
                // And record the offset.
                offset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            isDragging = true;
        } else if (Input.GetMouseButtonUp(0)) {
            // Stop dragging.
            dragging = null;
            isDragging = false;
            if (currentCollider != null)
            {
                currentCollider.isTrigger = false;
            }
            
        }

        if (dragging != null) {
            // Move object, taking into account original offset.
            dragging.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }
}
