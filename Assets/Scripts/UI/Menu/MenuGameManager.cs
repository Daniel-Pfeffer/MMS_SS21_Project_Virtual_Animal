using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject howToPlay;

    public void StartGame()
    {
        SceneManager.LoadScene("House", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowHowToPlay()
    {
        mainMenu.SetActive(false);
        howToPlay.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlay.SetActive(false);
        mainMenu.SetActive(true);
    }
}