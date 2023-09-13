using HudNS;
using MyConstants;
using SerializableTypes;
using System.Collections.Generic;
using UnityEngine;
namespace LightNS
{
    public class LightGlowTimer : MonoBehaviour
    {
        [Header("Time")]
        [SerializeProperty("GlowTime"), SerializeField]
        float glowTime = 100f;
        [Min(0)]
        public float StartedGlowTime = 100f;
        public float GlowTime
        {
            get { return glowTime; }
            set { glowTime = value < 0 ? 0 : value > StartedGlowTime ? StartedGlowTime : value; CurrentTimeLeft = glowTime; }
        }
        static public float CurrentTimeLeft;
        static public float StartedTimeLeft;
        LightGlowTimer instance;
        static public Dictionary<int, bool> percentToShow = new Dictionary<int, bool>() { { 90,true },
            { 50, true }, { 25, true }, { 10,true }, { 5, true }, { 2, true }, { 1, true } };
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
            CurrentTimeLeft = GlowTime;
            StartedTimeLeft = StartedGlowTime;
        }
        static public void AddTime(float addedTime)
        {
            //Delete check below if you wanna unlimited time
            CurrentTimeLeft = CurrentTimeLeft + addedTime > StartedTimeLeft ? StartedTimeLeft : (CurrentTimeLeft + addedTime);
            //Uncomment below if you wanna unlimited time
            //StartedTimeLeft = CurrentTimeLeft;
            foreach (var key in new List<int>(percentToShow.Keys))
            {
                if (key < (int)CurrentTimeLeft)
                    percentToShow[key] = true;
            }
            ShowPercentOfLightLeft();
        }
        void Update()
        {
            CurrentTimeLeft -= 1 * Time.deltaTime;
            CurrentTimeLeft = CurrentTimeLeft < 0 ? 0 : CurrentTimeLeft;
        }
        void FixedUpdate()
        {
            int leftTime = (int)((CurrentTimeLeft * 100) / StartedTimeLeft);
            if (percentToShow.ContainsKey(leftTime) && percentToShow[leftTime])
            {
                percentToShow[leftTime] = false;
                ShowPercentOfLightLeft();
            }
        }
        static public void ShowPercentOfLightLeft() =>
            GameObject.FindAnyObjectByType<MessagePrint>().PrintMessage((int)((CurrentTimeLeft * 100) / StartedTimeLeft) + "%", 0.5f, HUDConstants.LIGHT_INTERES_LEFT);
    }
}