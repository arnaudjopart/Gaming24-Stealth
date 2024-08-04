using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingForTarget : MonoBehaviour
{
    private Player m_target;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        m_target = GameManager.Instance.GetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}