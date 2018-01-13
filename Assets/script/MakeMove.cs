using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMove : MonoBehaviour {
	
	Transform transform;
	public float speed = 30f;
	GameObject buildingToDestroy;
	enum Direction {LEFT, RIGHT, UP, DOWN};
	Direction directionToGo;
	float previousDistance = float.MaxValue;
	float currentDistance;
	bool running = true;
	public GameObject fireball;
	bool firing = false;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform> ();
		buildingToDestroy = GameObject.FindGameObjectWithTag ("BuildingToDestroy");
	
		//if the ennemy is turned to left then Go left
		directionToGo = transform.rotation.z < 0 ? Direction.LEFT : Direction.RIGHT;
	}

	// Update is called once per frame
	void FixedUpdate () {

		currentDistance = Vector3.Distance (transform.position, buildingToDestroy.transform.position);
	
		if (currentDistance <= previousDistance && running) {
			//left
			if (directionToGo == Direction.LEFT) {
				transform.position = transform.position - (Vector3.left / speed);
			} else if (directionToGo == Direction.RIGHT) {
				transform.position = transform.position - (Vector3.right / speed);
			} else if (directionToGo == Direction.DOWN) {
				transform.position = transform.position - (Vector3.down / speed);
			} else {
				transform.position = transform.position - (Vector3.up / speed);
			}

		} else if (!firing) {
			//if we arrived just stop running
			running = false;
			TurnToBuilding ();
			StartCoroutine (Fire ());
			firing = true;

		}

		previousDistance = currentDistance;
	}

	void TurnToBuilding(){
		Vector3 target = buildingToDestroy.transform.position - transform.position;
		float angle = Mathf.Atan2 (target.x, target.y) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler (0,0,-angle);

	}
	IEnumerator Fire(){
		GameObject clone = Instantiate (fireball);
		clone.transform.position = transform.position;
		clone.transform.rotation = transform.rotation;
		clone.GetComponent<Rigidbody2D>().velocity = (transform.rotation * Vector3.up);
		yield return new WaitForSeconds (5.0f);
		yield return Fire ();
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag.Equals ("Map")) {
			if (directionToGo == Direction.UP || directionToGo == Direction.DOWN) {
				if (buildingToDestroy.transform.position.x > transform.position.x) {
					directionToGo = Direction.LEFT;	
					transform.SetPositionAndRotation (transform.position, Quaternion.AngleAxis (-90, new Vector3 (0, 0, 1)));
				} else {
					directionToGo = Direction.RIGHT;
					transform.SetPositionAndRotation (transform.position, Quaternion.AngleAxis (90, new Vector3 (0, 0, 1)));
				}			
			} else {
				if (buildingToDestroy.transform.position.y > transform.position.y) {
					directionToGo = Direction.DOWN;	
					transform.SetPositionAndRotation (transform.position, Quaternion.AngleAxis (0, new Vector3 (0, 0, 1)));
				} else {
					directionToGo = Direction.UP;
					transform.SetPositionAndRotation (transform.position, Quaternion.AngleAxis (180, new Vector3 (0, 0, 1)));
				}			
			}
		} else {
			running = false;
		}
	}
}
