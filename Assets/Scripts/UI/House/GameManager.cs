using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Slime slime;
    [SerializeField] private WaterDispenser water;
    [SerializeField] private FeedingDish food;
    [SerializeField] private Lights lights;

    [SerializeField] private GameObject pauseScreen;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            slime.TogglePause();
            if (slime.IsPaused)
            {
                pauseScreen.SetActive(true);
            }
            else
            {
                pauseScreen.SetActive(false);
            }
        }
    }

    public void SelectWater()
    {
        water.waterFill = 100;
    }

    public void SelectMeat()
    {
        food.foodFill = 100;
    }

    public void SelectTreats()
    {
        slime.GiveTreats();
    }

    public void SelectSleep()
    {
        lights.lightsOn = !lights.lightsOn;
    }

    public void SelectPill()
    {
        slime.health = 100;
    }

    public void SelectToilet()
    {
        slime.toilet = 100;
    }

    public void GameTime()
    {
        if (slime.energy < 25)
        {
            return;
        }

        slime.Save();
        SceneManager.LoadScene("Game_Jump", LoadSceneMode.Single);
    }

    public void Resume()
    {
        slime.TogglePause();
        pauseScreen.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}