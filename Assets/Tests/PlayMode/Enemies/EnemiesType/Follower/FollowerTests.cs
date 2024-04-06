using EntityNS.Type.Follower;
using NUnit.Framework;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.TestTools;
public class FollowerTests
{
    GameObject player;
    GameObject gameObject;
    Follower follower;
    Stopwatch stopwatch;
    [SetUp]
    public void Setup()
    {
        //Arrange
        player = GameObject.Instantiate(new GameObject());
        gameObject = GameObject.Instantiate(new GameObject());
        //player.gameObject.name = CommonConstantsNS.Constants.PLAYER;
        player.transform.position = new Vector3(0, 0, 10);
        stopwatch = new Stopwatch();
    }
    [UnityTest]
    public IEnumerator FollowTestOneThread_Expect_ChangedPosition()
    {
        //Arrange
        follower = gameObject.AddComponent<Follower>();
        follower.AddGameObject(gameObject);
        follower.FollowingDistance = 100;
        follower.StoppingDistance = 0;
        follower.FollowSpeed = 100;
        yield return new WaitForSeconds(0.1f);
        //Assert
        Assert.IsTrue(gameObject.transform.position == new Vector3(0, 0, 0));
    }
    [UnityTest]
    public IEnumerator FollowTestMultithreading_Expect_ChangedPosition()
    {
        //Arrange
        follower = gameObject.AddComponent<Follower>();
        follower.alwaysMultithreading = true;
        follower.AddGameObject(gameObject);
        follower.FollowingDistance = 100;
        follower.StoppingDistance = 0;
        follower.FollowSpeed = 100;
        yield return new WaitForSeconds(0.1f);
        //Assert
        Assert.IsTrue(gameObject.transform.position == new Vector3(0, 0, 10));
    }
    [UnityTest]
    public IEnumerator FollowTest_Compare_Multithreading_To_OneThread_Expect_OneThreadFaster_5()
    {
        //Arrange
        stopwatch.Reset();
        float onethreadTime = 0, multithreadTime = 0;
        follower = gameObject.AddComponent<Follower>();
        follower.FollowingDistance = 100;
        follower.StoppingDistance = 0;
        follower.FollowSpeed = 100;
        stopwatch.Start();
        for (int i = 0; i < 5; i++)
            follower.AddGameObject(gameObject);
        while (gameObject.transform.position == new Vector3(0, 0, 10)) ;
        yield return new WaitForSeconds(0.1f);
        stopwatch.Stop();
        onethreadTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();
        Object.Destroy(follower);
        player.transform.position = new Vector3(0, 0, 0);
        follower = gameObject.AddComponent<Follower>();
        follower.alwaysMultithreading = true;
        follower.FollowingDistance = 100;
        follower.StoppingDistance = 0;
        follower.FollowSpeed = 100;
        stopwatch.Start();
        for (int i = 0; i < 5; i++)
            follower.AddGameObject(gameObject);
        while (gameObject.transform.position == new Vector3(0, 0, 0)) ;
        yield return new WaitForSeconds(0.1f);
        stopwatch.Stop();
        multithreadTime = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log($"Multithreading time: {multithreadTime}\nOne thread time: {onethreadTime}\nTested by 5 objects.");
        Assert.True(onethreadTime < multithreadTime);
    }
    [UnityTest]
    public IEnumerator FollowTest_Compare_Multithreading_To_OneThread_Expect_MultithreadingFaster_100()
    {
        //Arrange
        stopwatch.Reset();
        float onethreadTime = 0, multithreadTime = 0;
        follower = gameObject.AddComponent<Follower>();
        follower.FollowingDistance = 100;
        follower.StoppingDistance = 0;
        follower.FollowSpeed = 100;
        stopwatch.Start();
        for (int i = 0; i < 100; i++)
            follower.AddGameObject(gameObject);
        while (gameObject.transform.position == new Vector3(0, 0, 10)) ;
        yield return new WaitForSeconds(0.1f);
        stopwatch.Stop();
        onethreadTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();
        Object.Destroy(follower);
        player.transform.position = new Vector3(0, 0, 0);
        follower = gameObject.AddComponent<Follower>();
        follower.alwaysMultithreading = true;
        follower.FollowingDistance = 100;
        follower.StoppingDistance = 0;
        follower.FollowSpeed = 100;
        stopwatch.Start();
        for (int i = 0; i < 100; i++)
            follower.AddGameObject(gameObject);
        while (gameObject.transform.position == new Vector3(0, 0, 0)) ;
        yield return new WaitForSeconds(0.1f);
        stopwatch.Stop();
        multithreadTime = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log($"Multithreading time: {multithreadTime}\nOne thread time: {onethreadTime}\nTested by 100 objects.");
        Assert.True(onethreadTime > multithreadTime);
    }
}
