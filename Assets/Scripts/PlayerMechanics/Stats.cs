using System;
using UnityEngine;

namespace NeonBlaze.PlayerMechanics
{
	public class Stats : MonoBehaviour
	{
		public event Action HealthDepleted;

		public float Health
		{
			get => mHealth;
			set
			{
				mHealth = Mathf.Clamp(value, 0, m_MaxHealth);
				if (mHealth <= 0) HealthDepleted?.Invoke();
			}
		}
		public float NormalizedHealth => Health / m_MaxHealth;

		public float Stamina
		{
			get => mStamina;
			set => mStamina = Mathf.Clamp(value, 0, m_MaxStamina);
		}
		public float NormalizedStamina => Stamina / m_MaxStamina;

		[SerializeField] private float m_MaxHealth = 100;
		[SerializeField] private float m_MaxStamina = 100;
		[SerializeField] private float m_StaminaRegenerationSpeed = 20;

		private float mHealth;
		private float mStamina;

		public void ManualUpdate(bool canRegenerateStamina)
		{
			if (canRegenerateStamina) Stamina += m_StaminaRegenerationSpeed * Time.deltaTime;
		}

		private void Start()
		{
			Health = m_MaxHealth;
			Stamina = m_MaxStamina;
		}
	}
}