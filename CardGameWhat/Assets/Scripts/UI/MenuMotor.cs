using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class MenuMotor : MonoBehaviour {


	public string sceneName;
	public GameObject startButton;
	public Animator fader;




	void Start(){

		DOTween.Init ();
	}


	public void LoadLevel(){


		startButton.transform.DOPunchScale (new Vector3 (1, 1, 1), .5f, 10, 1).SetEase (Ease.InOutQuart);
		StartCoroutine (BeingLoading ());

	}

	IEnumerator BeingLoading(){
		
		yield return new WaitForSeconds (.5f);
		fader.SetBool ("SetFader", true);
	
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene (sceneName);

	}

}
