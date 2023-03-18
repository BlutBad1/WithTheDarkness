using UnityEngine;

public class LightsTimers : MonoBehaviour
{
    [Header("Time")]
    public float time;
    [HideInInspector]
    public float StartingTime;
    private void Start()
    {
        StartingTime = time;
    }
    public void AddTime(float time)
    {
        this.time += time;
        StartingTime = this.time;
    }
    void Update()
    {
        time -= 1 * Time.deltaTime;
      
        time = time < 0 ? 0 : time;



    }
}