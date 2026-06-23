using System;
using System.IO;
using Entities.Environment;
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
            return repairCostMult;
        }
        
        public float GetRefuelMult()
        {
            return refuelCostMult;
        }

        public int GetStartOfDayPayment()
        {
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
        private int startOfDayPayment = 0;
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
            
            DestructionManager.instance.PushDaySetToPermanent();
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
                newsObjects = JsonUtility.FromJson<NewsWrapper>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Json/News.json")));
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
    }
}
