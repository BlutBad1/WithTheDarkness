using LightNS;

namespace GameObjectsControllingNS
{
    public class OilSupply : ObjectsSupply
    {
        public override bool CalculateObjectChances(float objectSpawnChance)
        {
            // (100% - TimeInPercentLeft)/2
            float chancesByOilLeft = (100 - (LightGlowTimer.CurrentTimeLeft * 100 / LightGlowTimer.StartedTimeLeft)) / 2;
            objectSpawnChance += chancesByOilLeft;
            return base.CalculateObjectChances(objectSpawnChance);
        }
    }
}