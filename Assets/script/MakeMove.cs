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
	
		if (currentDistance <= previousDistance) {
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

		}
		previousDistance = currentDistance;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag.Equals ("Map")) {
			if (directionToGo == Direction.UP || directionToGo == Direction.DOWN) {
				if (buildingToDestroy.transform.position.x > transform.position.x) {
					directionToGo = Direction.LEFT;	
					transform.SetPositionAndRotation (transform.position,Quaternion.AngleAxis(-90,new Vector3(0,0,1)));
				} else {
					directionToGo = Direction.RIGHT;
					transform.SetPositionAndRotation (transform.position,Quaternion.AngleAxis(90,new Vector3(0,0,1)));
				}			
			} else {
				if (buildingToDestroy.transform.position.y > transform.position.y) {
					directionToGo = Direction.DOWN;	
					transform.SetPositionAndRotation (transform.position,Quaternion.AngleAxis(0,new Vector3(0,0,1)));
				} else {
					directionToGo = Direction.UP;
					transform.SetPositionAndRotation (transform.position,Quaternion.AngleAxis(180,new Vector3(0,0,1)));
				}			
			}
		}
	}
}
