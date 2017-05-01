using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rooms : MonoBehaviour {

	/* 
	 * put the assets in
	 * make the assets. 
	 */

	[SerializeField] bool onGrid;

	[SerializeField] GameObject prefab;
	[SerializeField] GameObject[] furniture;
	[SerializeField] GameObject[] rug;
	[SerializeField] GameObject[] windowL;
	[SerializeField] GameObject[] windowR;

	[SerializeField] Transform[] placements;
	[SerializeField] Transform[] placements_wall_L;
	[SerializeField] Transform[] placements_wall_R;

	[SerializeField] GameObject parent;

	[SerializeField] GameObject[] sliders;

	Vector2[,] tiles;
	bool[,] filled;
	Vector2 limits = new Vector2(5,5);
	Vector2[] spots;
	List<Vector2> availablespots;

	Sprite[] floor;
	Sprite[] wall;

	int numfurniture = 4;
	List<GameObject> furnitures; //in the room

	int numrug = 1;
	List<GameObject> rugs;

	int numwindow = 2;
	List<GameObject> windows;

	float[] weights;

	bool generated;

	// Use this for initialization
	void Start () {
		generated = false;
//		Debug.Log ("start");
		weights = new float[]{1f,1f,1f};
		tiles = new Vector2[5,5];
		filled = new bool[5, 5];
		spots = new Vector2[(int)limits.x * (int)limits.y];
		availablespots = new List<Vector2> ();

		for (int i = 0; i < tiles.GetLength(0); i++) {
			for (int j = 0; j < tiles.GetLength(1); j++) {
				tiles [i, j] = new Vector2 (i, j);
				filled [i, j] = false;
				spots [tiles.GetLength (0) * i + j] = new Vector2 (i, j);
//				Debug.Log (tiles [i, j].x + ", " + tiles [i, j].y);
			}
		}
		availablespots.AddRange (spots);

		wall = Resources.LoadAll<Sprite>  ("wall");

//		numfurniture = Random.Range (3, 6);
//		numrug = Random.Range (0, 2); //either 0 or 1 rugs
//		numwindow = 1;//Random.Range(1,3); //1-2 windows
//
//
//		if (onGrid) {
//			numwindow = 0;
//		}
//		generate ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (!generated) {
				numfurniture = Random.Range (3, 6);
				numrug = Random.Range (0, 2); //either 0 or 1 rugs
				numwindow = 1;//Random.Range(1,3); //1-2 windows

				if (onGrid) {
					numwindow = 0;
				}
				generate();

				generated = true;
				sliders [0].SetActive (false);
				sliders [1].SetActive (false);
				sliders [2].SetActive (false);
			} else {
				SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
			}
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			addfurniture ();
		}
		
	}

	//eeeeh not really working with restart the way i want it to
	void clear(){
		for (int i = 0; i < furnitures.Count; i++) {
			Destroy (furnitures [i]);
		}
		for (int i = 0; i < rugs.Count; i++) {
			Destroy (rugs [i]);
		}
		for (int i = 0; i < windows.Count; i++) {
			Destroy (windows [i]);
		}
		furnitures.Clear ();
		rugs.Clear ();
		windows.Clear ();
	}

	void generate(){
		Debug.Log ("generating");
		furnitures = new List<GameObject>();
		rugs = new List<GameObject> ();
		windows = new List<GameObject> ();

		while (furnitures.Count < numfurniture) {
			addfurniture ();
			if (furnitures.Count >= numfurniture) {
				orderfurniture ();
			}
		}

		while (rugs.Count < numrug) {
			addrug ();
		}

		while (windows.Count < numwindow) {
			addwindow ();
		}
	}

	void addwindow(){
		Debug.Log ("added window");
		GameObject temp;
		Vector2 p;
		if (Random.Range(0f,1f)<0.5f){//windows.Count % 2 == 0) {
			temp = windowL[Random.Range(0,windowL.Length)];
			p = placements_wall_L [Random.Range (0, placements_wall_L.Length)].transform.position;
		} else {
			temp = windowR[Random.Range(0,windowR.Length)];
			p = placements_wall_R [Random.Range (0, placements_wall_R.Length)].transform.position;
		}
		temp.GetComponent<SpriteRenderer> ().sortingOrder = -1;
		GameObject t = Instantiate (temp, p, Quaternion.identity);
		windows.Add (t);
	}

	void addrug(){
		Debug.Log ("added rug");
		GameObject temp = rug[Random.Range(0,rug.Length)];
		int w = (int)temp.GetComponent<furniture> ().getdim ().x;
		int h = (int)temp.GetComponent<furniture> ().getdim ().y;


		int index = Random.Range (0, availablespots.Count);
		Vector2 p = availablespots [index];
		if ((p.x < (int)limits.x - w + 1) && (p.y < (int)limits.y - h + 1)) {
			GameObject r = Instantiate (temp);//, pos, Quaternion.identity);
			Vector3 pos;
			if (onGrid) {
				pos = new Vector3 (p.x/2f, 0f, p.y/-2f);
				r.transform.parent = parent.transform;
				r.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, 0));
				r.transform.localScale = Vector3.one;
				r.transform.localPosition = pos;
			} else {
				pos = placements [(int)p.x * 5 + (int)p.y].position;
				r.transform.position = pos;
				r.GetComponent<SpriteRenderer>().sortingOrder = -1;
			}
			rugs.Add (r);
		}
	}

	void addfurniture(){
		int findex = Random.Range (2, furniture.Length);
		if (furnitures.Count < 1) {	//always one bed
			findex = Random.Range (0, 2);
		} else {
			float probability = Random.Range (0f, 1f);
			float totalweight = weights [0] + weights [1] + weights [2];
			if (probability < weights [0] / totalweight) {
				//lamp
				Debug.Log("lamp");
				findex = 2+Random.Range(0,2);
			} else if (probability < weights [1] / totalweight) {
				//1x1
				Debug.Log("end");
				findex = 4+Random.Range(0,2);
			} else {
				//2x1
				Debug.Log("dresser");
				findex = 6+Random.Range(0,2);
			}
		}
		GameObject temp = furniture[findex];
		int w = (int)temp.GetComponent<furniture> ().getdim ().x;
		int h = (int)temp.GetComponent<furniture> ().getdim ().y;

		bool placed = false;

		Vector2 p = new Vector2(0,0);

		for (int k = 0; k < limits.x * limits.y; k++) {
//			int x = Random.Range (0, (int)limits.x - w+1);
//			int y = Random.Range (0, (int)limits.y - h+1);

			//pick a random spot
			int index = Random.Range (0, availablespots.Count);
			p = availablespots [index];
			availablespots.RemoveAt (index);

			//if the spot will fit the furniture w/o going out of the room, continue
			if ((p.x < (int)limits.x - w + 1) && (p.y < (int)limits.y - h + 1)){
				bool conflict = checkoverlap ((int)p.x, (int)p.y, w, h);
				if (!conflict) {

					//		Debug.Log (w+", "+h+": "+x + ", " + y);
					GameObject f = Instantiate (temp);//, pos, Quaternion.identity);
					Vector3 pos;
					if (onGrid) {
//						Debug.Log ("placing on grid");
						pos = new Vector3 (p.x/2f, 0f, p.y/-2f);
						f.transform.parent = parent.transform;
						f.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, 0));
						f.transform.localScale = Vector3.one;
						f.transform.localPosition = pos;
					} else {
						pos = placements [(int)p.x * 5 + (int)p.y].position;
						f.transform.position = pos;
					}

//					Debug.Log (p.x + ", " + p.y);

					for (int i = (int)p.x; i < (int)p.x + w; i++) {
						for (int j = (int)p.y; j < (int)p.y + h; j++) {
							filled [i, j] = true;
							//				Debug.Log (i + ", " + j + ": filled");
						}
					}
					placed = true;
					furnitureplaced (f,p,w,h);
					break;
				}
			}
		}
		availablespots.Clear ();
		availablespots.AddRange (spots);
	}

	void furnitureplaced(GameObject f, Vector2 p, int w, int h){
		furnitures.Add (f);
		furnitures [furnitures.Count - 1].GetComponent<furniture> ().setplace (p);
		if(!onGrid){
			int order = Mathf.Max ((int)p.x+w, (int)p.y+h);
//			furnitures [furnitures.Count - 1].GetComponent<SpriteRenderer> ().sortingOrder = (int)p.x+w+((int)p.y+h)*5;//order;
		}
	}

	bool checkoverlap(int x, int y, int w, int h){
		bool full = false;
		for (int i = x; i < x + w; i++) {
			for (int j = y; j < y + h; j++) {
				if (filled [i, j]) {
					full = true;
					break;
				} else {
				}
			}
		}

		return full;
	}

	void orderfurniture(){
		for (int i = 1; i < furnitures.Count; i++) {
			//for each furniture, check where it comes in order for the ones previous
			int index = i; //index it's right behind
//			Debug.Log("----- now checking "+furnitures[i].name);
			for (int j = i-1; j > -1; j--) {
//				Debug.Log ("checking " + furnitures [i].name + " against " + furnitures [j].name);
				if (isInFront (furnitures[i],furnitures[j])) {
//					Debug.Log (furnitures[i].name+" is in front of "+furnitures[j].name);
//					break; //only breaks out of the inner loop
				} else {
					//if it's behind it, send it to the index before that in the array
//					Debug.Log(furnitures[i].name+" is behind "+furnitures[j].name);
					//move it forward
					index = j;
				}
			}
			GameObject t = furnitures [i];
			furnitures.RemoveAt (i);
			furnitures.Insert (index, t);
		}
		for (int i = 0; i < furnitures.Count; i++) {
			furnitures [i].GetComponent<SpriteRenderer> ().sortingOrder = i;
		}
	}

	bool isInFront(GameObject a, GameObject b){
		bool infront = false;
		//check bottom right corner, check top left corner
		Vector2 pa = a.GetComponent<furniture>().getplace();
		Vector2 da = a.GetComponent<furniture>().getdim();
		Vector2 pb = b.GetComponent<furniture>().getplace();
		Vector2 db = b.GetComponent<furniture>().getdim();

		if (pa.y >= (pb.y + db.y)) {
			return true;
		}
		if (pa.x >= (pb.x + db.x)) {
			return true;
		}

		return infront;
	}

	public void adjustWeightLamp(float v){
		weights [0] = v;
	}
	public void adjustWeight1(float v){
		weights [1] = v;
	}
	public void adjustWeight2(float v){
		weights [2] = v;
	}
}
