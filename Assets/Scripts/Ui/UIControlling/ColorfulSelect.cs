using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UINS.Settings
{
    public class ColorfulSelect : MonoBehaviour
    {
        public Color DefaultColor = Color.white;
        public Color SelectedColor = Color.white;
        public MaskableGraphic[] ElementsOfSelection;
        public void SelectElement(MaskableGraphic el) =>
            SelectElement(Array.FindIndex(ElementsOfSelection, x => x == el));
        public void SelectElement(int index)
        {
            if (ElementsOfSelection == null)
                return;
            if (index >= 0 && index < ElementsOfSelection.Length)
            {
                ElementsOfSelection[index].color = SelectedColor;
                for (int i = 0; i < ElementsOfSelection.Length; i++)
                {
                    if (index != i)
                        ElementsOfSelection[i].color = DefaultColor;
                }
            }
        }
        public void ResetAll()
        {
            foreach (var item in ElementsOfSelection)
                item.color = DefaultColor;
        }
    }
}