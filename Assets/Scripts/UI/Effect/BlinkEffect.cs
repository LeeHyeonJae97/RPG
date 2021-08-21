using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.Animation
{
	public class BlinkEffect : Effect
	{
		[Required()]
		[SerializeField] private Graphic _target;
		[SerializeField] private float _duration;

		[Description()]
		[SerializeField] private string _description = "Invoke Blink effect on target";

		public override void Invoke()
		{
			Sequence sequence = DOTween.Sequence();
			Tween[] tweens = new Tween[2];
			tweens[0] = _target.DOFade(0, _duration / 2);
			tweens[1] = _target.DOFade(1, _duration / 2);

			for (int i = 0; i < tweens.Length; i++)
				sequence.Append(tweens[i]);
		}
	}
}
