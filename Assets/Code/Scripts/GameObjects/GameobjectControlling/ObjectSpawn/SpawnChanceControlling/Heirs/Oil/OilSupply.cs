using LightNS;

namespace GameObjectsControllingNS
{
    public class OilSupply : ObjectsSupply
    {
        public LightGlowTimer LightGlowTimer { get; set; }
        public override bool CalculateObjectChances(float objectSpawnChance)
        {
            // (100% - TimeInPercentLeft)/2
            if(LightGlowTimer)
            {
                float chancesByOilLeft = (100 - LightGlowTimer.GetGlowingLeftTimeInPercantage()) / 2;
                objectSpawnChance += chancesByOilLeft;
            }
            return base.CalculateObjectChances(objectSpawnChance);
        }
    }
}