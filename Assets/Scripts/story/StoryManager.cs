using System;
using System.IO;
using ScriptableObjects.Day;
using UnityEngine;

namespace story
{
    public class StoryManager : MonoBehaviour
    {
        //================================================================CLASSES
        
        [Serializable]
        public class News
        {
            public int day;
            public string Title;
            public string MainText;
            public string SecondaryTitle;
            public string SecondaryText;
        }

        [Serializable]
        public class NewsWrapper
        {
            public News[] news;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private DaySO[] days;

        //================================================================GETTER SETTER

        public DaySO GetDay()
        {
            return days[currentDaySOIndex];
        }

        public int GetDayNumber()
        {
            return currentDay;
        }

        public News GetNewsForDay()
        {
            foreach (News newsData in newsObjects.news)
            {
                if (newsData.day == currentDay)
                {
                    return newsData;
                }
            }
            return null;
        }

        public float GetRepairMult()
        {
            if (repairCostMult < 0)
            {
                repairCostMult = CalculateRepairMultiplier();
            }

            return repairCostMult;
        }
        
        public float GetRefuelMult()
        {
            if (refuelCostMult < 0)
            {
                refuelCostMult = CalculateRefuelMultiplier();
            }

            return refuelCostMult;
        }

        public int GetStartOfDayPayment()
        {
            if (startOfDayPayment < 0)
            {
                startOfDayPayment = CalculateStartOfDay();
            }
            return startOfDayPayment;
        }
        
        
        //================================================================FUNCTIONALITY
        public static StoryManager instance;
        
        private NewsWrapper newsObjects;
        private int currentDaySOIndex = -1;
        private int currentDay = 1;

        private float refuelCostMult = -1;
        private float repairCostMult = -1;
        private int startOfDayPayment = -1;

        public void IncrementDay()
        {
            currentDay++;
            if (currentDaySOIndex + 1 < days.Length && days[currentDaySOIndex + 1].dayNumber == currentDay)
            {
                currentDaySOIndex++;
                if(days[currentDaySOIndex].refuelCostMultiplier > 0)
                    refuelCostMult = days[currentDaySOIndex].refuelCostMultiplier;
                if(days[currentDaySOIndex].repairCostMultiplier > 0)
                    repairCostMult = days[currentDaySOIndex].repairCostMultiplier;
                if(days[currentDaySOIndex].startOfDayPayment > 0)
                    startOfDayPayment = days[currentDaySOIndex].startOfDayPayment;
                
            }
        }

        public void LoadDay(int dayNumber)
        {
            for (; currentDay < dayNumber;)
            {
                IncrementDay();
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                newsObjects = JsonUtility.FromJson<NewsWrapper>(File.ReadAllText("Assets/Json/News.json"));
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        
        public float CalculateRepairMultiplier()
        {
            if (days.Length <= currentDay) return 1;

            float mult = 1;
            for (int i = currentDaySOIndex; i >= 0; i--)
            {
                if (days[i].repairCostMultiplier > 0)
                {
                    mult = days[i].repairCostMultiplier;
                }
            }

            return mult;
        }

        public float CalculateRefuelMultiplier()
        {
            if (days.Length <= currentDay) return 1;

            float mult = 1;
            for (int i = currentDaySOIndex; i >= 0; i--)
            {
                if (days[i].refuelCostMultiplier > 0)
                {
                    mult = days[i].refuelCostMultiplier;
                }
            }

            return mult;
        }

        public int CalculateStartOfDay()
        {
            if (days.Length <= currentDay) return 1;

            int payment = 1;
            for (int i = currentDaySOIndex; i >= 0; i--)
            {
                if (days[i].startOfDayPayment > 0)
                {
                    payment = days[i].startOfDayPayment;
                }
            }

            return payment;
        }
    }
}
