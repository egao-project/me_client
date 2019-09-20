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
	private Image[] imageSampleRenderer = new Image[3];

	[SerializeField] 
	private GameObject[] NonDataCanvas = new GameObject[3];

	[SerializeField] 
	private GameObject[] FilemesCanvas = new GameObject[3];

	[SerializeField] 
	private Text[] TitleTexts = new Text[3];

    //テキスト管理
    private DialogController dialogView;

    //アルバムの最大数を定数化
    public const int MAX = 3;
	public static Frame[] list = new Frame[MAX];
	public static int frame_idx = 0;

	// Use this for initialization
    //TODO:変更が必要
	public IEnumerator Start () {
		base.Start ();
		nextCanvas.SetActive (true);

        //シングルトンで表示されているDialogViewerを取得
        //TODO:Findで見つからなかった場合のInstantiate処理を行う
        dialogView = GameObject.Find("DialogViewer").GetComponent<DialogController>();

        HttpItem r = APIController.Test2(null,
            () => {
                nextCanvas.SetActive(false);
                Debug.Log("エラーが発生しました。");
            });

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
			float x = imageRenderer [idx].transform.GetComponent<RectTransform> ().sizeDelta.x;
			float y = imageRenderer [idx].transform.GetComponent<RectTransform> ().sizeDelta.y;

			float bX, bY;

			bX = x;
			if (texture.width < x) {
				bX = texture.width;
			}
			bY = y;
			if (texture.height < y) {
				bY = texture.height;
			}

			var tmpTexture = getCenterClippedTexture (texture, (int)bX, (int)bY);
			imageRenderer[idx].sprite = 
				Sprite.Create (tmpTexture, new Rect (0, 0, bX, bY), Vector2.zero);
			imageSampleRenderer[idx].sprite = 
				Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero);

			//タイトル設定
			TitleTexts[idx].text = model.title;

			idx++;
		}
		nextCanvas.SetActive (false);

	}
	Texture2D getCenterClippedTexture(Texture2D texture,int x,int y)
	{
		Color[] pixel;
		Texture2D clipTex;
		int tw = texture.width;
		int th = texture.height;
		// GetPixels (x, y, width, height) で切り出せる
		pixel = texture.GetPixels(0, 0, x, y);
		// 横幅，縦幅を指定してTexture2Dを生成
		clipTex = new Texture2D(x, y); 
		clipTex.SetPixels(pixel);
		clipTex.Apply();
		return clipTex;
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
			if (list.Length > idx) {
				idx++;
			}
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
		//UniClipboard.SetText(url);
        SocialConnector.SocialConnector.Share("This is Me! ", url);

    }

	public void OnPressFilesButton(int i)
	{
		frame_idx = i;

		Frame selected = list [i];
		if (selected.id == null) {
            APIController.Test3(list, i, null,
            () => {
                nextCanvas.SetActive(false);
                Debug.Log("エラーが発生しました。");
            });
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
