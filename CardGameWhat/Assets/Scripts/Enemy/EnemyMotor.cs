using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyMotor : MonoBehaviour {

	public int health;
	public int enemyDamageAmount;
	public Slider healthMeter;
	public Text healthamountText;
	public Image attackMeter;
	public GameObject AM;
	public GameObject flashDamage;
	public GameObject wholeAttackMeter;
	public GameObject WAM;
	public Text attackRangeText;
	public GameObject ArmorText;
	public GameObject placeholderText;
	private SpriteRenderer SR;
	public static bool stopAttacking = false;
	private bool hasDied = false;
	private float attackTimer = 3f;
	private GameManager gm;



	public GameObject nextLevelScreen;
	public GameObject nextLevelButton;
	public Sprite[] enemyPortraits;
	private int enemySlot;
	private int enemyHealthSlot;
	public int enemyHealthMin, enemyHealthMax;
	private int enemyDamageSlot;
	public int enemyDamageMin, enemyDamageMax;



	void Start(){

		DOTween.Init ();
		attackRangeText.text = "" + enemyDamageAmount;
		healthamountText.text = "" + health;
		healthMeter.maxValue = health;
		healthMeter.value = health;
		hasDied = false;
		stopAttacking = false;
		SR = GetComponent<SpriteRenderer> ();
		gm = GameObject.FindGameObjectWithTag ("Player").GetComponent<GameManager> ();
	}

	void Update(){

		if (health <= 0 && !hasDied) {

			Death ();
			hasDied = true;
			gm.HasMatchEnded = true;
			gm.CancelInvoke ();
			stopAttacking = true;
			gm.LoadNextBattle ();
		
		}


		if (!stopAttacking) {
			attackTimer -= Time.deltaTime;
			attackMeter.fillAmount = attackTimer / 2;


			if (attackTimer <= 0) {

				attackTimer = 3f;
				gm.TakeDamage (enemyDamageAmount);
				Debug.Log ("Attack");

			}

		}
	}


  public void TakeDamage(int score){


		if (score == 0) 
		{
			health = health - score;
			healthamountText.text = "0";
			return;
		}

		health = health - score;
		healthMeter.value = health;
		healthamountText.text = "" + health;


	}

	void DamageFromPlayer(){


		StartCoroutine (DamageFlash ());

	}


	IEnumerator DamageFlash(){


		Color32 whateverColor = new Color32(242,36,65,255);
		for(int i = 0; i < 3; i++)
		{
			SR.material.color = Color.white;
			yield return new WaitForSeconds (.1F);
			SR.material.color = whateverColor;
			yield return new WaitForSeconds (.1F);
		}
		SR.material.color = Color.white;


	}


	void Death(){

		StartCoroutine (IsDead ());

	}

	IEnumerator IsDead(){

		Color32 whateverColor = new Color32(242,36,65,255);
		for(int i = 0; i < 3; i++)
		{
			SR.material.color = Color.white;
			yield return new WaitForSeconds (.1F);
			SR.material.color = whateverColor;
			yield return new WaitForSeconds (.1F);
		}
		SR.material.color = Color.white;

		yield return new WaitForSeconds (.5f);
		FindAllCards ();
		ArmorText.SetActive (false);
		AM.SetActive (false);
		WAM.SetActive (false);
		healthMeter.gameObject.SetActive (false);
		wholeAttackMeter.SetActive (false);
		SR.material.color = new Color32 (255, 255, 255, 0);
		placeholderText.SetActive (true);
	}



	public void LoadTheNextEnemy(){
		gm.Cards.Clear ();
		StartCoroutine (LoadingEnemy ());
		healthMeter.gameObject.SetActive (true);
		wholeAttackMeter.SetActive (true);
		ArmorText.SetActive (true);
		AM.SetActive (true);
		WAM.SetActive (true);
		placeholderText.SetActive (false);
		SR.material.color = new Color32 (255, 255, 255, 255);
		nextLevelButton.transform.DOPunchScale(new Vector3(1.1f,1.1f,1f),.25f,1,1).SetEase(Ease.OutQuart);
	}

	IEnumerator LoadingEnemy(){

		yield return new WaitForSeconds (.5f);
		nextLevelScreen.transform.DOLocalMoveY (1400, 1.2f).SetEase (Ease.InBounce);
		gm.ResetStats ();
		enemySlot = Random.Range (0, enemyPortraits.Length);
		SR.sprite = enemyPortraits [enemySlot];
		enemyHealthSlot = Random.Range (enemyHealthMin,enemyHealthMax);
		enemyDamageSlot = Random.Range (enemyDamageMin,enemyDamageMax);
		enemyDamageAmount = enemyDamageSlot;
		health = enemyHealthSlot;
		yield return new WaitForSeconds (1f);
		attackRangeText.text = "" + enemyDamageAmount;
		healthamountText.text = "" + health;
		healthMeter.maxValue = health;
		healthMeter.value = health;
		attackTimer = 3f;
		hasDied = false;
		stopAttacking = false;

	}


	void FindAllCards(){


		GameObject[] Cards = GameObject.FindGameObjectsWithTag("Cards");
		for (int i = 0; i < Cards.Length; i++) {

			Destroy (Cards [i]);
		}
	}




	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("AttackCard")) {

			DamageFromPlayer ();
			Instantiate (flashDamage, transform.position, Quaternion.identity);
		}

	}
}
