using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineobject : MonoBehaviour {

	[SerializeField] Mesh mesh;

	LineRenderer line;

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer> ();
		line.positionCount = mesh.vertices.Length+1;
		line.SetPositions (mesh.vertices);
		line.SetPosition (mesh.vertices.Length, mesh.vertices [0]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
