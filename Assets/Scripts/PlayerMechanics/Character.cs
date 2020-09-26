using System.Collections;
using UnityEngine;

namespace NeonBlaze.PlayerMechanics
{
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
	public class Character : MonoBehaviour
	{
		public float Health
		{
			get => mHealth;
			set => mHealth = Mathf.Clamp(value, 0, m_MaxHealth);
		}
		public float NormalizedHealth => Health / m_MaxHealth;

		public float Stamina
		{
			get => mStamina;
			private set => mStamina = Mathf.Clamp(value, 0, m_MaxStamina);
		}
		public float NormalizedStamina => Stamina / m_MaxStamina;

		[SerializeField] private SpriteRenderer m_Renderer;
		[SerializeField] private float m_Speed = 1f;
		[SerializeField] private float m_MaxHealth = 100;
		[SerializeField] private float m_MaxStamina = 100;
		[SerializeField] private float m_StaminaRegenerationSpeed = 10;
		[SerializeField] private float m_DashStaminaCost = 20;
		[SerializeField] private float m_DashSpeed = 1f;
		[SerializeField] private float m_DashDuration;
		[SerializeField] private float m_DashCooldown = 1f;

		private Rigidbody2D mRigidbody;
		private Collider2D mCollider;
		private ICharacterInput mInput;

		private Vector2 mDashDirection;
		private float mCurrentSpeed;
		private bool mDashCoolDownActive;
		private bool mDashActive;

		private float mHealth;
		private float mStamina;

		private void Awake()
		{
			mRigidbody = GetComponent<Rigidbody2D>();
			mCollider = GetComponent<Collider2D>();
			mInput = GetComponent<ICharacterInput>();
			if (mInput == null) Debug.LogError("No character input script was found on this gameObject");
		}

		private void Start()
		{
			mCurrentSpeed = m_Speed;
			Health = m_MaxHealth;
			Stamina = m_MaxStamina;

			m_Renderer.material.color = Color.white;
		}

		private void Update()
		{
			mInput.ManualUpdate();

			var movementDirection = !mDashActive
				? mInput.MovementDirection * mCurrentSpeed
				: mDashDirection;

			if (!mDashCoolDownActive
				&& Stamina >= m_DashStaminaCost
				&& mInput.Dash
				&& movementDirection.sqrMagnitude > 0.001f * 0.001f)
			{
				StartCoroutine(Dash());
			}

			if (mInput.LightAttack)
			{
				Debug.Log("FIRE!");
			}

			if (mInput.HeavyAttack)
			{
				Debug.Log("Charging secondary attack");
			}

			mRigidbody.MovePosition(mRigidbody.position + movementDirection * Time.smoothDeltaTime);

			Stamina += m_StaminaRegenerationSpeed * Time.deltaTime;
		}

		private void OnDestroy()
		{
			Destroy(m_Renderer.material);
		}

		private IEnumerator Dash()
		{
			mDashCoolDownActive = true;
			mDashActive = true;

			Stamina -= m_DashStaminaCost;
			mCollider.enabled = false;

			var originalColor = m_Renderer.material.color;
			var color = Color.white;
			color.a = 0.5f;
			m_Renderer.material.color = color;

			var originalSpeed = mCurrentSpeed;
			mCurrentSpeed = m_DashSpeed;
			mDashDirection = mInput.MovementDirection * mCurrentSpeed;

			m_DashDuration = Mathf.Max(Time.deltaTime, m_DashDuration);
			yield return new WaitForSeconds(m_DashDuration);

			mCurrentSpeed = originalSpeed;
			m_Renderer.material.color = originalColor;
			mCollider.enabled = true;

			mDashActive = false;

			yield return new WaitForSeconds(m_DashCooldown - m_DashDuration);

			mDashCoolDownActive = false;
		}
	}
}
