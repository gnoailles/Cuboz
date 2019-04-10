using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ushort m_dashSize          = 2;

    [SerializeField] private bool   m_restartAtSpawn    = false;

    private Vector3 m_lastSafePosition;
    private Vector3 m_positionBuffer;

    void Start()
    {
        m_lastSafePosition = m_positionBuffer = transform.position;
    }

    void Update()
    {
        if (transform.position.y <= -0.1)
            Respawn();
    }

    public void MoveForward()
    {
        TryMove(Vector3.forward);
    }

    public void MoveBackward()
    {
        TryMove(Vector3.back);
    }

    public void MoveRight()
    {
        TryMove(Vector3.right);
    }

    public void MoveLeft()
    {
        TryMove(Vector3.left);
    }

    public void DashForward()
    {
        TryMove(Vector3.forward * m_dashSize);
    }

    public void DashBackward()
    {
        TryMove(Vector3.back * m_dashSize);
    }

    public void DashRight()
    {
        TryMove(Vector3.right * m_dashSize);
    }

    public void DashLeft()
    {
        TryMove(Vector3.left * m_dashSize);
    }

    public void Respawn()
    {
        if(m_restartAtSpawn)
        {
            m_lastSafePosition = LevelManager.Instance.GetSpawnPos();
        }
        m_positionBuffer = transform.position = m_lastSafePosition;
    }

    private void TryMove(Vector3 p_direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, p_direction, out hit, p_direction.magnitude))
        {
            switch (hit.collider.tag)
            {
                case "Wall":
                    if (p_direction.magnitude > 1 && hit.distance > 1)
                        transform.Translate(p_direction.normalized);
                    break;
                case "Spike":
                    if (p_direction.magnitude > 1 && hit.distance < 1)
                        transform.Translate(p_direction.normalized);
                    else
                        transform.Translate(p_direction);
                    break;

                default:
                        transform.Translate(p_direction);
                    break;
            }
        }
        else
        {
            transform.Translate(p_direction);
        }

        if (!m_restartAtSpawn)
        {
            m_lastSafePosition = m_positionBuffer;
            m_positionBuffer = transform.position;
        }
    }
}