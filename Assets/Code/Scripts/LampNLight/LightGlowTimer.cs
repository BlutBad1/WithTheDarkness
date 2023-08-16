using HudNS;
using MyConstants;
using System.Collections.Generic;
using UnityEngine;
namespace LightNS
{
    public class LightGlowTimer : MonoBehaviour
    {
        [Header("Time")]
        [SerializeField]
        private float glowTime = 100f;
        static public float CurrentTimeLeft;
        static public float StartedTimeLeft;
        public void SetGlowTime(float time) =>
            glowTime = time;
        static public Dictionary<int, bool> percentToShow = new Dictionary<int, bool>() { { 90,true },
            { 50, true }, { 25, true }, { 10,true }, { 5, true }, { 2, true }, { 1, true } };
        private void Start()
        {
            CurrentTimeLeft = glowTime;
            StartedTimeLeft = glowTime;
        }
        static public void AddTime(float addedTime)
        {
            //Delete check below if wanna unlimited time
            CurrentTimeLeft = CurrentTimeLeft + addedTime > StartedTimeLeft ? StartedTimeLeft : (CurrentTimeLeft + addedTime);
            //Uncomment below if wanna unlimited time
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