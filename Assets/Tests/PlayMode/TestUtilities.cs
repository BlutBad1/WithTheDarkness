using UnityEngine;
namespace TestsNS
{
    public class TestUtilities 
    {
       public static Vector3 GetRandomPosition()
        {
            return new Vector3(Random.Range(-100000, 100000), Random.Range(-100000, 100000), Random.Range(-100000, 100000));
        }
    }
}
