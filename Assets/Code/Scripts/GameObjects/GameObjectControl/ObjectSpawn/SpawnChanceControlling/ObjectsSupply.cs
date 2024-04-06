using System;
using System.Linq;

namespace GameObjectsControllingNS
{
    /// <summary>
    /// "VeryLow" means that many times the objects did not appear, higher chances of a new object appearing.
    /// "VeryHigh" means that many times the objects appeared, lower chances of a new object appearing.
    /// </summary>
    public enum ObjectsSupplyLevel
    {
        VeryLow, Low, MediumLow, Medium, MediumHigh, High, VeryHigh
    }
    public class ObjectsSupply
    {
        protected static float ObjectSupplyLevelPercentage(ObjectsSupplyLevel level)
        {
            switch (level)
            {
                case ObjectsSupplyLevel.VeryLow:
                    return 1.6f;
                case ObjectsSupplyLevel.Low:
                    return 1.4f;
                case ObjectsSupplyLevel.MediumLow:
                    return 1.2f;
                case ObjectsSupplyLevel.Medium:
                    return 1f;
                case ObjectsSupplyLevel.MediumHigh:
                    return 0.8f;
                case ObjectsSupplyLevel.High:
                    return 0.6f;
                case ObjectsSupplyLevel.VeryHigh:
                    return 0.4f;
                default:
                    return 0;
            }
        }
        private ObjectsSupplyLevel objectSupplyLevel;
        protected virtual ObjectsSupplyLevel ObjectSupplyLevel { get { return objectSupplyLevel; } set { objectSupplyLevel = value; } }
        public virtual void SetHighestChance() =>
            objectSupplyLevel = ObjectsSupplyLevel.VeryLow;
        public virtual void SetLowestChance() =>
            objectSupplyLevel = ObjectsSupplyLevel.VeryHigh;
        public virtual void IncreaseObjectSupplyLevel() =>
            objectSupplyLevel = objectSupplyLevel == Enum.GetValues(typeof(ObjectsSupplyLevel)).Cast<ObjectsSupplyLevel>().Last() ? objectSupplyLevel : objectSupplyLevel + 1;
        public virtual void DecreaseObjectSupplyLevel() =>
             objectSupplyLevel = objectSupplyLevel == Enum.GetValues(typeof(ObjectsSupplyLevel)).Cast<ObjectsSupplyLevel>().First() ? objectSupplyLevel : objectSupplyLevel - 1;
        public virtual bool CalculateObjectChances(float objectSpawnChance)
        {
            if (objectSpawnChance >= 100f)
                return true;
            if (objectSpawnChance <= 0f)
                return false;
            objectSpawnChance *= ObjectSupplyLevelPercentage(ObjectSupplyLevel);
            if (objectSpawnChance > UnityEngine.Random.Range(0, 100))
            {
                DecreaseObjectSupplyLevel();
                return true;
            }
            else
            {
                IncreaseObjectSupplyLevel();
                return false;
            }
        }
    }
}
