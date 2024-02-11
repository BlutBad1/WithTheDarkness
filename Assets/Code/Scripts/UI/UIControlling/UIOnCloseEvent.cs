using UIControlling;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityMethodsNS;

public class UIOnCloseEvent : OnEnableMethodAfterStart
{
	[SerializeField, FormerlySerializedAs("UnityEvent")]
	protected UnityEvent unityEvent;

	protected override void OnEnableAfterStart()
	{
		WindowEscape.Instance.OnUIEscapePressed += EventInvoke;
	}
	private void OnDisable()
	{
		WindowEscape.Instance.OnUIEscapePressed -= EventInvoke;
	}
	public virtual void EventInvoke() =>
		unityEvent?.Invoke();
}
