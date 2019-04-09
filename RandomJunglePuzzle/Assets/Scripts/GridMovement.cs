using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GridMovement : MonoBehaviour
{
    [SerializeField]
    private ushort m_dashSize = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveForward()
    {
        transform.Translate(1.0f, 0.0f, 0.0f);
    }
    public void MoveBackward()
    {
        transform.Translate(-1.0f, 0.0f, 0.0f);
    }
    public void MoveLeft()
    {
        transform.Translate(0.0f, 0.0f, -1.0f);
    }
    public void MoveRight()
    {
        transform.Translate(0.0f, 0.0f, 1.0f);
    }

    public void DashForward()
    {
        transform.Translate(m_dashSize, 0.0f, 0.0f);
    }
    public void DashBackward()
    {
        transform.Translate(-m_dashSize, 0.0f, 0.0f);
    }
    public void DashLeft()
    {
        transform.Translate(0.0f, 0.0f, -m_dashSize);
    }
    public void DashRight()
    {
        transform.Translate(0.0f, 0.0f, m_dashSize);
    }

    void Respawn()
    {
        
    }
}
