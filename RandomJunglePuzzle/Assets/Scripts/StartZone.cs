using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    [SerializeField, Tooltip("In seconds")]
    private float m_duration = 1;

    [SerializeField, Tooltip("In meters per seconds")]
    private float m_speed = 1;

    [SerializeField]
    private Transform m_spawnPoint = null;

    private float m_time = 0;
    private Vector3 m_startPosition;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        m_time += Time.deltaTime / m_duration;
        transform.position = Vector3.Lerp(m_startPosition, m_spawnPoint.position, m_time);
    }

    [ContextMenu("Initialize")]
    void Initialize()
    {
        if (m_spawnPoint == null)
            Debug.Log("Member \"SpawnPoint\" is required.");
        else
            transform.position = new Vector3(m_spawnPoint.position.x, m_spawnPoint.position.y + m_duration * m_speed, m_spawnPoint.position.z);

        m_startPosition = transform.position;
    }
}
