using UnityEngine;

public class LightGlowTimer : MonoBehaviour
{

    [Header("Time")]
    [SerializeField]
    private float glowTime = 100f;
    static public float CurrentTimeLeft;
    static public float StartedTimeLeft;
   public void SetGlowTime(float time)
    {
        glowTime = time;
    }
    private void Start()
    {
        CurrentTimeLeft = glowTime;
        StartedTimeLeft = glowTime;
    }
   static public void AddTime(float addedTime)
    {
        CurrentTimeLeft += addedTime;
        StartedTimeLeft = CurrentTimeLeft;
    }
    void Update()
    {
        CurrentTimeLeft -= 1 * Time.deltaTime;

        CurrentTimeLeft = CurrentTimeLeft < 0 ? 0 : CurrentTimeLeft;

    }
}