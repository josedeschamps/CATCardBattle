using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour {

	public int playerHealth = 30;
	public int startingGem = 0;
	public int gemCost;
	public int gemAmount;
	public float resourceGainSpeed;
	private float timer;
	public Slider playerHealthMeter;
	public Slider playerGemMeter;
    public Slider playerExpMeter;
    private int expPoints = 0;
    public Text addExpPointFX;
	public Text healthAmountText;
	public Text gemText;
	public GameObject healEffect;
	public GameObject gemEffect;
	public GameObject PlacedHolderText;
	private SpriteRenderer SR;
	private bool hasDied = false;
	public bool HasMatchEnded = false;



	//---------Card System---//
	public GameObject[] Deck;
	public List<GameObject> Cards= new List<GameObject> ();
	public Transform startLocation;
	public float SpawnTime;
	public float SpawnRate;
	//----Card System Ends---//



	//-----Next Level System--//
	public GameObject nextBattle;






	void Start(){

		DOTween.Init ();
		hasDied = false;
		HasMatchEnded = false;
		startingGem = 0;
		timer = 0;
		healthAmountText.text = "" + playerHealth;
		playerHealthMeter.value = playerHealth;
		playerGemMeter.maxValue = resourceGainSpeed;
		SR = GetComponent<SpriteRenderer> ();
		LoadCardDeck ();
		InvokeRepeating ("SpawningCards", SpawnTime, SpawnRate);




	}


	void Update(){

		if (playerHealth <= 0 && !hasDied) {

			Death ();
			EnemyMotor.stopAttacking = true;
			hasDied = true;
			HasMatchEnded = true;
			CancelInvoke ();


		}

		GemTimer ();



	}




	public void TakeDamage(int score){


		if (score == 30) 
		{
			playerHealth = playerHealth - score;
			return;
		}



		playerHealth = playerHealth - score;
		playerHealthMeter.value = playerHealth;
		healthAmountText.DOText ("" + playerHealth, 1, false,ScrambleMode.Numerals).SetEase (Ease.InOutQuad);
		healthAmountText.transform.DOPunchScale(new Vector3(1.1f,1.1f,1f),.5f,1,1).SetEase(Ease.InBounce);
		DamageFromEnemy ();


	}


	public void HealDamage(int score){


		if (score == 30) 
		{
			playerHealth = playerHealth + score;
			return;
		}

		playerHealth = playerHealth +score;
		playerHealthMeter.DOValue (playerHealth, .25f, false).SetEase (Ease.InOutCirc);
		healthAmountText.text = "" + playerHealth;

	}


	public void AddResource(int gem){


		if (gem == 20) 
		{
			startingGem = startingGem + gem;
			return;
		}

		startingGem = startingGem + gem;
		gemText.text = "" + startingGem;
		gemText.transform.DOPunchScale(new Vector3(1.2f,1.5f,1f),.5f,2,1).SetEase(Ease.InBounce);

	}

	public void CostResource(int gem){


		if (gem == 0) 
		{
			startingGem = startingGem - gem;
			return;
		}

		startingGem = startingGem - gem;
		gemText.DOText ("" + startingGem, .3f, false,ScrambleMode.Numerals).SetEase (Ease.InOutQuad);


	}

	void GemTimer(){


		if (!HasMatchEnded) {

			timer += Time.deltaTime;
			playerGemMeter.value = timer;
		


			if (timer > resourceGainSpeed) {

				timer = 0;
				gemText.transform.localScale = new Vector3 (1, 1, 1);
				AddResource (1);

			}

		}


	}



    public void AddExp(int exp)
    {
        

        if (exp == 1)
        {
            expPoints = expPoints + exp;
            return;
        }

        expPoints = expPoints + exp;
        playerExpMeter.DOValue(expPoints, .25f, false).SetEase(Ease.InOutCirc);
        addExpPointFX.text= "+" + expPoints;
        addExpPointFX.transform.DOLocalMoveY(50f, .25f).SetEase(Ease.Flash);
        addExpPointFX.DOFade(0f, 1f).SetEase(Ease.InQuad);


    }

	public void DelayCost(){

		StartCoroutine (DelayThis ());
	}

	IEnumerator DelayThis(){
		CostResource (gemCost);
		yield return new WaitForSeconds (1f);
		AddResource (gemAmount);


	}

	void DamageFromEnemy(){


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
		PlacedHolderText.SetActive (true);

		yield return new WaitForSeconds (3f);
		SceneManager.LoadScene ("Level01");
	}




	void SpawningCards(){

		if (Cards.Count <= 0) {

			LoadCardDeck ();
		}
		int cardIndex = Random.Range (0, Cards.Count);
		Instantiate (Cards [cardIndex], startLocation.position, startLocation.rotation);
		Cards.RemoveAt (cardIndex);

	}

	void LoadCardDeck(){

		for (int i = 0; i < Deck.Length; i++) {

			Cards.Add (Deck[i]);
		}

	}


	public void LoadNextBattle(){

		StartCoroutine (LoadingTheNextBattle ());

	}



	IEnumerator LoadingTheNextBattle(){

		yield return new WaitForSeconds (2f);
		nextBattle.transform.DOLocalMoveY (0, 1.2f).SetEase (Ease.OutBounce);
        yield return new WaitForSeconds(2f);
        AddExp(50);


	}

	public void ResetStats(){

		hasDied = false;
		HasMatchEnded = false;
		playerHealth = 30;
		startingGem = 0;
		timer = 0;
		healthAmountText.text = "" + playerHealth;
		playerHealthMeter.value = playerHealth;
		gemText.text = "" + startingGem;
		gemText.transform.localScale = new Vector3 (1, 1, 1);
		LoadCardDeck ();
		InvokeRepeating ("SpawningCards", SpawnTime, SpawnRate);
	}




	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("HealCard")) {

			Instantiate (healEffect, transform.position, Quaternion.identity);

		}


		if (other.gameObject.CompareTag ("GemCard")) {

			Instantiate (gemEffect, transform.position, Quaternion.identity);

		}

	}

}
