using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class ValidatingElement : MonoBehaviour
{
    private bool        m_validated = false;
    private Color       m_baseEmissionColor;
    public  GameObject  m_plateModel = null;

    private void Start()
    {
        Material mat = GetComponentInChildren<Renderer>().material;
        m_baseEmissionColor = mat.GetColor("_EmissionColor");
        Color finalColor = m_baseEmissionColor * Mathf.LinearToGammaSpace(0.25f);
        DynamicGI.SetEmissive(GetComponentInChildren<Renderer>(), finalColor);
        mat.SetColor("_EmissionColor", finalColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!m_validated && other.GetComponent<PlayerController>() != null)
        {
            Validate();
        }
    }

    private void Validate()
    {
        m_validated = true;
        m_plateModel.transform.Translate(0.0f, -0.1f, 0.0f);

        LevelManager.Instance.ValidateElement(this);
        Material mat = GetComponentInChildren<Renderer>().material;
        mat.SetColor("_EmissionColor", m_baseEmissionColor);
        DynamicGI.SetEmissive(GetComponentInChildren<Renderer>(), m_baseEmissionColor);
    }

    public void Reset()
    {
        if(m_validated)
        { 
            m_plateModel.transform.Translate(0.0f, 0.1f, 0.0f);

            Material mat = GetComponentInChildren<Renderer>().material;
            m_baseEmissionColor = mat.GetColor("_EmissionColor");
            Color finalColor = m_baseEmissionColor * Mathf.LinearToGammaSpace(0.25f);
            DynamicGI.SetEmissive(GetComponentInChildren<Renderer>(), finalColor);

            m_validated = false;
        }
    }
}
