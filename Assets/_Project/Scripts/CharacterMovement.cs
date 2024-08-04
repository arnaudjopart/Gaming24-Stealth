using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private Animator m_characterAnimator;

    [SerializeField]private float m_speed =.5f;
    private Transform m_camera;
    private Vector3 m_moveVector;
    private float m_inputSpeed;
    private float m_currentVelocity;
    [SerializeField] private float m_smoothTime =.3f;
    private float m_targetSpeed;
    [SerializeField] private LayerMask m_wallLayer;
    [SerializeField] private Transform m_rayStartPoint;
    [SerializeField] private float m_wallDetectionDistance;
    private float m_wallCoverTimer;
    [SerializeField] private float m_wallCoverValidation =.3f;

    private enum STATE
    {
        MOVE,
        COVER
        
    }

    private STATE m_currentState;
    private bool m_moveVectorsHaveBeenUpdated;
    private Vector3 m_cameraForwardVectorOnXZPlane;
    private Vector3 m_cameraRightVectorOnXZPlane;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        m_characterController.SimpleMove(m_moveVector * (( m_speed) * m_inputSpeed));
        m_characterAnimator.SetFloat("speed",m_inputSpeed);
        
        m_inputSpeed = Mathf.SmoothDamp(m_inputSpeed, m_targetSpeed, ref m_currentVelocity, m_smoothTime);


        switch (m_currentState)
        {
            case STATE.MOVE:
                var wallCollisionRay = new Ray(m_rayStartPoint.position, transform.forward);
                if (Physics.Raycast(wallCollisionRay, out var hit, m_wallDetectionDistance, m_wallLayer))
                {
                    print("touched a wall");
                    m_wallCoverTimer += Time.deltaTime;
                    if (m_wallCoverTimer >= m_wallCoverValidation)
                    {
                        Debug.Log("Cover Mode");
                        m_characterAnimator.SetBool("isCover",true);
                        transform.rotation = Quaternion.LookRotation(hit.normal);
                        m_currentState = STATE.COVER;
                    }
                }
                else
                {
                    m_wallCoverTimer = 0;
                }
                break;
            case STATE.COVER:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
        
    }

    public void Move(InputAction.CallbackContext _context)
    {
        var inputData = _context.ReadValue<Vector2>();

        switch (m_currentState)
        {
            case STATE.MOVE:

                m_targetSpeed = inputData.magnitude;
                
                if (inputData.magnitude < .01f)
                {
                    var forward = m_camera.forward;
                    m_cameraForwardVectorOnXZPlane = new Vector3(forward.x, 0, forward.z).normalized;
                    var right = m_camera.right;
                    m_cameraRightVectorOnXZPlane = new Vector3(right.x, 0, right.z).normalized;
                }
                

                m_moveVector = (inputData.x * m_cameraRightVectorOnXZPlane + inputData.y * m_cameraForwardVectorOnXZPlane)
                    .normalized;

                transform.rotation = Quaternion.LookRotation(m_moveVector);
                break;
            case STATE.COVER:
                
                m_moveVector = Vector3.zero;
                
                if (inputData.y <= -.8f)
                {
                    m_characterAnimator.SetBool("isCover",false);
                    m_currentState = STATE.MOVE;
                }
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
    
}
