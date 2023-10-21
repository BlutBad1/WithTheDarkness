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
        [SerializeProperty("GlowTime"), SerializeField]
        private float glowTime = 100f;
        [Min(0)]
        public float MaxGlowTime = 100f;
        public TextMeshProUGUI Showcaser;
        public float GlowTime
        {
            get { return glowTime; }
            set { glowTime = value < 0 ? 0 : value > MaxGlowTime ? MaxGlowTime : value; CurrentTimeLeft = glowTime; }
        }
        [HideInInspector]
        public float CurrentTimeLeft;
        [HideInInspector]
        public float MaxTimeLeft;
        static public Dictionary<int, bool> percentToShow = new Dictionary<int, bool>() { { 90,true },
            { 50, true }, { 25, true }, { 10,true }, { 5, true }, { 2, true }, { 1, true } };
        private void Awake()
        {
            CurrentTimeLeft = GlowTime;
            MaxTimeLeft = MaxGlowTime;
        }
        private void Update()
        {
            CurrentTimeLeft -= 1 * Time.deltaTime;
            CurrentTimeLeft = CurrentTimeLeft < 0 ? 0 : CurrentTimeLeft;
        }
        private void FixedUpdate()
        {
            int leftTime = (int)((CurrentTimeLeft * 100) / MaxTimeLeft);
            if (percentToShow.ContainsKey(leftTime) && percentToShow[leftTime])
            {
                percentToShow[leftTime] = false;
                ShowPercentOfLightLeft();
            }
        }
        public void AddTime(float addedTime)
        {
            //Delete check below if you wanna unlimited time
            CurrentTimeLeft = CurrentTimeLeft + addedTime > MaxTimeLeft ? MaxTimeLeft : (CurrentTimeLeft + addedTime);
            //Uncomment below if you wanna unlimited time
            //StartedTimeLeft = CurrentTimeLeft;
            foreach (var key in new List<int>(percentToShow.Keys))
            {
                if (key < (int)CurrentTimeLeft)
                    percentToShow[key] = true;
            }
            ShowPercentOfLightLeft();
        }
        public void ShowPercentOfLightLeft() =>
           GameObject.FindAnyObjectByType<MessagePrint>().PrintMessage((int)((CurrentTimeLeft * 100) / MaxTimeLeft) + "%", 0.5f, Showcaser);
    }
}