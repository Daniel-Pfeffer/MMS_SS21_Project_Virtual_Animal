using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Jump_Run
{
    public class UIManagerJumpRun : MonoBehaviour
    {
        [SerializeField] private Text levelTextField;
        [SerializeField] private Text wayTextField;
        [SerializeField] private RectTransform healthRect;
        [SerializeField] private GameObject gameoverScreen;
        [SerializeField] private GameObject startupScreen;
        [SerializeField] private GameObject settingsScreen;

        [SerializeField] private Game.Slime slime;

        private bool started = false;

        public void Update()
        {
            // performance
            if (!started)
            {
                if (Input.anyKey)
                {
                    started = true;
                    slime.Play();
                    startupScreen.SetActive(false);
                }
            }
        }

        public void UpdateLevelDescription(int level)
        {
            levelTextField.text = $"Level {level}";
        }

        public void UpdateWayDescription(int maxWay, int currentWay)
        {
            int currway = 0;
            if (currentWay > 0)
            {
                currway = currentWay;
            }

            wayTextField.text = $"{currway}/{maxWay}";
        }

        public void GameOver()
        {
            gameoverScreen.SetActive(true);
        }

        public void UpdateHealth(int health)
        {
            healthRect.anchoredPosition = new Vector2(100.0f * health / 2 + 50, -100);
            healthRect.sizeDelta = new Vector2(100.0f * health, 100);
        }

        public void ReturnToHouse()
        {
            SceneManager.LoadScene("House", LoadSceneMode.Single);
        }

        public void TryAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ToggleSettings()
        {
            if (settingsScreen.active)
            {
                settingsScreen.SetActive(false);
                slime.Play();
            }
            else
            {
                slime.Stop();
                settingsScreen.SetActive(true);
            }
        }

        public void Resume()
        {
            settingsScreen.SetActive(false);
            slime.Play();
        }
    }
}