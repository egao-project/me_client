using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kakera;
using UnityEngine.SceneManagement;
using System.Threading;

public class DetailController : BaseController {

	[SerializeField] private GameObject nextCanvas;

	[SerializeField]
	private Unimgpicker imagePicker;

	[SerializeField]
	private Image[] imageRenderer = new Image[5];

	[SerializeField]
	private Text title;

	[SerializeField]
	private Image[] imageSampleRenderer = new Image[5];

	private int index;

	private string[] urls = new string[5];
	private int[] positions = new int[5];
	private int[] ids = new int[5];
	private Picture[] master = new Picture[5];
	private Frame frame = ListController.list [ListController.frame_idx];

	// Use this for initialization
	IEnumerator Start () {
		base.Start ();
		nextCanvas.SetActive (true);

		if (frame.path_list != null && frame.path_list != "") {
			urls = frame.path_list.Split (',');
			InitArrString (urls, frame.path_list);
			InitArrInt (positions, frame.position_list);
			InitArrInt (ids, frame.id_list);
            Debug.Log("test");

			for (int i = 0; i < urls.Length; i++) {
				Picture p = new Picture ();
				p.position = positions [i];
				p.id = ids [i];
				p.image = urls [i];
				master [p.position] = p;
			}

			int idx = 0;
			WWW www = null;
			foreach (string u in urls) {
				string tmp = u.Trim ();
				www = new WWW (tmp);
				// 画像ダウンロード完了を待機
				yield return www;
				var texture = www.texture;
				float x = imageRenderer [positions [idx]].transform.GetComponent<RectTransform> ().sizeDelta.x;
				float y = imageRenderer [positions [idx]].transform.GetComponent<RectTransform> ().sizeDelta.y;

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
				imageRenderer [positions [idx]].sprite = 
					Sprite.Create (tmpTexture, new Rect (0, 0, bX, bY), Vector2.zero);
				imageSampleRenderer [positions [idx]].sprite = 
					Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero);
				
				idx++;
				www = null;
			}
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

	// Update is called once per frame
	void Update () {

	}

	void Awake()
	{
		imagePicker.Completed += (string path) =>
		{
			StartCoroutine(LoadImage(path, imageRenderer[index]));
		};
	}

	public void OnPressShowPicker(int i)
	{
		index = i;
		imagePicker.Show("Select Image", "unimgpicker", 1024);
	}

	private IEnumerator LoadImage(string path, Image output)
	{
		nextCanvas.SetActive (true);

		var url = "file://" + path;
		var www = new WWW(url);
		yield return www;

		var texture = www.texture;
		if (texture == null)
		{
			Debug.LogError("Failed to load texture url:" + url);
		}

		//output.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		float x = imageRenderer [positions [index]].transform.GetComponent<RectTransform> ().sizeDelta.x;
		float y = imageRenderer [positions [index]].transform.GetComponent<RectTransform> ().sizeDelta.y;

		float bX, bY;

		if (x > y) {
			float gen = texture.width / x;
			bX = x;
			bY = y * gen;
		} else {
			float gen = texture.height / y;
			bX = x * gen;
			bY = y;
		}

		float bix = 0.1f;
		while (true) {
			if (texture.width * bix < 900) {
				bix = bix + 0.1f;
			} else {
				break;
			}
		}

		bX = texture.width * bix;
		bY = texture.height * bix;
		Debug.LogError(bX);
		Debug.LogError(bY);


		TextureScale.Bilinear(texture,(int)bX, (int)bY);
		var tmpTexture = getCenterClippedTexture (texture, (int)x, (int)y);

		output.sprite = 
			Sprite.Create (tmpTexture, new Rect (0, 0, x, y), Vector2.zero);
		imageSampleRenderer [index].sprite = 
			Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero);
		//output.SetNativeSize ();
		//output.material.mainTexture = texture;

		Picture p = master [index];
		HttpConector http = new HttpConector ();
		if (p != null) {
			http.Delete (Const.PICTURE_DELETE_URL, p.id.ToString ());
		} else {
			p = new Picture();
		}

		HttpItem r = http.PostImage (texture.EncodeToJPG(), frame.id, index);

		Picture pp = JsonUtility.FromJson<Picture> (r.body);

		p.id = pp.id;
		master [index] = p;

		nextCanvas.SetActive (false);

	}

	public void PushCommitButton()
	{
		frame.title = title.text;

		HttpConector http = new HttpConector ();
		HttpItem r = http.Post (Const.FRAME_ADD_TITLE,JsonUtility.ToJson (frame));
		Debug.Log (r.code);
		Debug.Log (r.body);

		if (r.code == 200||r.code == 201 ) {
			//サインイン画面に移動
			Invoke ("ViewLogin", 1.0f);

		} else {
			nextCanvas.SetActive (false);
			Debug.Log ("タイトル変更に失敗しました。");
		}

		SceneManager.LoadScene ("ListScene"); 
	}

	private void InitArrString(string[] arr,string list)
	{
		int idx = 0;
		foreach (string item in list.Split (',')) {
			arr[idx] = item;
			idx++;
		}
	}
	private void InitArrInt(int[] arr,string list)
	{
		int idx = 0;
		foreach (string item in list.Split (',')) {
			arr[idx] = int.Parse(item);
			idx++;
		}
	}
}
