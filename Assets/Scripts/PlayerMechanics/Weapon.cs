using System;
using UnityEngine;

namespace NeonBlaze.PlayerMechanics
{
	[RequireComponent(typeof(Collider2D))]
	public sealed class Weapon : MonoBehaviour
	{
		public event Action<Stats> ObjectHit;

		public float Damage => m_Damage;
		public float StaminaCost => m_StaminaCost;
		public float WindUpDuration => m_WindUpDuration;
		public float HitDuration => m_HitDuration;
		public float RecoveryDuration => m_RecoveryDuration;

		private Transform Transform
		{
			get
			{
				if (mTransform == null) mTransform = transform;
				return mTransform;
			}
		}

		[SerializeField] private Vector2 m_DefaultPosition;
		[SerializeField] private Vector2 m_WindupPosition;
		[SerializeField] private Vector2 m_HitPosition;
		[SerializeField] private Vector2 m_RecoveryPosition;
		[SerializeField] private float m_Damage = 20;
		[SerializeField] private float m_StaminaCost = 20;
		[SerializeField] [Range(0, 10)] private float m_WindUpDuration = 0.5f;
		[SerializeField] [Range(0, 5)] private float m_HitDuration = 0.1f;
		[SerializeField] [Range(0, 10)] private float m_RecoveryDuration = 0.3f;

		private Collider2D mCollider;
		private Transform mTransform;

		public void WindUp()
		{
			mCollider.enabled = false;
			Transform.localPosition = m_WindupPosition;
		}

		public void Hit()
		{
			mCollider.enabled = true;
			Transform.localPosition = m_HitPosition;
		}

		public void Recover()
		{
			mCollider.enabled = false;
			Transform.localPosition = m_RecoveryPosition;
		}

		public void Show()
		{
			gameObject.SetActive(true);
			mCollider.enabled = false;
			Transform.localPosition = m_DefaultPosition;
		}

		public void Hide()
		{
			gameObject.SetActive(false);
			mCollider.enabled = false;
			Transform.localPosition = m_DefaultPosition;
		}

		private void Awake()
		{
			mCollider = GetComponent<Collider2D>();
		}

		private void Start()
		{
			Hide();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			var character = other.GetComponent<Stats>();
			if (character == null) return;

			ObjectHit?.Invoke(character);
		}
	}
}