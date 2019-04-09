using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GridMovement : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveForward()
    {
        transform.Translate(1.0f, 0.0f, 0.0f);
    }

    void Respawn()
    {

    }
}
