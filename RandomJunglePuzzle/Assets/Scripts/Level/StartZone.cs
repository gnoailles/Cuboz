using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartZone : MonoBehaviour
{
    static public string m_sceneToLoad = "TestLevelElements";
    
    private InputManager m_inputManager = null;

    [SerializeField]
    private Transform m_player = null;

    [SerializeField, Tooltip("In seconds")]
    private float m_duration = 30.0f;

    [SerializeField, Tooltip("In meters")]
    private ushort m_xSize = 1;

    [SerializeField, Tooltip("In meters")]
    private ushort m_zSize = 1;

    private float m_timer = 0.0f;

    void Start()
    {
        m_inputManager = FindObjectOfType<InputManager>();
        if (m_inputManager == null)
        {
            GameObject container = new GameObject("InputManager");
            m_inputManager = container.AddComponent<InputManager>();
        }
        m_inputManager.RandomizeInputs();
        Initialize();
    }

    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= m_duration)
            SceneManager.LoadScene(m_sceneToLoad);
    }

    [ContextMenu("Initialize")]
    void Initialize()
    {
        transform.localScale = new Vector3(m_xSize, 1.0f, m_zSize);
        transform.position = new Vector3(m_xSize / 2.0f, -0.5f, m_zSize / 2.0f);

        if (m_player == null)
            Debug.Log("Member \"Player\" is null");
        else
            m_player.position = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
    }
}
