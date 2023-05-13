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
        MapData mapData;
        Stopwatch stopwatch;
      [SetUp]
        public void Setup()
        {
            //Arrange

            gameObject = GameObject.Instantiate(new GameObject());
            placeholder = GameObject.Instantiate(new GameObject());
            mapData = gameObject.AddComponent<MapData>();
            placeholder.AddComponent<TeleportTrigger>();
            stopwatch = new Stopwatch();
        }

        [UnityTest]
        public IEnumerator ShuffleLocationsTest_Expect_ShuffledLocations()
        {
            //Arrange 
            for (int i = 0; i < 5; i++)
                mapData.AddNewLocation(new Location { SpawnChance = 100, MapData = placeholder, EntryTeleportTrigger = placeholder.GetComponent<TeleportTrigger>() });
            //Act
            mapData.ShuffleLocations();
            yield return new WaitForSeconds(0.5f);
            //Assert
            Assert.True(mapData.LocationsArr.Length > 0);
        }
        [UnityTest]
        public IEnumerator ShuffleLocationsTest_Multithreading_Expect_ShuffledLocations()
        {
            //Arrange 
            for (int i = 0; i < 5; i++)
                mapData.AddNewLocation(new Location { SpawnChance = 100, MapData = placeholder, EntryTeleportTrigger = placeholder.GetComponent<TeleportTrigger>() });
            //Act
            mapData.alwaysMultithreading = true;
            mapData.ShuffleLocations();
            yield return new WaitForSeconds(0.5f);
            //Assert
            Assert.True(mapData.LocationsArr.Length > 0);
        }
        [UnityTest]
        public IEnumerator ShuffleLocationsTest_Compare_Multithreading_To_OneThread_Expect_OneThreadFaster_5()
        {
            stopwatch.Reset();
            //Arrange 
            float onethreadTime = 0, multithreadTime = 0;
            for (int i = 0; i < 5; i++)
                mapData.AddNewLocation(new Location { SpawnChance = 100, MapData = placeholder, EntryTeleportTrigger = placeholder.GetComponent<TeleportTrigger>() });
            //Act
            stopwatch.Start();
            mapData.ShuffleLocations();
            stopwatch.Stop();
            yield return new WaitForSeconds(0.5f);
            onethreadTime = stopwatch.ElapsedMilliseconds;
            mapData.alwaysMultithreading = true;
            stopwatch.Restart();
            stopwatch.Start();
            mapData.ShuffleLocations();
            stopwatch.Stop();
            yield return new WaitForSeconds(0.5f);
            multithreadTime = stopwatch.ElapsedMilliseconds;
            //Assert
            UnityEngine.Debug.Log($"Multithreading time: {multithreadTime}\nOne thread time: {onethreadTime}\nTested by 5 locations.");
            Assert.True(onethreadTime< multithreadTime);
        }
        [UnityTest]
        public IEnumerator ShuffleLocationsTest_Compare_Multithreading_To_OneThread_Expect_MultithreadingFaster_1000()
        {
            stopwatch.Reset();
            //Arrange 
            float onethreadTime = 0, multithreadTime = 0;
            for (int i = 0; i < 1000; i++)
                mapData.AddNewLocation(new Location { SpawnChance = 100, MapData = placeholder, EntryTeleportTrigger = placeholder.GetComponent<TeleportTrigger>() });
            //Act
            mapData.automaticallyEnableAfter = -1;
            stopwatch.Start();
            mapData.ShuffleLocations();
            stopwatch.Stop();
            yield return new WaitForSeconds(0.5f);
            onethreadTime = stopwatch.ElapsedMilliseconds;
            mapData.alwaysMultithreading = true;
            stopwatch.Restart();
            mapData.ShuffleLocations();
            stopwatch.Stop();
            yield return new WaitForSeconds(0.5f);
            multithreadTime = stopwatch.ElapsedMilliseconds;
            //Assert
            UnityEngine.Debug.Log($"Multithreading time: {multithreadTime}\nOne thread time: {onethreadTime}\nTested by 1000 locations.");
            Assert.True(onethreadTime > multithreadTime);
        }
        [UnityTest]
        public IEnumerator ShuffleLocationsTest_Compare_Multithreading_To_OneThread_Expect_MultithreadingFaster_10000()
        {
            stopwatch.Reset();
            //Arrange 
            float onethreadTime = 0, multithreadTime = 0;
            for (int i = 0; i < 10000; i++)
                mapData.AddNewLocation(new Location { SpawnChance = 100, MapData = placeholder, EntryTeleportTrigger = placeholder.GetComponent<TeleportTrigger>() });
            //Act
            mapData.automaticallyEnableAfter = -1;
            stopwatch.Start();
            mapData.ShuffleLocations();
            stopwatch.Stop();
            yield return new WaitForSeconds(0.5f);
            onethreadTime = stopwatch.ElapsedMilliseconds;
            mapData.alwaysMultithreading = true;
            stopwatch.Restart();
            mapData.ShuffleLocations();
            stopwatch.Stop();
            yield return new WaitForSeconds(0.5f);
            multithreadTime = stopwatch.ElapsedMilliseconds;
            //Assert
            UnityEngine.Debug.Log($"Multithreading time: {multithreadTime}\nOne thread time: {onethreadTime}\nTested by 10000 locations.");
            Assert.True(onethreadTime > multithreadTime);
        }
    }
}