using UnityEngine;
using System;

namespace Game
{
	/// <summary>
	/// Для подчастей клавиатуры (основная, дополнительная и т.д.)
	/// </summary>
    public class SubVRKeyboard : MonoBehaviour
    {
		public static event Action OnSwitch;

		[Tooltip("Первая выбранная клавиша, если используется управления джойстиком")]
		[SerializeField] private CustomVRKeyboardKey _firstSelected;

		public static SubVRKeyboard CurrentlyActiveKeyboard { get; private set; }

		public CustomVRKeyboardKey FirstSelectedKey => _firstSelected;

		private void Awake()
		{
			CurrentlyActiveKeyboard = this;
		}

		/// <summary>
		/// Переключить на эту подклавиатуру
		/// </summary>
		public void SwitchTo()
		{
			CurrentlyActiveKeyboard.gameObject.SetActive(false);
			gameObject.SetActive(true);
			CurrentlyActiveKeyboard = this;

			OnSwitch?.Invoke();
		}
	}
}
