using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GridMovement : MonoBehaviour
{
    [SerializeField] private ushort m_dashSize = 2;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void MoveForward()
    {
        if (!HaveCollision(new Vector3(1.0f, 0.0f, 0.0f)))
            transform.Translate(1.0f, 0.0f, 0.0f);
    }

    public void MoveBackward()
    {
        if (!HaveCollision(new Vector3(-1.0f, 0.0f, 0.0f)))
            transform.Translate(-1.0f, 0.0f, 0.0f);
    }

    public void MoveLeft()
    {
        if (!HaveCollision(new Vector3(0.0f, 0.0f, -1.0f)))
            transform.Translate(0.0f, 0.0f, -1.0f);
    }

    public void MoveRight()
    {
        if (!HaveCollision(new Vector3(0.0f, 0.0f, 1.0f)))
            transform.Translate(0.0f, 0.0f, 1.0f);
    }

    public void DashForward()
    {
        if (!HaveCollision(new Vector3(m_dashSize, 0.0f, 0.0f)))
            transform.Translate(m_dashSize, 0.0f, 0.0f);
    }

    public void DashBackward()
    {
        if (!HaveCollision(new Vector3(-m_dashSize, 0.0f, 0.0f)))
            transform.Translate(-m_dashSize, 0.0f, 0.0f);
    }

    public void DashLeft()
    {
        if (!HaveCollision(new Vector3(0.0f, 0.0f, -m_dashSize)))
            transform.Translate(0.0f, 0.0f, -m_dashSize);
    }

    public void DashRight()
    {
        if (!HaveCollision(new Vector3(0.0f, 0.0f, m_dashSize)))
            transform.Translate(0.0f, 0.0f, m_dashSize);
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