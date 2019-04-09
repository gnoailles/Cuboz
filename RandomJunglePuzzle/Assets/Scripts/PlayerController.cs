using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ushort m_dashSize = 2;
    
    void Start()
    {
    }

    public void MoveForward()
    {
        if(!HaveCollision(Vector3.forward))
        {
            transform.Translate(Vector3.forward);
        }
    }

    public void MoveBackward()
    {
        if (!HaveCollision(Vector3.back))
        {
            transform.Translate(Vector3.back);
        }
    }

    public void MoveRight()
    {
        if (!HaveCollision(Vector3.right))
        {
            transform.Translate(Vector3.right);
        }
    }
    public void MoveLeft()
    {
        if (!HaveCollision(Vector3.left))
        {
            transform.Translate(Vector3.left);
        }
    }

    public void DashForward()
    {
        if (!HaveCollision(Vector3.forward * m_dashSize))
        {
            transform.Translate(Vector3.forward * m_dashSize);
        }
    }

    public void DashBackward()
    {
        if (!HaveCollision(Vector3.back * m_dashSize))
        {
            transform.Translate(Vector3.back * m_dashSize);
        }
    }

    public void DashRight()
    {
        if (!HaveCollision(Vector3.right * m_dashSize))
        {
            transform.Translate(Vector3.right * m_dashSize);
        }
    }
    public void DashLeft()
    {
        if (!HaveCollision(Vector3.left * m_dashSize))
        {
            transform.Translate(Vector3.left * m_dashSize);
        }
    }

    public void Respawn()
    {
    }

    private bool HaveCollision(Vector3 p_direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, p_direction, out hit, p_direction.magnitude))
        {
            Debug.Log(hit.collider.tag);

            switch (hit.collider.tag)
            {
                case "Hole":
//                    return p_direction.magnitude <= 1;
                    break;
                default:
                    break;
            }
        }
        else
        {
            return false;
        }


        return true;
    }
}