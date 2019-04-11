using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public  float                   inputCooldown       = 0.1f;
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
        RaycastHit[] hits = Physics.RaycastAll(transform.position, p_direction, p_direction.magnitude);
        if (hits.Length > 0)
        {
            if(hits.Length > 1 && hits[0].distance > hits[1].distance)
            {
                RaycastHit farHit = hits[0];
                hits[0] = hits[1];
                hits[1] = farHit;
            }
            bool blocked = false;
            foreach(RaycastHit hit in hits)
            {
                switch (hit.collider.tag)
                {
                    case "Wall":
                        if (p_direction.magnitude > 1 && hit.distance > 1)
                        {
                                transform.Translate(p_direction.normalized);
                        }
                        else
                            blocked = true;
                        break;
                    case "Spike":
                        if (p_direction.magnitude > 1 && hit.distance < 1)
                        {
                            transform.Translate(p_direction.normalized);
                            blocked = true;
                        }
                        else
                        {
                            transform.Translate(p_direction);
                        }
                        break;

                    case "Finish":
                        if(p_direction.magnitude > 1 && hit.distance < 1)
                        {
                            if(hits.Length > 1 && hits[1].collider.tag == "Wall")
                            {
                                transform.Translate(p_direction.normalized);
                                LevelManager.Instance.EndPointEntered();
                                blocked = true;
                            }
                            else
                            {
                                transform.Translate(p_direction);
                            }
                        } 
                        else
                        { 
                            if(!LevelManager.Instance.EndPointEntered())
                            { 
                                transform.Translate(p_direction);
                                blocked = true;
                            }
                        }
                        break;
                    default:
                        if(hits.Length == 1 || hit.Equals(hits[1]))
                        {
                            transform.Translate(p_direction);
                        }
                        break;
                }
                if(blocked) 
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