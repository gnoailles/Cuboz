using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool m_useStartZone = true;

    public void LoadScene(string p_level)
    {
        if (m_useStartZone)
        {
            StartZone.m_sceneToLoad = p_level;
            SceneManager.LoadScene("StartZone");
        }
        else
        {
            SceneManager.LoadScene(p_level);
        }
    }
}
