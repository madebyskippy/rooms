using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineobject : MonoBehaviour {

	[SerializeField] Mesh[] mesh = new Mesh[2];

	int duration = 100; //how long it takes to switch, in steps
	int index;
	int current; //which mesh you're on
	int target; //which one you're moving to

	LineRenderer line;

	List<Vector3[]> views;

	Vector3[] verts;

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer> ();
		views = new List<Vector3[]> ();
		for (int i=0; i<mesh.Length; i++){
			Vector3[] view = new Vector3[mesh[0].vertices.Length + 1];
			if (i==0)
				verts = new Vector3[mesh[0].vertices.Length + 1];
			for (int j = 0; j < view.Length-1; j++) {
				if (i==0)
					verts [j] = mesh[i].vertices [j];
				view [j] = mesh[i].vertices [j];
			}
			if (i==0)
				verts [verts.Length - 1] = mesh[i].vertices [0];
			view [view.Length - 1] = mesh[i].vertices [0];
			views.Add (view);
		}
		line.positionCount = verts.Length;
		line.SetPositions (views[0]);

		current = 0;
		index = 0;
		target = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void step(int direction){
		//figure out what to change the points to
		index += direction;

		Debug.Log (index);
		if (index < 0) {
			index = 0;
		} if (index > duration) {
			index = 0;
			current = target;
		}


		//actually change the points
		for (int i = 0; i < verts.Length; i++) {
			//lerp between current and the next one ..... given index/duration
			verts[i] = Vector3.Lerp(views[current][i], views[target][i], (float)index/(float)duration);
		}
		line.SetPositions (verts);
	}
}
