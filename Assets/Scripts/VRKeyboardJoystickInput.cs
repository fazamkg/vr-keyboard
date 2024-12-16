using BNG;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// Используется для ввода на вр клавиатуре джойстиком
	/// </summary>
    public class VRKeyboardJoystickInput : MonoBehaviour
    {
		[Tooltip("Для выключения поворота игрока во время управления джойстиком")]
		[SerializeField] private PlayerRotation _playerRotation;

		[SerializeField] private CustomVRKeyboard _keyboard;
		[SerializeField] private float _deadzone;

		private CustomVRKeyboardKey _currentlySelectedKey;
		private bool _inputConsumed;
		private bool _enabled;

		private void Awake()
		{
			_keyboard.OnToggleInputMode += OnToggleInputMode;
			SubVRKeyboard.OnSwitch += OnSubKeyboardSwitch;
		}

		private void Update()
		{
			OnJoystickInput();
		}

		private void OnSubKeyboardSwitch()
		{
			if (_currentlySelectedKey != null)
			{
				_currentlySelectedKey.Deselect();
			}
			if (_enabled)
			{
				_currentlySelectedKey = SubVRKeyboard.CurrentlyActiveKeyboard.FirstSelectedKey;
				_currentlySelectedKey.Select();
			}
		}

		private void OnToggleInputMode()
		{
			_enabled = !_enabled;

			if (_enabled)
			{
				_currentlySelectedKey = SubVRKeyboard.CurrentlyActiveKeyboard.FirstSelectedKey;
				_currentlySelectedKey.Select();
				_playerRotation.AllowInput = false;

				foreach (var key in _keyboard.Keys)
				{
					key.JoystickMode = true;
				}
			}
			else
			{
				_playerRotation.AllowInput = true;

				foreach (var key in _keyboard.Keys)
				{
					key.JoystickMode = false;
				}
			}
		}

		private void OnJoystickInput()
		{
			if (_enabled == false) return;

			if (InputBridge.Instance.RightTriggerDown)
			{
				_currentlySelectedKey.Click();
			}

			var axis = InputBridge.Instance.GetInputAxisValue(InputAxis.RightThumbStickAxis);
			if (axis.sqrMagnitude < _deadzone * _deadzone)
			{
				_inputConsumed = false;
				return;
			}

			if (_inputConsumed) return;
			_inputConsumed = true;
			axis.Normalize();

			_currentlySelectedKey.Deselect();

			var radians = Mathf.Atan2(axis.y, axis.x);
			if (radians < 0f)
			{
				radians += Mathf.PI * 2f;
			}

			if (radians < Mathf.PI * 0.25f || radians > 1.75f * Mathf.PI)
			{
				_currentlySelectedKey = _currentlySelectedKey.Right;
			}
			else if (radians > Mathf.PI * 0.25 && radians < Mathf.PI * 0.75f)
			{
				_currentlySelectedKey = _currentlySelectedKey.Up;
			}
			else if (radians > Mathf.PI * 0.75f && radians < Mathf.PI * 1.25f)
			{
				_currentlySelectedKey = _currentlySelectedKey.Left;
			}
			else
			{
				_currentlySelectedKey = _currentlySelectedKey.Down;
			}

			_currentlySelectedKey.Select();
		}
	} 
}
