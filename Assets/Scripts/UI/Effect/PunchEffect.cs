using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

namespace UI.Animation
{
	public enum PunchEffectType { Position, AnchoredPosition, Scale, Rotation }

	public class PunchEffect : Effect
	{
		[Required()]
		[SerializeField] private RectTransform _target;
		[SerializeField] private ShakeEffectType _type;
		[SerializeField] private Vector3 _punch;
		[SerializeField] private float _duration;
		[SerializeField] private int _vibrato = 10;
		[SerializeField] private float _elasticity = 1;

		[Description()]
		[SerializeField] private string _description = "Invoke Punch effect on target";

		public override void Invoke()
		{
			switch (_type)
			{
				case ShakeEffectType.Position:
					_target.DOPunchPosition(_punch, _duration, _vibrato, _elasticity);
					break;

				case ShakeEffectType.AnchoredPosition:
					_target.DOPunchAnchorPos(_punch, _duration, _vibrato, _elasticity);
					break;

				case ShakeEffectType.Scale:
					_target.DOPunchScale(_punch, _duration, _vibrato, _elasticity);
					break;

				case ShakeEffectType.Rotation:
					_target.DOPunchRotation(_punch, _duration, _vibrato, _elasticity);
					break;
			}
		}
	}
}
