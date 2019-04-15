using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool m_useStartZone = true;

    public void LoadScene(string p_level)
    {
        if(!p_level.Contains("Level"))
        {
            InputManager inputManager = FindObjectOfType<InputManager>();
            if (inputManager)
            {
                inputManager.SetTestInputs();
                Destroy(inputManager.gameObject);
            }
            PlayerHUD HUD = FindObjectOfType<PlayerHUD>();
            if (HUD)
                Destroy(HUD.gameObject);
        }
        if (m_useStartZone)
        {
            StartZone.sceneToLoad = p_level;
            SceneManager.LoadScene("StartZone");
        }
        else
        {
            SceneManager.LoadScene(p_level);
        }
    }
}
