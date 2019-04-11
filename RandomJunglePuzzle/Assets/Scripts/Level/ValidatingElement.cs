using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class ValidatingElement : MonoBehaviour
{
    private bool    m_validated = false;

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
        GetComponent<MeshRenderer>().material.color = Color.green;
        LevelManager.Instance.ValidateElement();
    }
}
