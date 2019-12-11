using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealingEffectMotor : MonoBehaviour {

	public float deathTimer;
	private SpriteRenderer SR;


	void Start(){

		DOTween.Init ();
		SR = GetComponent <SpriteRenderer> ();
		SR.DOFade (0, 2).SetEase (Ease.InOutQuad);
		transform.DOLocalMoveY (-2, 1).SetEase (Ease.InOutQuad);
		Destroy (gameObject, deathTimer);

	}




}

