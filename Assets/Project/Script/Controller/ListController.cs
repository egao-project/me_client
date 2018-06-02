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

	[SerializeField] private GameObject nextCanvas;

	[SerializeField]
	private Image[] imageRenderer = new Image[3];

	[SerializeField] 
	private GameObject[] NonDataCanvas = new GameObject[3];

	[SerializeField] 
	private GameObject[] FilemesCanvas = new GameObject[3];

	[SerializeField] 
	private Text[] TitleTexts = new Text[3];

	public const int MAX = 3;
	public static Frame[] list = new Frame[MAX];
	public static int frame_idx = 0;

	// Use this for initialization
	public IEnumerator Start () {
		base.Start ();
		nextCanvas.SetActive (true);

		HttpConector http = new HttpConector ();
		HttpItem r = http.Get (Const.FRAME_URL,"username="+BaseController.user.username);
		JsonToFramList(r.body);

		int idx = 0;
		foreach (Frame model in list) {


			if (model.path_list == null || model.path_list == "") {
				continue;
			}

			//リスト切り替え
			NonDataCanvas [idx].SetActive (false);
			FilemesCanvas [idx].SetActive (true);

			//画像設定
			string[] urls = model.path_list.Split (',');
			//attacheWebImageToGameobject_appropriately (urls [0], imageRenderer [idx]);
			WWW www = new WWW(urls[0]);
			// 画像ダウンロード完了を待機
			yield return www;
			var texture = www.texture;
			imageRenderer[idx].sprite = 
				Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero);

			//タイトル設定
			TitleTexts[idx].text = model.title;

			idx++;
		}
		nextCanvas.SetActive (false);

	}

	private void JsonToFramList (string json) 
	{
		string[] separator = new string[] {"},"};
		string item = json;
	
		item = Regex.Replace(item,"^.*\\[", "");
		item = item.Replace ("]}", "");

		string[] jsons = item.Split(separator,System.StringSplitOptions.RemoveEmptyEntries);
		int idx = 0;
		foreach(string f in jsons){
			string tmp = f;
			if(!tmp.EndsWith("}")){
				tmp = tmp + "}";
			}
			list [idx] = JsonUtility.FromJson<Frame> (tmp);
			idx++;
		}
		for (int i = idx; i < MAX; i++) {
			list [i] = new Frame ();
		}

	}

	public void OnPressCopyBorad(int i)
	{
		Frame selected = list [i];
		string url = Const.VIEW_URL;
		if(selected != null) {
			url = url + "?frame_id=" + selected.id.ToString ();
		}
		UniClipboard.SetText(url);
	}

	public void OnPressFilesButton(int i)
	{
		frame_idx = i;

		Frame selected = list [i];
		if (selected.id == null) {
			HttpConector http = new HttpConector ();
			HttpItem r = http.PostFrom (Const.FRAME_ADD_URL, BaseController.user.username, i);
			if (r.code == 200) {
				list[i] = JsonUtility.FromJson<Frame> (r.body);
			}
		}
		SceneManager.LoadScene ("DetailScene"); 
	}

	/// <summary>
	/// //指定したウェブ画像を読み込んでゲームオブジェクトのテクスチャとして表示(適切に表示サイズを調整)
	/// 読み込み画像が最大で表示されるように表示部分が自動調整されます
	/// </summary>
	/// <param name="url"></param>
	/// <param name="gObj"></param>
	/// <returns></returns>
	public static IEnumerator<WWW> attacheWebImageToGameobject_appropriately(string url, Image gObj)
	{
		WWW texturewww = new WWW(url);
		yield return texturewww;
		gObj.GetComponent<Renderer>().material.mainTexture = texturewww.texture;

		float Obj_x = gObj.transform.lossyScale.x;
		float Obj_y = gObj.transform.lossyScale.y;
		float Img_x = (float)texturewww.texture.width;
		float Img_y = (float)texturewww.texture.height;

		float aspectRatio_Obj = Obj_x / Obj_y;
		float aspectRatio_Img = Img_x/Img_y;

		if (aspectRatio_Img> aspectRatio_Obj)
		{
			//イメージサイズのほうが横に長い場合
			gObj.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(aspectRatio_Obj / aspectRatio_Img, 1f));
			gObj.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(   (Img_x-(Obj_x*Img_y/Obj_y))/(2*Img_x)         , 1f));
		}
		else
		{
			//イメージサイズのほうが縦に長い場合
			gObj.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1f,  aspectRatio_Img/ aspectRatio_Obj));
			gObj.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(1f,  (Img_y-Obj_y*Img_x/Obj_x)/(2*Img_y)          ));
		}
	}


	// Update is called once per frame
	void Update () {
		
	}
}
