using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public const double gameSpeed = 1;
    public const double BiteSize = 0.05 * gameSpeed;
    public const double SipSize = 0.05 * gameSpeed;
    private double tickRate = 0.01 * gameSpeed;

    [NonSerialized] public double health = 100;
    [NonSerialized] public double hunger = 100;
    [NonSerialized] public double thirst = 100;
    [NonSerialized] public double happiness = 100;
    [NonSerialized] public double energy = 100;
    [NonSerialized] public double savefeeling = 100;
    [NonSerialized] public double stresslevel = 100;
    [NonSerialized] public double toilet = 100;

    [NonSerialized] public bool dead = false;
    [NonSerialized] public bool eating = false;
    [NonSerialized] public bool drinking = false;
    [NonSerialized] public bool sleeping = false;
    [NonSerialized] public bool pooping = false;

    public bool IsPaused { get; private set; } = false;

    public Bar healthBar;
    public Bar thirstBar;
    public Bar hungerBar;
    public Bar happinessBar;
    public Bar tirednessBar;
    public Bar toiletBar;

    public WaterDispenser water;
    public FeedingDish food;
    public Lights lights;

    public Animator animator;

    [SerializeField] private AudioClip dyingSound;

    [SerializeField] private AudioClip illSound;

    [SerializeField] private AudioClip sleepingSound;

    [SerializeField] private AudioClip feedingSound;

    [SerializeField] private AudioClip hungrySound;

    [SerializeField] private AudioClip thirstySound;

    [SerializeField] private AudioClip drinkingSound;

    [SerializeField] private AudioClip boredSound;

    [SerializeField] private AudioClip tiredSound;

    [SerializeField] private AudioClip scaredSound;


    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SaveState saveState = SaveState.GetInstance();
        if (saveState.HasSaved())
        {
            Dictionary<string, double> save = saveState.GetSlimeData();
            health = save["health"];
            hunger = save["hunger"];
            thirst = save["thirst"];
            toilet = save["toilet"];
            happiness = Math.Min(save["happiness"] + saveState.GetLevel() * 10, 100);
            energy = Math.Max(save["tiredness"] - saveState.GetLevel() * 10, 0);
        }

        healthBar.setDescription("Health");
        thirstBar.setDescription("Water");
        hungerBar.setDescription("Food");
        happinessBar.setDescription("Happiness");
        tirednessBar.setDescription("Energy");
        toiletBar.setDescription("Toilet");
    }

    void Update()
    {
        if (IsPaused)
        {
            return;
        }

        AudioClip playing = null;
        if (health < 25)
        {
            playing = illSound;
            // play sad sounds
            if (health <= 0)
            {
                dead = true;
                playing = dyingSound;
                audioSource.Stop();
                audioSource.clip = playing;
                audioSource.Play();
                return;
            }
        }


        // Every-Frame-Updates
        if (hunger > 0 && !eating)
        {
            hunger -= tickRate;
            hungerBar.UpdateBar(hunger);
        }

        if (thirst > 0 && !drinking)
        {
            thirst -= tickRate;
            thirstBar.UpdateBar(thirst);
        }

        if (energy > 0 && !sleeping)
        {
            energy -= tickRate;
            tirednessBar.UpdateBar(energy);
        }

        if (savefeeling > 0 && !(sleeping || eating || drinking))
        {
            playing = boredSound;
            savefeeling -= tickRate;
        }

        if (toilet > 0 && !pooping)
        {
            toilet -= tickRate;
            toiletBar.UpdateBar(toilet);
        }

        // Dependent updates
        // eat state update
        AudioClip returnClip = CheckAndApplyHunger();
        if (returnClip != null)
        {
            playing = returnClip;
        }

        // drink state update
        returnClip = CheckAndApplyThirst();
        if (returnClip != null)
        {
            playing = returnClip;
        }

        // sleep state update
        returnClip = CheckAndApplyEnergy();
        if (returnClip != null)
        {
            playing = returnClip;
        }

        // pet doesn't like to be in the dark
        if (!lights.lightsOn && !sleeping && stresslevel > 0)
        {
            playing = scaredSound;
            stresslevel -= tickRate;
        }

        if (hunger <= 45 || thirst <= 45 || energy <= 45)
        {
            stresslevel -= tickRate;
        }

        // Deteriorate at 25 for any
        if (hunger <= 25 || thirst <= 25 || energy <= 25 || happiness < 25 || health <= 25)
        {
            health -= tickRate;
            healthBar.UpdateBar(health);
        }


        UpdateHappiness();

        if (playing != null && !audioSource.isPlaying)
        {
            audioSource.clip = playing;
            audioSource.Play();
        }
    }

    public void UpdateHappiness()
    {
        happiness = (stresslevel + savefeeling) / 2;
        happinessBar.UpdateBar(happiness);
    }

    public void GiveTreats()
    {
        if (thirst < 5 || hunger >= 100 || savefeeling >= 100)
        {
            return;
        }

        savefeeling += 10;
        if (savefeeling > 100)
        {
            savefeeling = 100;
        }

        hunger += 5;
        if (hunger > 100)
        {
            hunger = 100;
        }


        thirst -= 5;
        if (thirst < 0)
        {
            thirst = 0;
        }
    }

    public AudioClip CheckAndApplyHunger()
    {
        AudioClip playing = null;
        if (eating || hunger < 50) // if the Pet is eating it continues until its full
        {
            playing = hungrySound;
            if (hunger < 100 && food.foodFill > 0)
            {
                eating = true;
                if (food.foodFill < BiteSize)
                {
                    playing = feedingSound;
                    hunger += food.foodFill;
                    food.foodFill = 0;
                    eating = false; // Pet stops eating because no food left
                }
                else
                {
                    playing = feedingSound;
                    food.foodFill -= BiteSize;
                    hunger += BiteSize;
                }
            }

            if (hunger >= 100)
            {
                playing = null; // pet not eating nor hungry
                eating = false;
                hunger = 100;
            }

            hungerBar.UpdateBar(hunger);
        }

        return playing;
    }

    public AudioClip CheckAndApplyThirst()
    {
        AudioClip playing = null;
        if (drinking || thirst < 50)
        {
            playing = thirstySound;
            if (thirst < 100 && water.waterFill > 0)
            {
                drinking = true;
                if (water.waterFill < SipSize)
                {
                    playing = drinkingSound;
                    thirst += water.waterFill;
                    water.waterFill = 0;
                    drinking = false; // Pet stops drinking because no water left
                }
                else
                {
                    playing = drinkingSound;
                    water.waterFill -= SipSize;
                    thirst += SipSize;
                }
            }

            if (thirst >= 100)
            {
                playing = null; // pet not drinking nor thirsty
                drinking = false;
                thirst = 100;
            }

            thirstBar.UpdateBar(thirst);
        }

        return playing;
    }

    private AudioClip CheckAndApplyEnergy()
    {
        AudioClip playing = null;
        if (sleeping || energy < 50)
        {
            playing = tiredSound;
            if (energy < 100 && !lights.lightsOn)
            {
                playing = sleepingSound;
                sleeping = true;
                energy += 10 * tickRate;
            }
            else
            {
                sleeping = false;
            }

            if (energy > 100)
            {
                playing = null;
                energy = 100;
            }

            tirednessBar.UpdateBar(energy);
        }

        return playing;
    }

    public void Save()
    {
        SaveState.GetInstance().SaveSlime(this);
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
    }
}