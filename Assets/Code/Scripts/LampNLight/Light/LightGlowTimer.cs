using HudNS;
using SerializableTypes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace LightNS
{
    public class LightGlowTimer : MonoBehaviour
    {
        [Header("Time")]
        [SerializeProperty("CurrentTimeLeftToGlow"), SerializeField, Min(0)]
        private float currentTimeLeftToGlow = 100f;
        [Min(1)]
        public float MaxTimeOfGlowing = 100f;
        public TextMeshProUGUI Showcaser;
        public float CurrentTimeLeftToGlow
        {
            get { return currentTimeLeftToGlow; }
            set { currentTimeLeftToGlow = value < 0 ? 0 : value > MaxTimeOfGlowing ? MaxTimeOfGlowing : value; }
        }
        static private Dictionary<int, bool> showingPercents = new Dictionary<int, bool>() { { 90,true },
            { 50, true }, { 25, true }, { 10,true }, { 5, true }, { 2, true }, { 1, true } };
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