using UnityEngine;
using UnityEngine.UI;

namespace NeonBlaze.PlayerMechanics
{
	public class PlayerUI : MonoBehaviour
	{
		[SerializeField] private Player m_Player;
		[SerializeField] private Image m_HealthBar;
		[SerializeField] private Image m_StaminaBar;

		private void Update()
		{
			m_HealthBar.fillAmount = m_Player.NormalizedHealth;
			m_StaminaBar.fillAmount = m_Player.NormalizedStamina;
		}
	}
}