using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpauseOnClick : MonoBehaviour
{
    public void Unpause()
    {
        if (LevelManager.Instance)
        {
            LevelManager.Instance.SetPaused(false);
        }
        else
        {
            InputManager[] objs = FindObjectsOfType<InputManager>();
            objs[0].shouldUnpause = true;
        }
    }
}
