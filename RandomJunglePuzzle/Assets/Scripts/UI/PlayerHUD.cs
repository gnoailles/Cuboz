using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Text m_timerText;
    [SerializeField] private GameObject m_pauseMenu;

    void Awake()
    {
        PlayerHUD[] objs = FindObjectsOfType<PlayerHUD>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        m_timerText.gameObject.SetActive(LevelManager.Instance);

        if(LevelManager.Instance)
        { 
            m_timerText.text = LevelManager.Instance.LevelTimer.ToString("F2");
            m_pauseMenu.SetActive(LevelManager.Instance.IsPaused);
        }
        else
        {
            if (Input.GetButtonDown("Pause"))
            {
                InputManager[] objs = FindObjectsOfType<InputManager>();
                objs[0].isPaused = !m_pauseMenu.activeSelf;
                m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
            }
        }
    }
}
