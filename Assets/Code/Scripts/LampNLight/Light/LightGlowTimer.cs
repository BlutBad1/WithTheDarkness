using HudNS;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace LightNS
{
    public class LightGlowTimer : MonoBehaviour
    {
        [Header("Time")]
        [Min(0)]
        public float CurrentTimeLeftToGlow = 100f;
        [Min(0)]
        public float MaxTimeOfGlowing = 100f;
        public TextMeshProUGUI Showcaser;

        static private Dictionary<int, bool> showingPercents = new Dictionary<int, bool>() { { 90,true },
            { 50, true }, { 25, true }, { 10,true }, { 5, true }, { 2, true }, { 1, true } };
        private void Awake()
        {
            CurrentTimeLeftToGlow = CurrentTimeLeftToGlow > MaxTimeOfGlowing ? MaxTimeOfGlowing : CurrentTimeLeftToGlow;
        }
        private void Update()
        {
            CurrentTimeLeftToGlow -= 1 * Time.deltaTime;
            CurrentTimeLeftToGlow = CurrentTimeLeftToGlow < 0 ? 0 : CurrentTimeLeftToGlow;
        }
        private void FixedUpdate()
        {
            int leftTime = (int)GetGlowingLeftTimeInPercantage();
            if (showingPercents.ContainsKey(leftTime) && showingPercents[leftTime])
            {
                showingPercents[leftTime] = false;
                ShowPercentOfLightLeft();
            }
        }
        public void AddTime(float addedTime)
        {
            //Delete check below if you wanna unlimited time
            CurrentTimeLeftToGlow = CurrentTimeLeftToGlow + addedTime > MaxTimeOfGlowing ? MaxTimeOfGlowing : (CurrentTimeLeftToGlow + addedTime);
            //Uncomment below if you wanna unlimited time
            //StartedTimeLeft = CurrentTimeLeft;
            foreach (var key in new List<int>(showingPercents.Keys))
            {
                if (key < (int)CurrentTimeLeftToGlow)
                    showingPercents[key] = true;
            }
            ShowPercentOfLightLeft();
        }
        public float GetGlowingLeftTimeInPercantage() =>
            (CurrentTimeLeftToGlow * 100 / MaxTimeOfGlowing);
        public void ShowPercentOfLightLeft() =>
           GameObject.FindAnyObjectByType<MessagePrint>().PrintMessage((int)GetGlowingLeftTimeInPercantage() + "%", 0.5f, Showcaser);
    }
}