using HUDConstantsNS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LightNS
{
	public class LightGlowTimer : MonoBehaviour
	{
		private static Dictionary<int, bool> showingPercents = new Dictionary<int, bool>() { { 90,true },
			{ 50, true }, { 25, true }, { 10,true }, { 5, true }, { 2, true }, { 1, true } };

		[Header("Time")]
		[SerializeField, FormerlySerializedAs("CurrentTimeLeftToGlow"), Min(0)]
		private float currentTimeLeftToGlow = 100f;
		[SerializeField, FormerlySerializedAs("MaxTimeOfGlowing"), Min(0)]
		private float maxTimeOfGlowing = 100f;

		public float CurrentTimeLeftToGlow { get => currentTimeLeftToGlow; set => currentTimeLeftToGlow = value; }
		public float MaxTimeOfGlowing { get => maxTimeOfGlowing; set => maxTimeOfGlowing = value; }

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
		private void ShowPercentOfLightLeft()
		{
			HUDConstants.LightPercentPrinter.PrintMessage((int)GetGlowingLeftTimeInPercantage() + "%", gameObject);
		}
	}
}