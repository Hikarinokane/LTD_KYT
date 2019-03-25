using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class CharaSelect : MonoBehaviour {

	private TextAsset dataFile;
	private StringReader rd;
	private StreamWriter sw;
	private FileInfo fi;

	private string[] names = new string[4];
	private int count = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick(GameObject chara){
		if (count < 4) {
			if (checkName (chara.name)) {
				names [count] = chara.name;
				count++;
			}
		}
		if (count == 4)
			writeData ();

	}

	public bool checkName(string name){
		for (int i = 0; i < count; i++) {
			if (names [i] == name) {
				Debug.LogWarning ("ERROR:Don't Select same charactor");
				return false;
			}
		}
		return true;
	}

	public void writeData(){
		Debug.Log ("write data");
		fi = new FileInfo (Application.dataPath + "/Resources/data/data.csv");
		sw = fi.CreateText ();
		//Debug.Log (names [0] + "," + names [1] + "," + names [2] + "," + names [3]);
		sw.WriteLine (names [0] + "," + names [1] + "," + names [2] + "," + names [3]);
		sw.Flush ();
		sw.Close ();

		SceneManager.LoadScene ("Main");
	}
}
