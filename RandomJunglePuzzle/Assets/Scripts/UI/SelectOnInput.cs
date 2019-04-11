using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnInput : MonoBehaviour
{
    [SerializeField] private EventSystem m_eventSystem;
    [SerializeField] private GameObject m_selectedObject;
    [SerializeField] private Button m_backButton;

    private bool m_buttonSelected;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_buttonSelected && Input.GetAxisRaw("VerticalDPad") != 0)
        {
            m_eventSystem.SetSelectedGameObject(m_selectedObject);
            m_buttonSelected = true;
        }

        if (m_backButton != null && Input.GetButton("B"))
            m_backButton.onClick.Invoke();
    }

    private void OnDisable()
    {
        m_buttonSelected = false;
    }
}
