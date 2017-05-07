using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linemanager : MonoBehaviour {

	[SerializeField] lineobject[] stuffs;

	bool move;
	int direction;


	// Use this for initialization
	void Start () {
		move = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			move = true;
			direction = -1;
		}if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			move = true;
			direction = 1;
		}if (Input.GetKeyUp (KeyCode.RightArrow)) {
			move = false;
		}if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			move = false;
		}

		if (move) {
			step(direction);
		}
	}

	void step(int direction){
		for (int i = 0; i < stuffs.Length; i++) {
			stuffs [i].step (direction);
		}
	}
}
