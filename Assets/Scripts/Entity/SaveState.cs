using System;
using System.Collections.Generic;

namespace Entity
{
    public class SaveState
    {
        private static SaveState instance;
        public double health;
        public double hunger;
        public double thirst;
        public double happiness;
        public double tiredness;
        public double toilet;
        public int levelCount;
        private bool hasSaved = false;

        private SaveState()
        {
        }

        public static SaveState GetInstance()
        {
            if (instance == null)
            {
                instance = new SaveState();
            }

            return instance;
        }

        public void SaveSlime(Slime slime)
        {
            health = slime.health;
            hunger = slime.hunger;
            thirst = slime.thirst;
            happiness = slime.happiness;
            tiredness = slime.energy;
            toilet = slime.toilet;
            hasSaved = true;
        }

        public void SaveLevel(int level)
        {
            levelCount = level;
        }

        public int GetLevel()
        {
            int toRet = levelCount;
            levelCount = 0;
            return toRet;
        }

        public bool HasSaved()
        {
            return hasSaved;
        }

        public Dictionary<string, double> GetSlimeData()
        {
            Dictionary<string, double> dict =
                new Dictionary<string, double>
                {
                    {"hunger", hunger},
                    {"health", health},
                    {"thirst", thirst},
                    {"happiness", happiness},
                    {"tiredness", tiredness},
                    {"toilet", toilet}
                };
            hasSaved = false;
            return dict;
        }
    }
}