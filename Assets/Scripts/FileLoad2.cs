using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class FileLoad2 : MonoBehaviour {

	private List<string[]> iventStr = new List<string[]> ();
	private List<int> iventOrder = new List<int> ();

	public List<int> IventOrder{
		get{ return iventOrder; }
		set{ iventOrder = value; }
	}
	public List<string[]> IventStr{
		get{ return iventStr; }
		set{ iventStr = value; }
	}

	private StreamReader sr;
	private StreamWriter wr;

	private string[] names = new string[4];

	public void readData(){
		//マスの並び順を取得
		//dataFile = Instantiate (Resources.Load ("data/iventOrder"))as TextAsset;
		//rd = new StringReader (dataFile.text);
		sr = new StreamReader (Application.dataPath + "/Resources/data/iventOrder.csv");
		string[] str = sr.ReadLine ().Split (',');
		for (int i = 0; str [i] != "goal"; i++) {
			iventOrder.Add (int.Parse (str [i]));
		}
		//END

		//イベントの文章を取得
		//dataFile = Instantiate (Resources.Load ("data/iventStr"))as TextAsset;
		//rd = new StringReader (dataFile.text);
		sr = new StreamReader (Application.dataPath + "/Resources/data/iventStr2.csv");
		while (sr.Peek () > -1) {
			str = sr.ReadLine ().Split (',');
			iventStr.Add (str);
			//Debug.Log ("str1:" + str [0] + "str2:" + str [1] + "str2:" + str [2]);
		}
		//END
		
		sr.Close ();
	}

	public string[] readnames(){
		sr = new StreamReader (Application.dataPath + "/Resources/data/data.csv");
		string[] str = sr.ReadLine ().Split (',');
		for (int i = 0; i < 4; i++) {
			names [i] = str [i];
		}
		sr.Close ();

		return names;
	}
}
