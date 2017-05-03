using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class cube_chara : MonoBehaviour {

	/*
	 * todo:
	 */

	[SerializeField] GameObject limbs;
	[SerializeField] Animator animator;


	private float radius;

	private float speed=0.05f;
	private float stunTime = 0.25f;
	private float stunTimer;
	private bool isStunned;

	private bool isFree;

	private Animator anim;

	int horiz, vert;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		changeMode (-1);
		stunTimer = 0f;
		isStunned = false;
		isFree = false;
		horiz = 0;
		vert=0;
	}

	// Update is called once per frame
	void Update () {
		//a terrible way to code controls
		if (Input.GetKeyDown (KeyCode.W)) {
			vert = 1;
		}if (Input.GetKeyDown (KeyCode.A)) {
			horiz = -1;
		}if (Input.GetKeyDown (KeyCode.S)) {
			vert = -1;
		}if (Input.GetKeyDown (KeyCode.D)) {
			horiz = 1;
		}
		if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S)) {
			vert = 0;
		}if (Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.D)) {
			horiz = 0;
		}

		Vector3 increment = new Vector3 (horiz * speed, 0f, vert * speed);

		float dist = Vector3.Distance (Vector3.zero, transform.position + increment);

		Vector3 dir = increment;
		dir.Normalize ();
		dir *= -1;

		//to make sure it doesn't go outside of the cube but also
		//tHIS ISN'T WORkrign nrgiht nowwwwwwww
		Vector3 temp = transform.position + increment;
		temp.z = Mathf.Clamp (temp.z, -0.9f,0.9f);
		temp.x = Mathf.Clamp (temp.x, -0.9f,0.9f);
		transform.position = temp;
	}

	public void setRadius(float r){
		radius = r;
	}

	public void setFree(bool b){
		isFree = b;
	}

	//for animations
	public void changeMode(int m){
		anim.SetInteger ("mode", m);
		if (m==3){
			isStunned = false;
		}
	}
}
