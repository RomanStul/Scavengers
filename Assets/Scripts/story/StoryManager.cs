using System;
using System.IO;
using ScriptableObjects.Day;
using ScriptableObjects.Item;
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

        public float GetOreMultiplier(ItemSO.Items ore)
        {
            return oreCostMultipliers[(int)ore];
        }

        public int GetEvacuateCost()
        {
            return evacuateCost;
        }
        
        
        
        
        //================================================================FUNCTIONALITY
        public static StoryManager instance;
        
        private NewsWrapper newsObjects;
        private int currentDaySOIndex = -1;
        private int currentDay = 1;

        private float refuelCostMult = 1;
        private float repairCostMult = 1;
        private int evacuateCost = 0;
        private int startOfDayPayment = 1;
        private float[] oreCostMultipliers;

        public void IncrementDay()
        {
            currentDay++;
            if (currentDaySOIndex + 1 < days.Length && days[currentDaySOIndex + 1].dayNumber == currentDay)
            {
                currentDaySOIndex++;
                if(days[currentDaySOIndex].refuelCostMultiplier > 0)
                    refuelCostMult *= days[currentDaySOIndex].refuelCostMultiplier;
                if(days[currentDaySOIndex].repairCostMultiplier > 0)
                    repairCostMult *= days[currentDaySOIndex].repairCostMultiplier;
                if(days[currentDaySOIndex].startOfDayPayment > 0)
                    startOfDayPayment += days[currentDaySOIndex].startOfDayPayment;
                if(days[currentDaySOIndex].evacuateCost > 0)
                    evacuateCost += days[currentDaySOIndex].evacuateCost;
                foreach (OrePriceChanges opc in days[currentDaySOIndex].priceChanges)
                {
                    oreCostMultipliers[(int)opc.item] *= opc.multiplier;
                }
                
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
                oreCostMultipliers = new float[Enum.GetValues(typeof(ItemSO.Items)).Length];
                for (int i = 0; i < oreCostMultipliers.Length; i++)
                {
                    oreCostMultipliers[i] = 1;
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        
        public float CalculateRepairMultiplier()
        {
            Debug.Log("calculate");
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
            Debug.Log("calculate");
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
            Debug.Log("calculate");
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
