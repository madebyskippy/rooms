using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class furniture : MonoBehaviour {

	[SerializeField] Vector2 dim;
	[SerializeField] int id;
	[SerializeField] Vector2 placement;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int getid(){
		return id;
	}

	public Vector2 getdim(){
		return dim;
	}

	public void setplace(Vector2 p){
		placement = p;
	}

	public Vector2 getplace(){
		return placement;
	}
}
