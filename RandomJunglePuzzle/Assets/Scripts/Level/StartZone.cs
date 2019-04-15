using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
struct DelayedLoad
{
    public string  scene;
    public float   delay;
}

public class StartZone : MonoBehaviour
{
    static public string    sceneToLoad     = "Level 1";
    
    private InputManager m_inputManager = null;

    private float m_timer = 0.0f;
    public float Timer => m_timer;

    [SerializeField]
    private List<DelayedLoad> m_waitTimes;
    
    void Start()
    {
        m_timer = m_waitTimes.Find(x => x.scene == sceneToLoad).delay;

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
            SceneManager.LoadScene(sceneToLoad);
    }
}
