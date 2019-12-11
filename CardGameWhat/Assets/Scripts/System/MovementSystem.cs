using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour {

	public float movementSpeed = 5f;

	private Rigidbody2D RB2D;
	private Transform MainPosition;
    public bool hasReach = false;

	void Start(){

		hasReach = false;

		RB2D = GetComponent<Rigidbody2D> ();
		MainPosition = GameObject.FindGameObjectWithTag ("EndPosition").GetComponent<Transform> ();
	}


	void FixedUpdate(){

		CardMovement ();
		FollowEndPostion ();
	}


	void CardMovement(){
		if (!hasReach) {
			RB2D.velocity = new Vector2 (movementSpeed * Time.deltaTime, 0f);

		}
	}

	void FollowEndPostion(){

		if (hasReach) {

			transform.position = Vector3.MoveTowards (transform.position, MainPosition.transform.position, movementSpeed * Time.deltaTime);
		}
	}


	void CallKillCommand(){

		StartCoroutine (KillTheCard ());
	}


	IEnumerator KillTheCard(){

		yield return new WaitForSeconds (1f);
		Destroy (gameObject);
	}


	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("EndPosition")) {

			hasReach = true;
			CallKillCommand ();
		}

	}

}
