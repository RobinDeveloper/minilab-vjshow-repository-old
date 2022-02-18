using System;
using System.Collections;
using System.Collections.Generic;
using ParticleBoom;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttractInput : MonoBehaviour
{
    [SerializeField] private AttractorParent[] m_Attractors;

    [Header("InputActions")] 
    [SerializeField] private InputAction m_ScaleMultiplier;
    [SerializeField] private InputAction m_GravityChanger;

    private void OnEnable()
    {
        m_ScaleMultiplier.performed += ChangeScaleMultiplier;
        m_GravityChanger.performed += ChangeGravity;
        
        m_GravityChanger.Enable();
        m_ScaleMultiplier.Enable();
    }
    
    private void OnDisable()
    {
        m_ScaleMultiplier.performed -= ChangeScaleMultiplier;
        m_GravityChanger.performed -= ChangeGravity;
        
        m_GravityChanger.Disable();
        m_ScaleMultiplier.Disable();
    }

    private void ChangeGravity(InputAction.CallbackContext _obj)
    {
        for (int i = 0; i < m_Attractors.Length; i++)
        {
            m_Attractors[i].GravityStrength = (m_Attractors[i].GravityStrength * _obj.ReadValue<float>()) * 1.5f;
            m_Attractors[i].MaxGravity = m_Attractors[i].GravityStrength + 25;
        }
    }

    private void ChangeScaleMultiplier(InputAction.CallbackContext _obj)
    {
        for (int i = 0; i < m_Attractors.Length; i++)
        {
            m_Attractors[i].AudioScaleMultiplier = 1 * _obj.ReadValue<float>();
        }
    }
}
