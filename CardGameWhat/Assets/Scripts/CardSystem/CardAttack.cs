using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttack : MonoBehaviour {

	public float distance = 10f;
	public float movementSpeed = 5f;
	public int cardDamageAmount;
	public int cardCost;
	public bool canTouch = true;
	private bool hasTouchThis = false;
	private bool canBeUsed = false;
	public Transform MainPosition;
	private SpriteRenderer SR;
	private GameManager gm;
	private EnemyMotor em;



	void Start(){

		hasTouchThis = false;
		canBeUsed = false;
		SR = GetComponent<SpriteRenderer> ();
		SR.material.color = new Color32 (255, 255, 255, 35);
		gm = GameObject.FindGameObjectWithTag ("Player").GetComponent<GameManager> ();
		em = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<EnemyMotor> ();

	}


	void Update(){

		ReturnToPosition ();
		CardMovement ();
		CostChecker ();

	

	}



	void CostChecker(){

		if (cardCost <= gm.startingGem) {

			canBeUsed = true;
			SR.material.color = new Color32(255, 255, 255, 255);

		} 

		else if (cardCost > gm.startingGem) {

			canBeUsed = false;
			SR.material.color = new Color32 (255, 255, 255, 35);
		}

	}

	void CardMovement(){


		if (hasTouchThis) {

			transform.position = Vector3.MoveTowards (transform.position, MainPosition.transform.position, movementSpeed * Time.deltaTime);
		}


	}

	void ReturnToPosition(){


		if (Input.GetMouseButton (0)) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction);


			if (hit != null && hit.collider != null) 
			{

				hasTouchThis = false;

			}

		}



		if (Input.GetMouseButtonUp (0)) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction);


			if (hit != null && hit.collider != null) 
			{

				hasTouchThis = true;

			}

		}


	}

	void OnMouseDrag(){


		if (canBeUsed) {

			if (canTouch) {
				Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance);
				Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
				transform.position = objPosition;

			}

		}
	}


	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("Enemy")) {

			gm.CostResource (cardCost);
			em.TakeDamage (cardDamageAmount);
			Destroy (gameObject);
		}

	}
}


