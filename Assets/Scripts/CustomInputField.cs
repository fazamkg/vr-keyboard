using UnityEngine;
using TMPro;
using System.Collections;

namespace Game
{
	/// <summary>
	/// Кастомное поле для ввода
	/// </summary>
	public class CustomInputField : MonoBehaviour
	{
		[SerializeField] private TMP_Text _text;
		[SerializeField] private RectTransform _caret;
		[SerializeField] private float _caretBlinkInterval;

		private int _caretPosition;

		public string Text
		{
			get => _text.text;
			set => _text.text = value;
		}
		public int Length => Text.Length;
		public int CaretPosition
		{
			get => _caretPosition;
			set
			{
				_caretPosition = value;

				Text += " ";
				_text.ForceMeshUpdate();

				var charInfo = _text.textInfo.characterInfo[_caretPosition];
				var posLocal = charInfo.bottomLeft;
				posLocal.y = charInfo.baseLine;
				var posWorld = _text.transform.TransformPoint(posLocal);
				_caret.position = posWorld;

				Text = Text.Substring(0, Length - 1);
			}
		}

		private void OnEnable()
		{
			StartCoroutine(BlinkCaretCoroutine());
			CaretPosition = 0;
		}

		private IEnumerator BlinkCaretCoroutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(_caretBlinkInterval);
				_caret.gameObject.SetActive(!_caret.gameObject.activeSelf);
			}
		}
	}
}
