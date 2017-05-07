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
		if (index == 0) {
			if (current + direction < 0) {
//				Debug.Log ("nothing before this");
//				return;
				target = views.Count-1;
			}else if (current + direction > views.Count-1) {
//				Debug.Log ("nothing after this");
//				return;
				target = 0;
			}else {
				target = current + direction;
			}
//			Debug.Log ("switching target to "+target+" from current "+current+" and direction "+direction);
		}

		if (target > current) {
			if (target > current + 1) {
				index -= direction;
			} else {
				index += direction;
			}
		} else {
			if (target < current - 1) {
				index += direction;
			} else {
				index -= direction;
			}
		}

		//actually change the points
		for (int i = 0; i < verts.Length; i++) {
			//lerp between current and the next one ..... given index/duration
			verts[i] = Vector3.Lerp(views[current][i], views[target][i], (float)index/(float)duration);
		}
		line.SetPositions (verts);

		if (index >= duration) {
			current = target;
			index = 0;
//			Debug.Log ("switching current to "+current+", index to "+index+", "+views.Count);
		}
	}
}
