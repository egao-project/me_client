using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ListController : BaseController {

	// Use this for initialization
	public void Start () {
		base.Start ();

		HttpConector http = new HttpConector ();
		HttpItem r = http.Get (Const.FRAME_URL,"username="+BaseController.user.username);
		string[] separator = new string[] {"},"};
		string[] jsons = r.body.Split(separator,System.StringSplitOptions.RemoveEmptyEntries);
		List<Frame> list = new List<Frame> ();
		foreach(string f in jsons){
			string item = f;
			item = item.Replace ("[", "");
			item = item.Replace ("]", "");
			list.Add(JsonUtility.FromJson<Frame> (item));
		}
		string a = "";

	}


	// Update is called once per frame
	void Update () {
		
	}
}
