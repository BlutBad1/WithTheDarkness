using System;
using System.Linq;
using UnityEngine;
namespace Data.Text
{
	public enum TextKind
	{
		UI = 0, InGameInteractable
	}
	public enum LanguageLocal
	{
		English = 0
	}
	[Serializable]
	public class LocalText
	{
		public string Text;
		public LanguageLocal Language;
		public TextKind TextKind;
	}
	[CreateAssetMenu(fileName = "TextData", menuName = "ScriptableObject/Text/TextData")]
	public class TextData : ScriptableObject
	{
		[SerializeField]
		protected LocalText[] texts;

		public virtual string GetText()
		{
			return GetTextsByLanguage()[0].Text;
		}
		protected LocalText[] GetTextsByLanguage()
		{
			return texts.Where(x => x.Language == LanguageLocal.English).ToArray();
		}
	}
}