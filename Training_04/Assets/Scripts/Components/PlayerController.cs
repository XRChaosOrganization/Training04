using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Camera mainCamera;
    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint2D;
    public float grappleRange;
    private float grappleMaxDistance;
    public Transform firePoint;
    private Vector2 grapplePoint;
    private bool isGrappling;
    public float swingForce;
    public float tongueLengthSpeed;
    public static PlayerController playerC;
    public LayerMask grappables;
    private float angle;
    public float timeTest;
    public GameObject aimCursor;
    private float aimDistance = 2;
    
    void Awake()
    {
        playerC = this;
        rb = GetComponent<Rigidbody2D>();
        lineRenderer.enabled = false;
        ReleaseRope();
        Time.timeScale = timeTest;
    }
    void Update()
    {
        if (isGrappling == false)
        {
            aimCursor.GetComponent<SpriteRenderer>().enabled = true;
            Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 grappleDir = mousePos - (Vector2)firePoint.position;
            aimCursor.transform.position = (Vector2)transform.position + grappleDir.normalized * aimDistance;
        }
        else
        {
            aimCursor.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGrappling == false)
        {
            Grapple();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            ReleaseRope();
        }
        if (isGrappling)
        {
            UpdateRopePhysics();
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                rb.AddForce(swingForce * Input.GetAxisRaw("Horizontal") * Vector2.right);
            }
            if (Input.GetAxisRaw("Vertical") != 0 )
            {
                if (distanceJoint2D.distance <= grappleMaxDistance)
                {
                    distanceJoint2D.distance -= Input.GetAxisRaw("Vertical") * tongueLengthSpeed;
                }
                else
                {
                    distanceJoint2D.distance = grappleMaxDistance;
                }
            }
            
        }
        if (isGrappling && lineRenderer.positionCount > 2)
        {
            
            Vector2 hingeDir = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(2);
            Vector2 playerDir = lineRenderer.GetPosition(0) - lineRenderer.GetPosition(2);
            angle = Vector2.Angle(hingeDir, playerDir);
            
            if (angle <1f)
            {
                UnwrapRope();
            }
;        }
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
            Vector2 distance = grapplePoint - (Vector2)transform.position;
            distanceJoint2D.distance = distance.magnitude;
            distanceJoint2D.enabled = true;
            lineRenderer.enabled = true;
            isGrappling = true;
        }
        else
        {
            Vector2 failedGrapplePoint = (Vector2)transform.position + grappleDir.normalized * grappleMaxDistance;
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
    public void ReleaseRope()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        isGrappling = false;
        distanceJoint2D.enabled = false;
        lineRenderer.enabled = false;
        grappleMaxDistance = grappleRange;
        distanceJoint2D.distance = grappleMaxDistance;
    }
    void UpdateRopePhysics()
    {
        Vector2 anchorDir = lineRenderer.GetPosition(1) - transform.position;
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, anchorDir, Vector2.Distance(transform.position, distanceJoint2D.connectedAnchor) -0.07f,grappables);
        if (_hit )
        {
            if (GetclosestCorner(_hit.collider.transform,_hit.point) != (Vector2)lineRenderer.GetPosition(1))
            {
                lineRenderer.positionCount++;
                for (int i = lineRenderer.positionCount -1; i > 1; i--)
                {
                    lineRenderer.SetPosition(i , lineRenderer.GetPosition(i-1));
                }
                lineRenderer.SetPosition(1, GetclosestCorner(_hit.collider.transform,_hit.point));
            }
            WrapRope();
        }

    }
    void WrapRope()
    {
        distanceJoint2D.connectedAnchor = lineRenderer.GetPosition(1);
        distanceJoint2D.distance = grappleMaxDistance- Vector2.Distance(lineRenderer.GetPosition(1),lineRenderer.GetPosition(2));
        grappleMaxDistance = distanceJoint2D.distance;
        distanceJoint2D.distance = (distanceJoint2D.connectedAnchor - (Vector2)transform.position).magnitude;
        
    }
    void UnwrapRope()
    {
        float distanceToAdd = Vector2.Distance(lineRenderer.GetPosition(1), lineRenderer.GetPosition(2));
        distanceJoint2D.connectedAnchor = lineRenderer.GetPosition(2);
        for (int i = 1; i < lineRenderer.positionCount - 1; i++)
        {
            lineRenderer.SetPosition(i, lineRenderer.GetPosition(i+1));
        }
        lineRenderer.positionCount--;
        distanceJoint2D.distance = grappleMaxDistance + distanceToAdd;
        grappleMaxDistance = distanceJoint2D.distance;
        distanceJoint2D.distance = (distanceJoint2D.connectedAnchor - (Vector2)transform.position).magnitude;
        
    }
    Vector2 GetclosestCorner(Transform rect, Vector2 _hitPoint)
    {
        Vector2 cornerAPos = (rect.position.x - rect.localScale.x / 2) * Vector2.right + (rect.position.y + rect.localScale.y / 2) * Vector2.up;
        Vector2 cornerBPos = (rect.position.x + rect.localScale.x / 2) *Vector2.right + (rect.position.y + rect.localScale.y / 2) *Vector2.up ;
        Vector2 cornerCPos = (rect.position.x - rect.localScale.x / 2) *Vector2.right + (rect.position.y - rect.localScale.y / 2) *Vector2.up ;
        Vector2 cornerDPos = (rect.position.x + rect.localScale.x / 2) *Vector2.right + (rect.position.y - rect.localScale.y / 2) *Vector2.up ;
        float distA = (cornerAPos - _hitPoint).magnitude;
        float distB = (cornerBPos - _hitPoint).magnitude;
        float distC = (cornerCPos - _hitPoint).magnitude;
        float distD = (cornerDPos - _hitPoint).magnitude;
        float minvalue =Mathf.Min(distA, distB, distC, distD);
        Vector2 returnCorner = new Vector2();
        if (minvalue == distA)
        {
            returnCorner = cornerAPos;
        }
        if (minvalue == distB)
        {
            returnCorner = cornerBPos;
        }
        if (minvalue == distC)
        {
            returnCorner = cornerCPos;
        }
        if (minvalue == distD)
        {
            returnCorner = cornerDPos;
        }
        return returnCorner;
    }
    Vector2 LerpTongue(Vector2 _grapplePoint, float _time)
    {
        float x = Mathf.Lerp(transform.position.x, _grapplePoint.x, _time) * Time.deltaTime;
        float y = Mathf.Lerp(transform.position.y, _grapplePoint.y, _time) * Time.deltaTime;
        return new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("DeathZone"))
        {
            Destroy(this.gameObject);
            Debug.Log("GameOver");
            //GameOver
        }
        if (col.CompareTag("Finish"))
        {
            Debug.Log("Level Finished !!");
            //Win screen
        }
    }
}
