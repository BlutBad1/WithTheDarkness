using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIControlling
{
    public class MyEventTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        public UnityEvent PointerClickEvent;
        public UnityEvent PointerEnterEvent;
        public void OnPointerClick(PointerEventData eventData) =>
            PointerClickEvent?.Invoke();
      
        public void OnPointerEnter(PointerEventData eventData) =>
            PointerEnterEvent?.Invoke();
    }
}