using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIControlling
{
    public class SliderInputBox : MonoBehaviour
    {
        public Slider Slider;
        public TMP_InputField InputField;
        float ValueMax;
        float ValueMin;
        protected void Awake()
        {
            ValueMax = Slider.maxValue;
            ValueMin = Slider.minValue;
#if UNITY_EDITOR
            if (!InputField || !Slider)
                Debug.LogError("Some components weren't assign!");
#endif
            else
                ChangeInputFieldBySlider();
        }
        public virtual void CheckRange()
        {
            if (float.TryParse(InputField.text, out float result))
                InputField.text = result > ValueMax ? ValueMax.ToString() : result < ValueMin ? ValueMin.ToString() : result.ToString();
            else
                InputField.text = ValueMin.ToString();
        }
        public virtual void ChangeInputFieldBySlider() =>
             InputField.text = Slider.value.ToString();
        public virtual void ChangeSliderByInputField()
        {
            CheckRange();
            if (float.TryParse(InputField.text, out float result))
                Slider.value = result;
        }
    }
}