using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDef : MonoBehaviour {
	public struct player
	{
		private GameObject plob;
		private GameObject icon;
		private int plpo;

		public GameObject Plob{
			get{ return plob; }
			set{ plob = value; }
		}
		public GameObject Icon{
			get{ return icon; }
			set{ icon = value; }
		}
		public int Plpo{
			get{ return plpo; }
			set{ plpo = value; }
		}
	}

	public struct ivent
	{
		private GameObject ivob;
		private string[] ivstr;

		public GameObject Ivob{
			get{ return ivob; }
			set{ ivob = value; }
		}
		public string[] Ivstr{
			get{ return ivstr; }
			set{ ivstr = value; }
		}
	}

	public struct ivent2
	{
		private GameObject ivob;
		private string[] ivstr;

		public GameObject Ivob{
			get{ return ivob; }
			set{ ivob = value; }
		}
		public string[] Ivstr{
			get{ return ivstr; }
			set{ ivstr = value; }
		}
	}
}
