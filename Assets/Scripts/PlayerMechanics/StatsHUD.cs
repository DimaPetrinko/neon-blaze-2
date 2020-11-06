using UnityEngine;
using UnityEngine.UI;

namespace NeonBlaze.PlayerMechanics
{
	public sealed class StatsHUD : BaseUIPanel
	{
		[SerializeField] private Stats m_Stats;
		[SerializeField] private Image m_HealthBar;
		[SerializeField] private Image m_StaminaBar;

		private void Update()
		{
			m_HealthBar.fillAmount = m_Stats.NormalizedHealth;
			m_StaminaBar.fillAmount = m_Stats.NormalizedStamina;
		}
	}
}