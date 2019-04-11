using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField]
    public bool m_ramdomizeAllInputs = ramdomizeAllInputs;
    public static bool  ramdomizeAllInputs = false;

    private void Start()
    {
        ramdomizeAllInputs = m_ramdomizeAllInputs;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            InputManager inputManager = FindObjectOfType<InputManager>();
            if(inputManager == null)
            {
                throw new UnassignedReferenceException("Missing input manager!");
            }
            if(ramdomizeAllInputs)
            {
                inputManager.SwapMovement(0);
                inputManager.SwapMovement(1);
                inputManager.SwapMovement(2);
                inputManager.SwapMovement(3);
            }
            else
            {
                inputManager.SwapRandomMovement();
            }
        }
    }
}
