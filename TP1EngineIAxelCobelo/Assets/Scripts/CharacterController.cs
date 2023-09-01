using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Camera m_camera;
    [SerializeField]
    private float m_accelerationValue;
    [SerializeField]
    private float m_maxVelocity;
    private Rigidbody m_rb;

    void Start()
    {
        m_camera = Camera.main;
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this code is for the character to always move forward no matter its angle relative to the character
        //because otherwise if the camera is on top, the closer it gets to the normal, the less the character moves
        var vectorOnFloor = Vector3.ProjectOnPlane(m_camera.transform.forward, Vector3.up);
        vectorOnFloor.Normalize();//we have always to normailize after a dot.product
        if (Input.GetKey(KeyCode.W))
        {
            m_rb.AddForce(vectorOnFloor*m_accelerationValue, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_rb.AddForce(vectorOnFloor * m_accelerationValue*-1, ForceMode.Acceleration);
        }

        if (m_rb.velocity.magnitude > m_maxVelocity)
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= m_maxVelocity;
            Debug.Log("mag: " + m_rb.velocity.magnitude + " Velocity: " + m_rb.velocity);
        }
    }
}
