using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform       m_end;
    [SerializeField] [Tooltip("Number of collectibles needed to finish level")]
    private ushort          m_neededCollectibles = 0;
    private GameObject[]    m_spawnPoints;

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
    }
    

    [ContextMenu("Update SpawnPoints")]
    void UpdateSpawnPoints()
    {
        m_spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
    }

    public Vector3 GetSpawnPos()
    {
        return m_spawnPoints[Random.Range(0, m_spawnPoints.Length)].transform.position;
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
}
