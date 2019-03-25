using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator2 : MonoBehaviour {
	private string[] charaName = new string[4];

	public GameObject playerPrefab;
	private GameObject[] player = new GameObject[4];

	public GameObject iconPrefab;
	private GameObject[] icon = new GameObject[4];

	public GameObject[] Player{
		get{ return player; }
		set{ player = value; }
	}

	public GameObject[] Icon {
		get{ return icon; }
		set{ icon = value; }
	}

	public void generater (Vector3 pos) {
		charaName = GetComponent<FileLoad2> ().readnames ();
		playerGenerate (pos);
		iconGenerate (pos);
	}

	public void playerGenerate(Vector3 pos){
		GameObject playerObject = new GameObject ("Empty");
		playerObject.name = "PlayerObject";
		for (int i = 0; i < 4; i++) {
			player [i] = Instantiate (playerPrefab, pos + Vector3.up * 0.65f, Quaternion.Euler (0, 180.0f, 0));
			player [i].transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
			player [i].name = charaName [i];
			player [i].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("design/001/" + charaName [i]);
			player [i].transform.SetParent (playerObject.transform, false);
		}
	}

	public void iconGenerate(Vector3 pos){
		GameObject IconObject = new GameObject ("Empty");
		IconObject.name = "IconObject";
		for (int i = 0; i < 4; i++) {
			icon [i] = Instantiate (iconPrefab, pos + Vector3.up * 0.1f, Quaternion.Euler (90.0f, 0, 0));
			icon [i].transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			icon [i].name = charaName [i] + "Icon";
			icon [i].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("design/UI/MapIcon/" + charaName [i] + "MapIcon");
			icon [i].transform.SetParent (IconObject.transform, false);
		}
	}
}
