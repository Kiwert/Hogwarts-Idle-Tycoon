using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    private Vector3 dragOrigin;
    private Camera mainCamera;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float duration;
    [SerializeField] float zoomMultiplier;
    public float selectZoomAmount;
    [SerializeField] float minY = 5f;
    [SerializeField] float maxY = 15f;
    [SerializeField] float dragSmooth;
    [SerializeField] float focusDragSmooth;
    [SerializeField] float idleMovementMagnitude = 0.2f;
    [SerializeField] float idleMovementSpeed = 1f;
    
    [SerializeField] float distanceFromHitObject;
    
    Vector3 moveTarget;
    private Transform cameraFirstChild;
    private bool isDragging = false;
    public bool isFocusing = false;
    private float lastClickTime;
    private float doubleClickTime = 0.3f;
    bool zoomin;
    GameObject lastHit;


    //this script needs to be optimazed too, it is complicated because i am using prespective camera (zoom in/out), in case we will use orthographic camera alot will be removed
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);

    }
    void Start()
    {
        mainCamera = Camera.main;
        cameraFirstChild = transform.GetChild(0);
        moveTarget = transform.position;
    }
    public void Zoomin()
    {
        zoomin = true;
        
    }
    void Update()
    {
        if (zoomin)
        {
            lastHit.GetComponent<AreaInteraction>().SelectArea();
            Vector3 newTarget = lastHit.transform.position;
            Vector3 lookDir = cameraFirstChild.transform.forward;
            Vector3 offset = lookDir.normalized * -distanceFromHitObject;
            offset.y = transform.position.y;
            moveTarget = new Vector3(newTarget.x, 0, newTarget.z) + offset;
            isFocusing = true;
            isDragging = false;
            Zoom(selectZoomAmount);
            zoomin = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
           
            if (Time.time - lastClickTime < doubleClickTime)
            {
               
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("area"))
                    {
                        lastHit = hit.collider.gameObject;
                        hit.collider.gameObject.GetComponent<AreaInteraction>().SelectArea();
                        Vector3 newTarget = hit.transform.position;
                        Vector3 lookDir = cameraFirstChild.transform.forward;
                        Vector3 offset = lookDir.normalized * -distanceFromHitObject;
                        offset.y = transform.position.y;
                        moveTarget = new Vector3(newTarget.x,0, newTarget.z) + offset;
                        isFocusing = true;
                        isDragging = false;
                        Zoom(selectZoomAmount);
                       
                        
                    }
                }

            }
            else
            {
              
               
                isDragging = true;
                zoomin = false;
                dragOrigin = Input.mousePosition;
            }

            lastClickTime = Time.time;
        }

        if (Input.GetMouseButton(0) && isDragging )
        {
           
           
            Vector3 difference = Input.mousePosition - dragOrigin;
            if (difference != Vector3.zero)
            {
                UIManager.Instance.HideAreaEditorPanel();
                UIManager.Instance.HideAreaUnlockPanel();
            }
            moveTarget += new Vector3(-difference.x, 0f, -difference.y);
            dragOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
               
                moveTarget = transform.position;
                dragOrigin = moveTarget;
            }
            isDragging = false;
        }

        if (!isDragging&&!isFocusing)
        {
           
            Vector3 idleMovement = new Vector3(Mathf.Sin(Time.time * idleMovementSpeed), 0f, Mathf.Cos(Time.time * idleMovementSpeed));
            moveTarget = transform.position + idleMovement * idleMovementMagnitude;

        }
        
        if (transform.position != moveTarget)
        {
            if (isFocusing)
            {
                transform.position = Vector3.Lerp(transform.position, moveTarget, Time.deltaTime * focusDragSmooth);
            }
            else
                transform.position = Vector3.Lerp(transform.position, moveTarget, Time.deltaTime * dragSmooth);
        }
        else
        {
            isFocusing = false;
        }
            

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Zoom(Input.GetAxis("Mouse ScrollWheel") * 10 * zoomMultiplier);
        }
        if (isFocusing&& Input.GetAxis("Mouse ScrollWheel") != 0&&!isDragging)
        {
            Zoom(selectZoomAmount* zoomMultiplier);
        }


    }
    void Zoom(float amount)
    {
        float currentZoomDistance = Vector3.Distance(cameraFirstChild.transform.position, transform.position);
        float targetZoomDistance = currentZoomDistance - (amount * zoomSpeed);

       
        StartCoroutine(SmoothZoom(targetZoomDistance, currentZoomDistance));
    }

    IEnumerator SmoothZoom(float targetZoomDistance, float currentZoomDistance)
    {
        Debug.Log("ZOOMIN");
        float timer = 0f;
     

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float newZoomDistance = Mathf.Lerp(currentZoomDistance, targetZoomDistance, t);

            float clampedZoomDistance = Mathf.Clamp(newZoomDistance, minY, maxY);
            Vector3 zoomDirection = cameraFirstChild.transform.forward;
            cameraFirstChild.localPosition = zoomDirection * -clampedZoomDistance;

            yield return null;
        }
    }
}
