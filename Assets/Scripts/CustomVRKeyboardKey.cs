using UnityEngine;
using TMPro;
using UnityEditor;

namespace Game
{
	/// <summary>
	/// Кастомная клавиша ВР клавиатуры
	/// </summary>
    public class CustomVRKeyboardKey : MonoBehaviour
    {
		[SerializeField] private CustomButton _button;
		[SerializeField] private TMP_Text _text;
		[SerializeField] private CustomVRKeyboard _keyboard;
		[SerializeField] private string _keycode;
		[SerializeField] private string _keycodeShift;
		[SerializeField] private SpecialCharacter _specialCharacter;

		[Header("Navigation")]
		[SerializeField] private CustomVRKeyboardKey _up;
		[SerializeField] private CustomVRKeyboardKey _left;
		[SerializeField] private CustomVRKeyboardKey _right;
		[SerializeField] private CustomVRKeyboardKey _down;

		private string KeyCode
		{
			get
			{
				if (_specialCharacter == SpecialCharacter.None)
				{
					if (UseShiftKey && string.IsNullOrEmpty(_keycodeShift) == false)
					{
						return _keycodeShift;
					}
					else
					{
						return _keycode;
					}
				}

				return _specialCharacter.ToString();
			}
		}

		public bool UseShiftKey { get; set; }
		public bool JoystickMode
		{
			get => _button.JoystickMode;
			set => _button.JoystickMode = value;
		}

		public CustomVRKeyboardKey Up => _up;
		public CustomVRKeyboardKey Down => _down;
		public CustomVRKeyboardKey Left => _left;
		public CustomVRKeyboardKey Right => _right;

		private void Awake()
		{
			if (_button != null)
			{
				_button.OnClick += OnKeyHit;
			}
		}

		/// <summary>
		/// Выбрать клавишу
		/// </summary>
		public void Select()
		{
			_button.Select();
		}

		/// <summary>
		/// Убрать выборку на клавише
		/// </summary>
		public void Deselect()
		{
			_button.Deselect();
		}

		/// <summary>
		/// Нажать на клавишу
		/// </summary>
		public void Click()
		{
			_button.Click();
		}

		/// <summary>
		/// Переключить режим Shift
		/// </summary>
		public void ToggleShift()
		{
			UseShiftKey = !UseShiftKey;

			if (_text == null)
			{
				return;
			}

			if (UseShiftKey && string.IsNullOrEmpty(_keycodeShift) == false)
			{
				_text.text = _keycodeShift;
			}
			else
			{
				_text.text = _keycode;
			}
		}

		private void OnKeyHit() => OnKeyHit(KeyCode);

		private void OnKeyHit(string key)
		{
			if (_keyboard == null)
			{
				Debug.LogWarning($"Pressed key {key}, but no keyboard was found");
			}

			_keyboard.PressKey(key);
		}

#if UNITY_EDITOR
		[ContextMenu("Fill out references")]
		private void FillOutReferences()
		{
			Undo.RecordObject(this, "Fill out references");

			_button = GetComponent<CustomButton>();
			_text = GetComponentInChildren<TMP_Text>();
			_keyboard = GetComponentInParent<CustomVRKeyboard>();
		}

		private void OnDrawGizmosSelected()
		{
			Handles.color = Color.red;
			if (_up != null) Handles.DrawLine(transform.position, _up.transform.position, 2f);
			if (_down != null) Handles.DrawLine(transform.position, _down.transform.position, 2f);
			if (_left != null) Handles.DrawLine(transform.position, _left.transform.position, 2f);
			if (_right != null) Handles.DrawLine(transform.position, _right.transform.position, 2f);
		}
#endif
	}
}
