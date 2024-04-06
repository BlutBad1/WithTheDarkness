using LocationManagementNS;
using NUnit.Framework;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.TestTools;
namespace Map
{
    public class MapDataTests
    {
        GameObject gameObject;
        GameObject placeholder;
        LocationsSpawnController mapData;
        Stopwatch stopwatch;
        [SetUp]
        public void Setup()
        {
            //Arrange

            gameObject = GameObject.Instantiate(new GameObject());
            placeholder = GameObject.Instantiate(new GameObject());
            mapData = gameObject.AddComponent<LocationsSpawnController>();
            placeholder.AddComponent<BlackScreenTeleportTrigger>();
            stopwatch = new Stopwatch();
        }

       
    }
}