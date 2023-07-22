using SettingsNS;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UIControlling
{
    enum RebindingTypes
    {
        Button, Composite
    }
    public class KeyRebinding : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text rebindingKeyText;
        [SerializeField]
        private InputActionReference rebindingActionReference;
        private InputAction rebindingAction;
        private bool changing = false;
        [SerializeField]
        private string excludeKeys;
        private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
        [Min(0)]
        public int BindingIndex;
        [SerializeField]
        private RebindingTypes type;
        private string oldBindPath;
        private void Start()
        {
            DefineAction();
            GameSettings.OnKeyRebind += DisplayKey;
            DisplayKey();
        }
        public void DefineAction() =>
            rebindingAction = GameSettings.PlayerInput.FindAction(rebindingActionReference.name);
        public void StartRebinding()
        {
            if (!changing)       //If a button is being rebinded
            {
                changing = true;
                rebindingKeyText.text = "...";
                DefineAction();
                oldBindPath = rebindingAction.bindings[GetBindingIndex()].effectivePath;
                if (type == RebindingTypes.Button)
                {
                    rebindingOperation = rebindingAction.PerformInteractiveRebinding()
                    .WithControlsExcluding(excludeKeys)
                    .WithCancelingThrough("<Keyboard>/escape")
                    .WithControlsExcluding("<keyboard>/contextMenu")
                    .WithControlsExcluding("<keyboard>/anyKey")
                    .WithControlsExcluding("<keyboard>/leftMeta")
                    .WithControlsExcluding("<keyboard>/capsLock")
                    .WithControlsExcluding("<keyboard>/leftAlt")
                    .WithControlsExcluding("<keyboard>/alt")
                    .WithControlsExcluding("<keyboard>/rightAlt")
                    .WithControlsExcluding("<keyboard>/tab")
                    .WithControlsExcluding("<keyboard>/numLock")
                    .WithControlsExcluding("<keyboard>/printScreen")
                    .WithControlsExcluding("<keyboard>/scrollLock")
                    .WithControlsExcluding("<keyboard>/pause")
                    .OnMatchWaitForAnother(0.2f)
                    .OnCancel(operation => RebindCancel())
                    .OnComplete(operation => RebindComplete()).Start();
                }
                else if (type == RebindingTypes.Composite)
                {
                    rebindingOperation = rebindingAction.PerformInteractiveRebinding()
                    .WithTargetBinding(BindingIndex)
                    .WithControlsExcluding(excludeKeys)
                    .WithCancelingThrough("<Keyboard>/escape")
                    .WithControlsExcluding("<keyboard>/contextMenu")
                    .WithControlsExcluding("<keyboard>/anyKey")
                    .WithControlsExcluding("<keyboard>/alt")
                    .WithControlsExcluding("<keyboard>/leftMeta")
                    .WithControlsExcluding("<keyboard>/capsLock")
                    .WithControlsExcluding("<keyboard>/leftAlt")
                    .WithControlsExcluding("<keyboard>/rightAlt")
                    .WithControlsExcluding("<keyboard>/tab")
                    .WithControlsExcluding("<keyboard>/numLock")
                    .WithControlsExcluding("<keyboard>/printScreen")
                    .WithControlsExcluding("<keyboard>/scrollLock")
                    .WithControlsExcluding("<keyboard>/pause")
                    .OnMatchWaitForAnother(0.2f)
                    .OnCancel(operation => RebindCancel())
                    .OnComplete(operation => RebindComplete()).Start();
                }
            }
        }
        public void DisplayKey()
        {
            int bindingIndex = GetBindingIndex();
            if (bindingIndex == -1)
            {
                rebindingKeyText.text = "NONE";
                return;
            }
            rebindingKeyText.text = AbbreviateTitle(InputControlPath.ToHumanReadableString(rebindingAction.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice));
            //Debug.Log(rebindingAction.action.bindings[bindingIndex].effectivePath);
        }
        private int GetBindingIndex()
        {
            int bindingIndex = 0;
            try
            {
                if (type == RebindingTypes.Button)
                    bindingIndex = rebindingAction.GetBindingIndexForControl(rebindingAction.controls[0]);
                else if (type == RebindingTypes.Composite)
                    bindingIndex = BindingIndex;
            }
            catch (System.Exception)
            {
                return -1;
            }
            return bindingIndex;
        }
        public void RebindCancel()
        {
            changing = false;
            rebindingOperation.Dispose();
            DisplayKey();
        }
        private void RebindComplete()
        {
            changing = false;
            rebindingOperation.Dispose();
            foreach (var item in rebindingAction.actionMap)
            {
                foreach (var bind in item.bindings)
                {
                    if ((bind.effectivePath == rebindingAction.bindings[GetBindingIndex()].effectivePath) && (bind.name != rebindingAction.bindings[GetBindingIndex()].name || item.name != rebindingAction.name || bind != rebindingAction.bindings[GetBindingIndex()]))
                    {
                        //if (string.IsNullOrEmpty(bind.name) && bind.effectiveProcessors == null)
                        //    item.ChangeBindingWithPath(bind.effectivePath).WithPath(oldBindPath);
                        //else
                        item.ApplyBindingOverride(item.GetBindingIndex(bind), oldBindPath);
                        GameSettings.OnKeyRebind?.Invoke();
                    }
                }
            }
            DisplayKey();
        }
        public static string AbbreviateTitle(string originalTitle)
        {
            string abbreviatedTitle = originalTitle;
            // Replace "Left" with "L" and "Right" with "R"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(Left|Right)", m => m.Value[0].ToString());
            // Replace "Control" with "CTRL"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)Control", "CTRL");
            // Replace "Page Down" with "PGDN"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)Page Down", "PGDN");
            // Replace "Page Up" with "PGUP"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)Page Up", "PGUP");
            // Replace "Delete" with "DEL"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)Delete", "DEL");
            // Replace "Insert" with "INS"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)Insert", "INS");
            // Replace "Numpad" with "NUM"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)Numpad", "NUM");
            // Replace "Enter" with "ENTR"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)Enter", "ENTR");
            // Replace "NUM ENTR" with "ENTR"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)NUM ENTR", "ENTR");
            // Replace "LEFT BUTTON" with "LMB"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)L BUTTON", "LMB");
            // Replace "RIGHT BUTTON" with "RMB"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)R BUTTON", "RMB");
            // Replace "FORWARD" with "MOUSE4"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)FORWARD", "MOUSE4");
            // Replace "BACK" with "MOUSE5"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)BACK", "MOUSE5");
            // Replace "MIDDLE BUTTON" with "RMB"
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(?i)MIDDLE BUTTON", "MMB");
            // All letters to upper
            abbreviatedTitle = Regex.Replace(abbreviatedTitle, "(Control|Shift|Alt)", m => m.Value.ToUpper());
            return abbreviatedTitle;
        }
    }
}