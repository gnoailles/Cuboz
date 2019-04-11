using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController    m_playerController      = null;
    
    private InputManager        m_inputManager          = null;
    
    [SerializeField]
    private string              m_nextLevel             = null;
    [SerializeField]
    private float               m_nextStartZoneTimer    = 20.0f;

    [SerializeField]
    private bool                m_randomizeInputs       = true;

    [SerializeField] [Tooltip("Number of collectibles needed to finish level")]
    private ushort              m_neededCollectibles    = 0;

    private GameObject[]        m_spawnPoints;
    private ushort              m_validatedElementsCount = 0;

    private static LevelManager m_instance;
    public  static LevelManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<LevelManager>();

                if (m_instance == null)
                {
                    Debug.Log("A LevelManager is required for every game level!");
                }
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

    public void ValidateElement()
    {
        ++m_validatedElementsCount;
    }

    public bool EndPointEntered()
    {
        if(m_validatedElementsCount >= m_neededCollectibles)
        {
            StartZone.m_sceneToLoad = m_nextLevel;
            StartZone.duration      = m_nextStartZoneTimer;
            SceneManager.LoadScene("StartZone");
            return true;
        }
        return false;
    }
}
