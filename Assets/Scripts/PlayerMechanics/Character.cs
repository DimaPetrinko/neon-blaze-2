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
		[SerializeField] [Range(0, 10)] private float m_Speed = 1;
		[Header("Movement")]
		[SerializeField] [Range(0, 1)] private float m_MovementSmoothness = 1f;
		[Header("Dash")]
		[SerializeField] private float m_DashStaminaCost = 20;
		[SerializeField] [Range(0, 50)]  private float m_DashSpeed = 1;
		[SerializeField] [Range(0, 2)] private float m_DashDuration;
		[SerializeField] [Range(0, 10)] private float m_DashCooldown = 1;
		[Header("LightAttack")]
		[SerializeField] private Weapon m_Weapon;

		private Rigidbody2D mRigidbody;
		private Collider2D mCollider;
		private ICharacterInput mInput;

		private CharacterState mCurrentState;
		private CharacterState mPreviousState;

		private Vector2 mTargetPosition;
		private Vector2 mDashDirection;

		private float mHealth;
		private float mStamina;

		#region Unity events

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
			mCurrentState = CharacterState.Normal;
			mTargetPosition = mRigidbody.position;
		}

		private void Update()
		{
			mInput.ManualUpdate();

			Vector2 movementDirection;
			if (mCurrentState.IsNormalOrDashCooldown()) movementDirection = mInput.MovementDirection * m_Speed;
			else if (mCurrentState.Is(CharacterState.Dash)) movementDirection = mDashDirection;
			else if (mCurrentState.IsAttack()) movementDirection = Vector2.zero;
			else movementDirection = Vector2.zero;

			if (mCurrentState.IsNormal()
				&& Stamina >= m_DashStaminaCost
				&& mInput.Dash
				&& movementDirection.sqrMagnitude > 0.001f * 0.001f)
			{
				StartCoroutine(Dash());
			}

			if (mCurrentState.IsNormalOrDashCooldown() && mInput.LightAttack)
			{
				StartCoroutine(AttackCoroutine());
			}

			if (mCurrentState.IsNormalOrDashCooldown() && mInput.HeavyAttackHeld)
			{
				Debug.Log("Charging secondary attack");
			}

			mTargetPosition += movementDirection * Time.smoothDeltaTime;

			m_MovementSmoothness = Mathf.Max(0.001f, m_MovementSmoothness);
			mRigidbody.MovePosition(Vector2.Lerp(mRigidbody.position, mTargetPosition,
				Time.smoothDeltaTime / m_MovementSmoothness));

			if (mCurrentState.IsNormalOrDashCooldown())
			{
				Stamina += m_StaminaRegenerationSpeed * Time.deltaTime;
			}
		}

		private void OnDestroy()
		{
			Destroy(m_Renderer.material);
			if (mCurrentState.Is(CharacterState.AttackHit)) m_Weapon.ObjectHit -= HandleLightAttackHit;
		}

		#endregion

		private IEnumerator AttackCoroutine()
		{
			mPreviousState = mCurrentState;
			mCurrentState = CharacterState.AttackWindUp;

			Stamina -= m_Weapon.StaminaCost;
			m_Weapon.Show();
			m_Weapon.WindUp();

			yield return new WaitForSeconds(m_Weapon.WindUpDuration);

			mCurrentState = CharacterState.AttackHit;
			m_Weapon.ObjectHit += HandleLightAttackHit;
			m_Weapon.Hit();

			yield return new WaitForSeconds(m_Weapon.HitDuration);

			mCurrentState = CharacterState.AttackRecovery;
			m_Weapon.ObjectHit -= HandleLightAttackHit;
			m_Weapon.Recover();

			yield return new WaitForSeconds(m_Weapon.RecoveryDuration);

			m_Weapon.Hide();
			mCurrentState = mPreviousState;
		}

		private void HandleLightAttackHit(Character other)
		{
			if (other == this) return;

			other.Health -= m_Weapon.Damage;
		}

		private IEnumerator Dash()
		{
			mPreviousState = mCurrentState;
			var truePreviousState = mPreviousState;
			mCurrentState = CharacterState.Dash;

			Stamina -= m_DashStaminaCost;
			mCollider.enabled = false;

			var originalColor = m_Renderer.material.color;
			var newColor = originalColor;
			newColor.a *= 0.5f;
			m_Renderer.material.color = newColor;

			mDashDirection = mInput.MovementDirection * m_DashSpeed;

			m_DashDuration = Mathf.Max(Time.deltaTime, m_DashDuration);
			yield return new WaitForSeconds(m_DashDuration);

			mCurrentState = CharacterState.DashCooldown;

			m_Renderer.material.color = originalColor;
			mCollider.enabled = true;

			yield return new WaitForSeconds(m_DashCooldown - m_DashDuration);

			if (mCurrentState.IsAttack()) mPreviousState = truePreviousState;
			else mCurrentState = mPreviousState;
		}

		private void Die()
		{
			Debug.Log($"{name} died!");
			gameObject.SetActive(false);
		}

		#region States shortcuts

		

		#endregion
	}
}
