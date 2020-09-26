using UnityEngine;
using UnityEngine.UI;

namespace NeonBlaze.PlayerMechanics
{
	public class CharacterHUD : MonoBehaviour
	{
		[SerializeField] private Character m_Character;
		[SerializeField] private Image m_HealthBar;
		[SerializeField] private Image m_StaminaBar;

		private void Update()
		{
			m_HealthBar.fillAmount = m_Character.NormalizedHealth;
			m_StaminaBar.fillAmount = m_Character.NormalizedStamina;
		}
	}
}