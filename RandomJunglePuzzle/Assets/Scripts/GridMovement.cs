using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField] private ushort m_dashSize = 2;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void MoveForward()
    {
        transform.Translate(0.0f, 0.0f, 1.0f);
    }

    public void MoveBackward()
    {
        transform.Translate(0.0f, 0.0f, -1.0f);
    }

    public void MoveRight()
    {
        transform.Translate(1.0f, 0.0f, 0.0f);
    }
    public void MoveLeft()
    {
        transform.Translate(-1.0f, 0.0f, 0.0f);
    }

    public void DashForward()
    {
        transform.Translate(0.0f, 0.0f, m_dashSize);
    }

    public void DashBackward()
    {
        transform.Translate(0.0f, 0.0f, -m_dashSize);
    }

    public void DashRight()
    {
        transform.Translate(m_dashSize, 0.0f, 0.0f);
    }
    public void DashLeft()
    {
        transform.Translate(-m_dashSize, 0.0f, 0.0f);
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