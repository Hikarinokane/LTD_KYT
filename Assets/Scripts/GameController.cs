using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	private GameObject gc;

	public GameObject mainCamera;
	public GameObject mapCamera;
	private Vector3 mapCameraPos;

	public GameObject buttons;
	private bool isButtons = false;

	public GameObject info;

	private GameObject[] plob = new GameObject[4];
	private GameObject[] icon = new GameObject[4];
	private PlayerDef.player[] p = new PlayerDef.player[4];
	private Vector3 plpos;
	private float moveSpeed = 1.0f;
	private int charaOrder = 3;	//初期値3以上
	private bool isNext = false;
	private bool isMove = false;

	private int count;

	public Transform iventParent;
	private List<PlayerDef.ivent> iv = new List<PlayerDef.ivent> ();
	private List<int> iventOrder = new List<int> ();
	private List<string[]> iventStr = new List<string[]> ();

	private GameObject dice;
	private Die_d6 die_d6;
	private bool isDice = false;
	public GameObject dicePrefab;
	public GameObject box;

	public GameObject finishButton;

	private float rotTime = 3.0f;
	private float lifeTime = 5.0f;
	private float time = 0.0f;

	private float invokeTime = 1.0f; 
	private int initialPos = 0;

	private bool isDebug = false;	//デバッグ用（加速します）

	// Use this for initialization
	void Start () {
		if (isDebug)
			debugMode ();

		GetComponent<FileLoad2> ().readData ();
		iventOrder = GetComponent<FileLoad2> ().IventOrder;
		iventStr = GetComponent<FileLoad2> ().IventStr;

//		Debug.Log ("order:" + iventOrder.Count + ",str:" + iventStr.Count);

		foreach (Transform child in iventParent) {
			if (iv.Count > iventStr.Count - 1)
				errorFinish ();
			PlayerDef.ivent tmp = new PlayerDef.ivent ();
			tmp.Ivob = child.gameObject;
			tmp.Ivstr = iventStr [iv.Count];
			iv.Add (tmp);
		}

		GetComponent<Generator2> ().generater (iv [iventOrder [initialPos]].Ivob.transform.position);
		plob = GetComponent<Generator2> ().Player;
		icon = GetComponent<Generator2> ().Icon;
		for (int i = 0; i < 4; i++) {
			p [i].Plob = plob [i];
			p [i].Icon = icon [i];
			p [i].Plpo = initialPos;
		}
		info.SetActive (false);

		addCharaOrder ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isDice)
			diceRotation ();
		
		if (isButtons)
			buttons.SetActive (true);
		else
			buttons.SetActive (false);

		if (info.activeSelf && Input.GetMouseButtonDown (0)) {
			if (isNext) {
				isNext = false;
				Invoke ("addCharaOrder", invokeTime);
				destroyInfo ();
				isButtons = false;
			} else if (isMove) {
				isMove = false;
				playerMove ();
				destroyInfo ();
				isButtons = false;
			} else {
				destroyInfo ();
				isButtons = true;
			}
		}
			
		plpos = p [charaOrder].Plob.transform.position;
		p [charaOrder].Icon.transform.position = plpos + Vector3.down * 0.55f;
		box.transform.position = plpos + Vector3.down * 0.45f + Vector3.back * 1.2f;
		mainCamera.transform.position = plpos + Vector3.back * 2.2f;


		if (mapCamera.GetComponent<Camera> ().enabled) {
			mapCameraMove ();
		}
	}



	public void diceFunction(){
		isButtons = false;
		dice = Instantiate (dicePrefab) as GameObject;
		dice.transform.position = box.transform.position + Vector3.up * 0.7f;
		dice.transform.localScale = new Vector3 (0.12f, 0.12f, 0.12f);
		die_d6 = dice.GetComponent<Die_d6> ();
		dice.transform.rotation = Random.rotation;

		isDice = true;
	}
	public void mapFunction(){
		isButtons = false;
		mapCamera.transform.position = plpos + Vector3.up * 5.0f;
		mapCamera.GetComponent<Camera> ().enabled = true;
	}

	public void mapCameraMove(){
		mapCameraPos = mapCamera.transform.position;
		if (Input.GetMouseButtonDown (0)) {
			isButtons = true;
			mapCamera.GetComponent<Camera> ().enabled = false;
		} else if (Input.GetKey ("up") && mapCamera.transform.position.z < 29.0f)
			mapCameraPos.z += 10 * Time.deltaTime;
		else if (Input.GetKey ("down") && mapCamera.transform.position.z > -29.0f)
			mapCameraPos.z -= 10 * Time.deltaTime;
		else if (Input.GetKey ("right") && mapCamera.transform.position.x < 29.0f)
			mapCameraPos.x += 10 * Time.deltaTime;
		else if (Input.GetKey ("left") && mapCamera.transform.position.x > -29.0f)
			mapCameraPos.x -= 10 * Time.deltaTime;
		mapCamera.transform.position = mapCameraPos;
	}

	public void diceRotation(){
		time += Time.deltaTime;
		if (time < lifeTime) {
			if (time < rotTime) {
				dice.transform.Rotate (new Vector3 (360, 360, 360) * Time.deltaTime, Space.Self);
			} else {
				dice.GetComponent<Rigidbody> ().useGravity = true;
			}
		} else {
			count = p [charaOrder].Plpo + 1;
			p [charaOrder].Plpo += die_d6.value;

			isMove = true;
			printInfo ("Dice:" + die_d6.value);

			Destroy (dice);
			time = 0.0f;
			isDice = false;
		}

	}

	public void playerMove(){
		if (count <= p [charaOrder].Plpo && count < iventOrder.Count) {
			iTween.MoveTo (p [charaOrder].Plob, iTween.Hash (
				"position", iv [iventOrder [count]].Ivob.transform.position + 0.65f * Vector3.up,
				"time", moveSpeed,
				"oncomplete", "complete", 
				"oncompletetarget", this.gameObject, 
				"easeType", "linear"
			//"space", Space.worldでグローバル座標系で移動
			));
			count++;
		} else if (goalCheck ()) {
			finishButton.SetActive (true);
		} else if (iventOrder.Count <= count) {
			Debug.Log ("goal");
			isNext = true;
			printInfo ("Player" + (charaOrder + 1) + " Goal!");
		} else {
			Debug.Log ("kyt");
			isNext = true;
			printKYT ();
		}
	}

	public void complete(){
		playerMove ();
	}

	public void addCharaOrder(){
		for (int i = 0; i < 4; i++) {
			if (i != charaOrder) {
				Vector3 tmp = p [i].Plob.transform.position;
				tmp.z -= 0.1f;
				p [i].Plob.transform.position = tmp;
				printInfo ("Player" + (charaOrder + 1) + " Turn!");
			}
		}

		if (charaOrder < 3)
			charaOrder++;
		else if (charaOrder >= 3)
			charaOrder = 0;
		
		for (int i = 0; i < 4; i++) {
			if (i != charaOrder) {
				Vector3 tmp = p [i].Plob.transform.position;
				tmp.z += 0.1f;
				p [i].Plob.transform.position = tmp;
				printInfo ("Player" + (charaOrder + 1) + " Turn!");
			}
		}
		if (p [charaOrder].Plpo >= iventOrder.Count - 1)
			addCharaOrder ();
	}

	public void printInfo(string str){
		Text txt;

		info.SetActive (true);
		txt = info.transform.GetChild (0).GetComponent<Text> ();
		txt.fontSize = 100;
		txt.text = str;
	}

	public void printKYT(){
		Text txt;
//		if (p [charaOrder].Plpo >= iventOrder.Count - 1)
//			addCharaOrder ();
		info.SetActive (true);
		txt = info.transform.GetChild (0).GetComponent<Text> ();
		txt.fontSize = 50;
		txt.text = iv [iventOrder [count - 1]].Ivstr [Random.Range (0, 3)];
	}

	public void destroyInfo(){
		info.SetActive (false);
	}

	public bool goalCheck(){
		for (int i = 0; i < 4; i++) {
			if (iventOrder.Count > p [i].Plpo + 1)
				return false;
		}
		return true;
	}

	public void gameEnd(){
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
//		#else
//		Application.quit();
		#endif
	}

	public void errorFinish(){
		#if UNITY_EDITOR
		Debug.LogError ("ERROR!");
//		#else
//		Application.quit();
		#endif
	}

	public void debugMode(){
		moveSpeed = 0.1f;
		rotTime = 0.0f;
		lifeTime = 1.5f;
		invokeTime = 0.0f;
		initialPos = 38;
	}

}
