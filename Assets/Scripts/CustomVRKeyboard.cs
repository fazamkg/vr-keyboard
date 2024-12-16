using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System;

namespace Game
{
	public enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}

	public enum SpecialCharacter
	{
		None,
		Space,
		Backspace,
		Shift,
		Enter,
		MainKeyboard,
		SecondaryKeyboard,
		SwitchInput
	}

	/// <summary>
	/// ВР Клавиатура
	/// </summary>
    public class CustomVRKeyboard : MonoBehaviour
    {
		public event Action OnToggleInputMode;

		[SerializeField] private CustomInputField _inputField;
		[SerializeField] private SubVRKeyboard _mainKeyboard;
		[SerializeField] private SubVRKeyboard _secondaryKeyboard;
		[SerializeField] private List<CustomVRKeyboardKey> _keys;

		private bool _useShift;

		private string Text
		{
			get => _inputField.Text;
			set => _inputField.Text = value;
		}
		private int CaretPosition
		{
			get => _inputField.CaretPosition;
			set => _inputField.CaretPosition = value;
		}
		public List<CustomVRKeyboardKey> Keys => _keys;

		/// <summary>
		/// Нажать на клавишу
		/// </summary>
		/// <param name="key">Клавиша в чистом виде или название enum SpecialCharacter</param>
		public void PressKey(string key)
		{
			if (_inputField == null)
			{
				Debug.LogWarning($"Pressed Key {key} but no input field");
				return;
			}

			UpdateInputField(key);
		}

		private void UpdateInputField(string key)
		{
			switch (key)
			{
				case nameof(SpecialCharacter.Enter):
					InsertCharacter("\n");
					break;

				case nameof(SpecialCharacter.Space):
					InsertCharacter(" ");
					break;

				case nameof(SpecialCharacter.Backspace):
					if (CaretPosition == 0)
						break;

					Text = Text.Remove(CaretPosition - 1, 1);
					CaretPosition--;
					break;

				case nameof(SpecialCharacter.Shift):
					ToggleShift();
					break;

				case nameof(SpecialCharacter.MainKeyboard):
					if (_mainKeyboard == null)
					{
						Debug.LogWarning("No main keyboard reference!");
					}
					_mainKeyboard.SwitchTo();
					break;

				case nameof(SpecialCharacter.SecondaryKeyboard):
					if (_secondaryKeyboard == null)
					{
						Debug.LogWarning("No secondary keyboard reference!");
					}
					_secondaryKeyboard.SwitchTo();
					break;

				case nameof(SpecialCharacter.SwitchInput):
					OnToggleInputMode?.Invoke();
					break;

				default:
					InsertCharacter(key);
					break;
			}
		}

		private void InsertCharacter(string character)
		{
			Text = Text.Insert(CaretPosition, character);
			CaretPosition++;
		}

		private void ToggleShift()
		{
			_useShift = !_useShift;

			foreach (var key in _keys)
			{
				if (key == null) continue;

				key.ToggleShift();
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Grab keys")]
		private void GrabKeys()
		{
			Undo.RecordObject(this, "Grab keys");

			_keys = GetComponentsInChildren<CustomVRKeyboardKey>(true).ToList();
		}
#endif
	} 
}
