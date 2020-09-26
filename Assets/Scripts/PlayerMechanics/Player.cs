using System;
using System.Collections;
using UnityEngine;

namespace NeonBlaze.PlayerMechanics
{
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
	public class Player : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer m_Renderer;
		[SerializeField] private float m_Speed = 1f;
		[SerializeField] private float m_MaxHealth = 100;
		[SerializeField] private float m_MaxStamina = 100;
		[SerializeField] private float m_StaminaRegenerationSpeed = 10;
		[SerializeField] private float m_DashStaminaCost = 20;
		[SerializeField] private float m_DashSpeed = 1f;
		[SerializeField] private float m_DashDuration;
		[SerializeField] private float m_DashCooldown = 1f;

		public float Health
		{
			get => mHealth;
			set => mHealth = Mathf.Clamp(value, 0, m_MaxHealth);
		}
		public float NormalizedHealth => (float)Health / m_MaxHealth;

		public float Stamina
		{
			get => mStamina;
			private set => mStamina = Mathf.Clamp(value, 0, m_MaxStamina);
		}
		public float NormalizedStamina => (float)Stamina / m_MaxStamina;

		private Rigidbody2D mRigidbody;
		private Collider2D mCollider;
		private float mCurrentSpeed;
		private bool mDashActive;

		private float mHealth;
		private float mStamina;

		private void Awake()
		{
			mRigidbody = GetComponent<Rigidbody2D>();
			mCollider = GetComponent<Collider2D>();
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
			var movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
			movementVector *= Time.smoothDeltaTime * mCurrentSpeed;

			if (!mDashActive && Stamina >= m_DashStaminaCost && Input.GetButtonDown("Dash"))
			{
				StartCoroutine(Dash());
			}

			if (Input.GetButtonDown("LightAttack"))
			{
				Debug.Log("FIRE!");
			}

			if (Input.GetButton("HeavyAttack"))
			{
				Debug.Log("Charging secondary attack");
			}

			mRigidbody.MovePosition(mRigidbody.position + movementVector);

			Stamina += m_StaminaRegenerationSpeed * Time.deltaTime;
		}

		private void OnDestroy()
		{
			Destroy(m_Renderer.material);
		}

		private IEnumerator Dash()
		{
			mDashActive = true;
			m_DashDuration = Mathf.Max(Time.deltaTime, m_DashDuration);

			Stamina -= m_DashStaminaCost;
			mCollider.enabled = false;

			var originalColor = m_Renderer.material.color;
			var color = Color.white;
			color.a = 0.5f;
			m_Renderer.material.color = color;

			var originalSpeed = mCurrentSpeed;
			mCurrentSpeed = m_DashSpeed;

			yield return new WaitForSeconds(m_DashDuration);

			mCurrentSpeed = originalSpeed;
			m_Renderer.material.color = originalColor;
			mCollider.enabled = true;

			yield return new WaitForSeconds(m_DashCooldown - m_DashDuration);

			mDashActive = false;
		}
	}
}
