using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube_manager : MonoBehaviour {

	[SerializeField] GameObject room;

	float spin;

	// Use this for initialization
	void Start () {
		spin = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			spin = 1f;
		}if (Input.GetKeyDown (KeyCode.RightArrow)) {
			spin = -1f;
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.RightArrow)) {
			spin = 0;
		}

		room.transform.Rotate (new Vector3 (0f, spin, 0f));
	}
}
