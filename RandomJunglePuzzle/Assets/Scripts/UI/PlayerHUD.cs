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
        if(LevelManager.Instance)
        { 
            m_timerText.text = LevelManager.Instance.LevelTimer.ToString();
            m_pauseMenu.SetActive(LevelManager.Instance.IsPaused);
        }
        else
        {
            if (Input.GetButton("Pause"))
                m_pauseMenu.SetActive(true);
        }
    }
}
