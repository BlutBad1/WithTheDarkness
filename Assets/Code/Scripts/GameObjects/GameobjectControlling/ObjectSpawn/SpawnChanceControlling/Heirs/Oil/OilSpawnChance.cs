using LightNS;

namespace GameObjectsControllingNS
{
    public class OilSpawnChance : ObjectSpawnChance<OilSpawnChance, OilSupply>
    {
        protected override void AssignSupplyType()
        {
            base.AssignSupplyType();
            OilSupply oilSupply = (OilSupply)objectsSupplyInstance;
            oilSupply.LightGlowTimer = UtilitiesNS.Utilities.GetClosestComponent<LightGlowTimer>(transform.position);
        }
    }
}