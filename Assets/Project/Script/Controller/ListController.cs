using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.IO;

public class ListController : BaseController {

	[SerializeField]
	private Image[] imageRenderer = new Image[3];

	[SerializeField] 
	private GameObject[] NonDataCanvas = new GameObject[3];

	[SerializeField] 
	private GameObject[] FilemesCanvas = new GameObject[3];

	[SerializeField] 
	private Text[] TitleTexts = new Text[3];

	public static List<Frame> list = new List<Frame>();
	public static int frame_idx = 0;

	// Use this for initialization
	public IEnumerator Start () {
		base.Start ();

		HttpConector http = new HttpConector ();
		HttpItem r = http.Get (Const.FRAME_URL,"username="+BaseController.user.username);
		list = JsonToModel<Frame>(r.body);

		int idx = 0;
		foreach (Frame model in list) {

			//リスト切り替え
			NonDataCanvas [idx].SetActive (false);
			FilemesCanvas [idx].SetActive (true);

			//画像設定
			string[] urls = model.path_list.Split (',');
			WWW www = new WWW(urls[0]);
			// 画像ダウンロード完了を待機
			yield return www;
			var texture = www.texture;
			imageRenderer[idx].sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

			//タイトル設定
			TitleTexts[idx].text = model.title;
		}

	}

	private List<Type> JsonToModel<Type> (string json) 
	{
		string[] separator = new string[] {"},"};
		string item = json;
	
		item = Regex.Replace(item,"^.*\\[", "");
		item = item.Replace ("]}", "");

		string[] jsons = item.Split(separator,System.StringSplitOptions.RemoveEmptyEntries);
		List<Type> list = new List<Type> ();
		foreach(string f in jsons){
			string tmp = f;
			if(!tmp.EndsWith("}")){
				tmp = tmp + "}";
			}
			list.Add(JsonUtility.FromJson<Type> (tmp));
		}
		return list;
	}

	public void OnPressFilesButton(int i)
	{
		frame_idx = i;
		SceneManager.LoadScene ("DetailScene"); 
	}


	// Update is called once per frame
	void Update () {
		
	}
}
