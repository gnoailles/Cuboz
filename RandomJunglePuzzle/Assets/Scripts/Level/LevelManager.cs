using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController        m_playerController      = null;
    
    private InputManager            m_inputManager          = null;
    
    [SerializeField]
    private string                  m_nextLevel             = null;
    
    [SerializeField]
    private bool                    m_randomizeInputs       = true;

    [SerializeField] [Tooltip("Number of collectibles needed to finish level")]
    private ushort                  m_neededValidatingElements    = 0;

    private GameObject[]            m_spawnPoints;
    private List<ValidatingElement> m_validatedElements = new List<ValidatingElement>();

    private float m_levelTimer = 0.0f;
    public float LevelTimer => m_levelTimer;
    private bool m_isPaused = false;
    public bool IsPaused => m_isPaused;
    private bool m_shouldUnpause = false;
    private float m_unpauseTimer = 0.0f;

    private static float m_totalTime;
    public float TotalTime => m_totalTime;

    private static LevelManager m_instance;
    public  static LevelManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<LevelManager>();
            }

            return m_instance;
        }
    }

    void Start()
    {
        UpdateSpawnPoints();
        if(m_spawnPoints == null)
        {
            throw new UnassignedReferenceException("Missing spawn points!");
        }
        if(m_nextLevel.Length <= 0)
        {
            throw new UnassignedReferenceException("Missing next level name!");
        }
        if(m_playerController == null)
        {
            throw new UnassignedReferenceException("Missing player controller reference!");
        }

        m_inputManager = FindObjectOfType<InputManager>();
        if(m_inputManager == null)
        {
            GameObject container = new GameObject("InputManager");
            m_inputManager = container.AddComponent<InputManager>();

            if (!m_randomizeInputs)
            {
                m_inputManager.SetTestInputs();
            }
            else
            {
                m_inputManager.RandomizeInputs();
            }
        }
        else
        {
            if (!m_randomizeInputs)
            {
                m_inputManager.SetTestInputs();
            }
        }
        m_inputManager.player = m_playerController;

        m_playerController.transform.position = GetSpawnPos();

        PlayerHUD playerHud = FindObjectOfType<PlayerHUD>();
        if (playerHud == null)
        {
            Instantiate(Resources.Load("HUD"));
        }

        if(m_neededValidatingElements > m_validatedElements.Count)
        {
            StartCoroutine("RaiseFinish");
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))   
        {
            m_isPaused = !m_isPaused;
        }

        if (m_isPaused && m_shouldUnpause && m_unpauseTimer <= 0)
        {
            m_shouldUnpause = false;
            m_isPaused = false;
        }

        if (m_shouldUnpause)
        {
            m_unpauseTimer -= Time.deltaTime;
            m_unpauseTimer = Mathf.Max(m_unpauseTimer, 0.0f);
        }

        if (!m_isPaused)
            m_levelTimer += Time.deltaTime;
    }

    [ContextMenu("Update SpawnPoints")]
    void UpdateSpawnPoints()
    {
        m_spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
    }

    private void OnDrawGizmos()
    {
        if(m_spawnPoints == null)
        {
            UpdateSpawnPoints();
        }
        if(m_spawnPoints != null)
        { 
            foreach (GameObject spawnPoint in m_spawnPoints)
            {
                Gizmos.DrawWireCube(spawnPoint.transform.position, Vector3.one);
            }
        }
    }

    public Vector3 GetSpawnPos()
    {
        return m_spawnPoints[Random.Range(0, m_spawnPoints.Length)].transform.position;
    }

    public void ValidateElement(ValidatingElement p_element)
    {
        m_validatedElements.Add(p_element);
        if (m_neededValidatingElements <= m_validatedElements.Count)
        {
            StartCoroutine("LowerFinish");
        }
    }

    public void ResetValidatedElements()
    {
        foreach(ValidatingElement element in m_validatedElements)
        {
            element.Reset();
        }
        m_validatedElements.Clear();
        if (m_neededValidatingElements > m_validatedElements.Count)
        {
           StartCoroutine("RaiseFinish");
        }
    }

    public bool LevelComplete()
    {
        if(m_validatedElements.Count >= m_neededValidatingElements)
        {

            return true;
        }
        return false;
    }

    public void LoadNextScene()
    {
        if(SceneManager.GetActiveScene().name == "Level 10")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        { 
            SceneManager.LoadScene("StartZone");
            StartZone.sceneToLoad = m_nextLevel;
        }
    }

    public void SetPaused(bool p_status)
    {
        if (p_status)
        {
            m_isPaused = true;
            m_shouldUnpause = false;
        }
        else
        {
            m_shouldUnpause = true;
            m_unpauseTimer = 0.1f;
        }
    }

    void OnDestroy()
    {
        m_totalTime += m_levelTimer;
    }

    IEnumerator LowerFinish()
    {
        float elapsedTime = 0;
        GameObject finish = GameObject.FindGameObjectWithTag("Finish");
        Vector3 pos = finish.transform.position;
        float finalHeight = pos.y - 0.5f;
        while (elapsedTime < 1.0f)
        {
            pos.y = Mathf.Lerp(pos.y, finalHeight, (elapsedTime / 1.0f));
            finish.transform.position = pos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RaiseFinish()
    {
        float elapsedTime = 0;
        GameObject finish = GameObject.FindGameObjectWithTag("Finish");
        Vector3 pos = finish.transform.position;
        float finalHeight = pos.y + 0.5f;
        while (elapsedTime < 1.0f)
        {
            pos.y = Mathf.Lerp(pos.y, finalHeight, (elapsedTime / 1.0f));
            finish.transform.position = pos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
