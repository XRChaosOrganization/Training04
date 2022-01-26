using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Camera mainCamera;
    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint2D;
    public float grappleMaxDistance;
    public Transform firePoint;
    private Vector2 grapplePoint;
    private bool isGrappling;
    public float swingForce;
    public float tongueLengthSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer.enabled = false;
        distanceJoint2D.enabled = false;
        distanceJoint2D.distance = grappleMaxDistance;
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Grapple();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isGrappling = false;
            distanceJoint2D.enabled = false;
            lineRenderer.enabled = false;
            distanceJoint2D.distance = grappleMaxDistance;
        }
        if (Input.GetAxisRaw("Horizontal") !=0 && isGrappling)
        {
            rb.AddForce(swingForce * Input.GetAxisRaw("Horizontal")*Vector2.right);
        }
        if (Input.GetAxisRaw("Vertical") != 0 && isGrappling)
        {
            if (distanceJoint2D.distance <= grappleMaxDistance )
            {
                distanceJoint2D.distance -= Input.GetAxisRaw("Vertical") * tongueLengthSpeed;
            }
            else
            {
                distanceJoint2D.distance = 10;
            }
        }
        
        if (lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, transform.position);
        }
    }

    public void Grapple()
    {
        // LOOK At A AJOUTER
        // LERP A AJOUTER
        Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 grappleDir = mousePos - (Vector2)firePoint.position;
        RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, grappleDir.normalized,grappleMaxDistance);
        if (_hit.collider != null)
        {
            grapplePoint = _hit.point;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grapplePoint);
            distanceJoint2D.connectedAnchor = grapplePoint;
            distanceJoint2D.enabled = true;
            lineRenderer.enabled = true;
            isGrappling = true;
        }
        else
        {
            Vector2 failedGrapplePoint = (Vector2)transform.position + grappleDir.normalized * grappleMaxDistance;
            
            Debug.Log(failedGrapplePoint);
            StartCoroutine(FailToGrapple(failedGrapplePoint));
            
        }
    }

    public IEnumerator FailToGrapple(Vector2 _grapplePoint)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, _grapplePoint);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(.2f);
        lineRenderer.enabled = false;
    }
}
