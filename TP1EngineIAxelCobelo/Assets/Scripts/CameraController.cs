using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_objectToLookAt;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private Vector2 m_clampingXRotationValues = Vector2.zero;
    [SerializeField]
    private float m_camMaxDist = 10.0f;
    [SerializeField]
    private float m_camMinDist = 2.0f;
    [SerializeField]
    private float scrollSpeed = 4.0f; // Scroll speed


    // Update is called once per frame
    void Update()
    {
        UpdateHorizontalMovements();
        UpdateVerticalMovements();
        UpdateCameraScroll();
    }

    private void FixedUpdate()
    {
        FixedUpdateCameraObstruction();
    }

    private void UpdateHorizontalMovements()
    {
        float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeed;
        transform.RotateAround(m_objectToLookAt.position, m_objectToLookAt.up, currentAngleX);
    }

    private void UpdateVerticalMovements()
    {
        //we get the mouse Y
        float currentAngleY = Input.GetAxis("Mouse Y") * m_rotationSpeed;
        //we get the x rotation of the cam (using eulerAngles for some reason)
        float eulersAngleX = transform.rotation.eulerAngles.x;
        //we add the 2 for some reason
        float comparisonAngle = eulersAngleX + currentAngleY;
        //we clamp it - actually, we're just keeping the values between -180 and 180
        comparisonAngle = ClampAngle(comparisonAngle);
        //perguntar Maxime - se currAngleY (mouseY) < 0? E addedAngles <  m_clampingXRotationValues.x (que é 0?)
        //OU a mesma coisa se for maior...
        if ((currentAngleY < 0 && comparisonAngle < m_clampingXRotationValues.x)
            || (currentAngleY > 0 && comparisonAngle > m_clampingXRotationValues.y))
        {
            return;
        }
        transform.RotateAround(m_objectToLookAt.position, transform.right, currentAngleY);
    }

    private void UpdateCameraScroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {    
            //TODO: Faire une vérification selon la distance la plus proche ou la plus éloignée
            //Que je souhaite entre ma caméra et mon objet

            float currCam2CharDist = m_objectToLookAt.transform.position.z - transform.position.z;
            //Debug.Log("scroll" + camDist);
            if ((currCam2CharDist > m_camMinDist) && (currCam2CharDist < m_camMaxDist))
            {
                //TODO: Lerp plutôt que d'effectuer immédiatement la translation
                //on trouve la prochaine position
                Vector3 targetPos = transform.position + Vector3.forward * Input.mouseScrollDelta.y;
                //et on Lerp vers elle
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * scrollSpeed);
            }
            else
            {
               if(currCam2CharDist <= m_camMinDist)
                {
                    return;
                    //transform.Translate(Vector3.back*0.5f, Space.Self);
                }
               else if(currCam2CharDist >= m_camMaxDist)
                {
                    return;
                    //transform.Translate(Vector3.forward * 0.5f, Space.Self);
                }
            }
        }
    }

    private void FixedUpdateCameraObstruction()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Hit!");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.white);
        }
    }

    private float ClampAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
}





/*
    public Transform target; // The character's position
    public float minDistance = 2f; // Minimum distance on the z-axis
    public float maxDistance = 10f; // Maximum distance on the z-axis
    public float scrollSpeed = 2f; // Scroll speed

 
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        Vector3 moveDirection = transform.forward * scrollInput * scrollSpeed;

        // Calculate the new camera position
        Vector3 newPosition = transform.position + moveDirection;

        // Calculate the clamped z position
        newPosition.z = Mathf.Clamp(newPosition.z, target.position.z - maxDistance, target.position.z - minDistance);

        // Move the camera to the new position
        transform.Translate(moveDirection, Space.World);

        // Ensure the camera stays within the distance limits
        if (transform.position.z > target.position.z - minDistance)
        {
            transform.Translate(Vector3.forward * (target.position.z - minDistance - transform.position.z), Space.World);
        }
        else if (transform.position.z < target.position.z - maxDistance)
        {
            transform.Translate(Vector3.forward * (target.position.z - maxDistance - transform.position.z), Space.World);
        }

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The character's position
    public float minDistance = 2f; // Minimum distance on the z-axis
    public float maxDistance = 10f; // Maximum distance on the z-axis
    public float scrollSpeed = 2f; // Scroll speed

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float offsetZ = scrollInput * scrollSpeed;

        // Calculate the new target position
        targetPosition.z = Mathf.Clamp(targetPosition.z - offsetZ, target.position.z - maxDistance, target.position.z - minDistance);

        // Interpolate camera position using Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * scrollSpeed);
    }
}

*/

