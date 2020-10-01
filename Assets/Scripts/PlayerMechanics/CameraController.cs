using System;
using UnityEngine;

namespace NeonBlaze.PlayerMechanics
{
	[RequireComponent(typeof(Camera))]
	public sealed class CameraController : MonoBehaviour
	{
		public Transform Target
		{
			get => m_Target;
			set => m_Target = value;
		}

		[SerializeField] private Transform m_Target;
		[SerializeField] private Vector3 m_CameraOffset;
		[SerializeField] [Range(0f, 1000f)] private float m_AheadMultiplier = 1f;
		[SerializeField] [Range(0f, 3f)] private float m_FollowSmoothness = 1f;

		private Vector3 mPreviousPosition;

		private void Start()
		{
			OnValidate();
		}

		private void LateUpdate()
		{
			if (Target == null) return;
			if (transform.localPosition.sqrMagnitude < 0.001f) return;

			m_FollowSmoothness = Mathf.Max(0.001f, m_FollowSmoothness);

			var position = transform.position;
			var velocity = m_Target.position - mPreviousPosition;

			var followOffset = velocity * m_AheadMultiplier;
			var targetPosition = Target.position + m_CameraOffset + followOffset;
			position = Vector3.Lerp(position, targetPosition,
				1 / m_FollowSmoothness * Time.smoothDeltaTime);
			transform.position = position;

			mPreviousPosition = m_Target.position;
		}

		private void OnValidate()
		{
			Target = m_Target;
		}
	}
}