using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject m_virtualCameraGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        m_virtualCameraGameObject.SetActive(true);
        
    }
    private void OnTriggerExit(Collider other)
    {
        m_virtualCameraGameObject.SetActive(false);
        
    }
    
}
