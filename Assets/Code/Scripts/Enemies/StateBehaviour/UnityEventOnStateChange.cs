using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace EnemyNS.Base.StateBehaviourNS
{
    public class UnityEventOnStateChange : MonoBehaviour, ISerializationCallbackReceiver
    {
        public static List<string> StateNames;

        [SerializeField]
        private StateHandler stateHandler;
        [SerializeField, ListToMultiplePopup(typeof(UnityEventOnStateChange), "StateNames")]
        private int invokeEventIfChangeTo;
        [SerializeField]
        private UnityEvent unityEvent;

        private void OnEnable()
        {
            SubscribeEvent();
        }
        private void OnDisable()
        {
            UnsubscribeEvent();
        }
        public void OnBeforeSerialize()
        {
            if (StateNames == null || StateNames.Count == 0)
                StateNames = Enum.GetNames(typeof(EnemyState)).ToList();
        }
        public void OnAfterDeserialize() { }
        public void SubscribeEvent()
        {
            stateHandler.OnStateChange += OnStateChangeEvent;
        }
        public void UnsubscribeEvent()
        {
            stateHandler.OnStateChange -= OnStateChangeEvent;
        }
        private void OnStateChangeEvent(EnemyState oldState, EnemyState newState)
        {
            for (int i = 0; i < StateNames.Count; i++)
            {
                if ((invokeEventIfChangeTo & (1 << i)) != 0 && StateNames[i] == newState.ToString())
                    unityEvent?.Invoke();
            }
        }
    }
}