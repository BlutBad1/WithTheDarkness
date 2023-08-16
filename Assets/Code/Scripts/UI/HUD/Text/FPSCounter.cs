using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HudNS
{


public class FPSCounter : MonoBehaviour
{

    public float updateInterval = 0.5f; 
    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;
    // Use this for initialization
    void Start()
    {
        timeleft = updateInterval;

    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
            GetComponent<TextMeshProUGUI>().text = fps.ToString("F0") + " FPS";
        }
    }
}
}