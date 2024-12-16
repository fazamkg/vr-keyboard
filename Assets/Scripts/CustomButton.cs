using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

namespace Game
{
	/// <summary>
	/// Кастомная кнопка
	/// </summary>
	public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
		IPointerClickHandler
	{
		public event Action OnClick;

		[SerializeField] private Image _image;
		[SerializeField] private Color _normalColor;
		[SerializeField] private Color _highlightColor;
		[SerializeField] private Color _pressedColor;
		[SerializeField] private float _fadeDuration;

		private bool _within;
		private bool _joystickMode;

		public bool JoystickMode
		{
			get => _joystickMode;
			set
			{
				_joystickMode = value;
				Deselect();
			}
		}

		private void OnEnable()
		{
			_image.CrossFadeColor(_normalColor, 0f, true, true);
		}

		public void Select()
		{
			_image.CrossFadeColor(_highlightColor, _fadeDuration, true, true);
		}

		public void Deselect()
		{
			_image.CrossFadeColor(_normalColor, _fadeDuration, true, true);
		}

		public void Click()
		{
			_image.CrossFadeColor(_pressedColor, _fadeDuration, true, true);
			OnClick?.Invoke();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (JoystickMode) return;

			_within = true;

			Select();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (JoystickMode) return;

			_within = false;

			Deselect();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (JoystickMode) return;

			if (_within)
			{
				Click();
			}
		}
	}
}
