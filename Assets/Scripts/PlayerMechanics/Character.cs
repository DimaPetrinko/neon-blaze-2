using System.Collections;
using UnityEngine;

namespace NeonBlaze.PlayerMechanics
{
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
	public sealed class Character : MonoBehaviour
	{
		public float Health
		{
			get => mHealth;
			set
			{
				mHealth = Mathf.Clamp(value, 0, m_MaxHealth);
				if (mHealth <= 0) Die();
			}
		}

		public float NormalizedHealth => Health / m_MaxHealth;

		public float Stamina
		{
			get => mStamina;
			private set => mStamina = Mathf.Clamp(value, 0, m_MaxStamina);
		}
		public float NormalizedStamina => Stamina / m_MaxStamina;

		[Header("Appearance")]
		[SerializeField] private SpriteRenderer m_Renderer;
		[Header("Stats")]
		[SerializeField] private float m_MaxHealth = 100;
		[SerializeField] private float m_MaxStamina = 100;
		[SerializeField] private float m_StaminaRegenerationSpeed = 10;
		[SerializeField] private float m_LightAttackDamage = 20;
		[SerializeField] private float m_Speed = 1;
		[Header("Dash")]
		[SerializeField] private float m_DashStaminaCost = 20;
		[SerializeField] private float m_DashSpeed = 1;
		[SerializeField] private float m_DashDuration;
		[SerializeField] private float m_DashCooldown = 1;

		private Rigidbody2D mRigidbody;
		private Collider2D mCollider;
		private ICharacterInput mInput;

		private Vector2 mDashDirection;
		private bool mDashCoolDownActive;
		private bool mDashActive;

		private float mHealth;
		private float mStamina;

		private void Awake()
		{
			mRigidbody = GetComponent<Rigidbody2D>();
			mCollider = GetComponent<Collider2D>();
			mInput = GetComponent<ICharacterInput>();
			if (mInput == null)
			{
				Debug.LogError("No character input script was found on this gameObject");
				enabled = false;
			}
		}

		private void Start()
		{
			Health = m_MaxHealth;
			Stamina = m_MaxStamina;

			var color = m_Renderer.material.color;
			m_Renderer.material.color = color; // just to create a material instance
		}

		private void Update()
		{
			mInput.ManualUpdate();

			var movementDirection = !mDashActive
				? mInput.MovementDirection * m_Speed
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
				Attack();
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

		private void Attack()
		{
			// cast circle in front
			// if something is in it
			// and it's a character
			// retract health from it

			var hits = new RaycastHit2D[4];
			var hitsCount = Physics2D.CircleCastNonAlloc(transform.position,
				((CircleCollider2D)mCollider).radius * 2f, Vector2.zero, hits);

			if (hitsCount <= 0) return;

			for (var i = 0; i < hitsCount; i++)
			{
				if (hits[i].transform == transform) continue;

				var character = hits[i].transform.GetComponent<Character>();
				if (character == null) continue;

				character.Health -= m_LightAttackDamage;
			}
		}

		private IEnumerator Dash()
		{
			mDashCoolDownActive = true;
			mDashActive = true;

			Stamina -= m_DashStaminaCost;
			mCollider.enabled = false;

			var originalColor = m_Renderer.material.color;
			var newColor = originalColor;
			newColor.a *= 0.5f;
			m_Renderer.material.color = newColor;

			mDashDirection = mInput.MovementDirection * m_DashSpeed;

			m_DashDuration = Mathf.Max(Time.deltaTime, m_DashDuration);
			yield return new WaitForSeconds(m_DashDuration);

			m_Renderer.material.color = originalColor;
			mCollider.enabled = true;

			mDashActive = false;

			yield return new WaitForSeconds(m_DashCooldown - m_DashDuration);

			mDashCoolDownActive = false;
		}

		private void Die()
		{
			Debug.Log($"{name} died!");
			gameObject.SetActive(false);
		}
	}
}
