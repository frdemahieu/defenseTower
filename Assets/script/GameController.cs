using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour {

	public SpriteRenderer tankSprite;
	public bool isGameOver = false;
	private static GameController instance;
	private List<GameObject> respawns;

	void Awake (){
		if (instance == null) { 
			instance = this;
		} else if (instance != this) {
			DestroyObject (this);
		}
	}

	// Use this for initialization
	void Start () {
		if (tankSprite == null)
			throw new UnityException ("Tank Sprite is not loaded");	

		respawns = GameObject.FindGameObjectsWithTag ("Respawn").ToList ();


		StartCoroutine (MakeTankAppear(5.0f));
		    
	}

	IEnumerator MakeTankAppear(float time){

		GameObject respawn = respawns.ElementAt (Random.Range (0, 4));
		Instantiate (tankSprite,respawn.transform.position,respawn.transform.rotation);

		yield return new WaitForSeconds(time);
		yield return MakeTankAppear (time);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	public GameController Instance {
		get{ return instance;}
		set{ instance = value; }
	}
}
