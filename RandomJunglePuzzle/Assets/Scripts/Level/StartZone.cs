using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartZone : MonoBehaviour
{
    static public string m_sceneToLoad = "TestLevelElements";
    static public float     m_duration        = 30.0f;
    
    private InputManager m_inputManager = null;

    private float m_timer = 30.0f;
    public float Timer => m_timer;

    void Start()
    {
        m_timer = m_duration;
        m_inputManager = FindObjectOfType<InputManager>();
        if (m_inputManager == null)
        {
            GameObject container = new GameObject("InputManager");
            m_inputManager = container.AddComponent<InputManager>();
        }
        m_inputManager.RandomizeInputs();
    }

    void Update()
    {
        if (!m_inputManager.isPaused)
            m_timer -= Time.deltaTime;

        if (m_timer <= 0)
            SceneManager.LoadScene(m_sceneToLoad);
    }
}
