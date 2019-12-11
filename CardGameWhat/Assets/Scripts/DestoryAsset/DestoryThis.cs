using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryThis : MonoBehaviour {

	public float deathTimer;
	private SpriteRenderer SR;


	void Start(){

		SR = GetComponent <SpriteRenderer> ();
		Destroy (gameObject, deathTimer);
		Flash ();
	}



	void Flash(){


		StartCoroutine (DamageFlash ());

	}


	IEnumerator DamageFlash(){


		Color32 whateverColor = new Color32(255,255,255,135);
		for(int i = 0; i < 3; i++)
		{
			SR.material.color = Color.white;
			yield return new WaitForSeconds (.1F);
			SR.material.color = whateverColor;
			yield return new WaitForSeconds (.1F);
		}
		SR.material.color = Color.white;

	}
}
