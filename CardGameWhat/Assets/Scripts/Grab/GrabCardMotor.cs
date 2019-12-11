using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCardMotor : MonoBehaviour {

	public float distance = 10f;
	public float movementSpeed = 5f;
	public bool canTouch = true;
	private bool hasTouchThis = false;
	public Transform MainPosition;



	void Start(){

		hasTouchThis = false;
	}


	void Update(){

		ReturnToPosition ();
	    CardMovement ();

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


		if (canTouch) {
			Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance);
			Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
			transform.position = objPosition;

		}
	}


	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("Enemy")) {

			Debug.Log ("Card use");
			Destroy (gameObject);
		}

	}
}
