using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kakera;
using UnityEngine.SceneManagement;


public class DetailController : BaseController {

	[SerializeField]
	private Unimgpicker imagePicker;

	[SerializeField]
	private Image[] imageRenderer = new Image[5];

	private int index;

	private string[] urls = new string[5];
	private int[] positions = new int[5];
	private int[] ids = new int[5];
	private Picture[] master = new Picture[5];
	private Frame frame = ListController.list [ListController.frame_idx];

	// Use this for initialization
	IEnumerator Start () {
		base.Start ();

		if (frame.path_list != null && frame.path_list != "") {
			urls = frame.path_list.Split (',');
			InitArrString (urls, frame.path_list);
			InitArrInt (positions, frame.position_list);
			InitArrInt (ids, frame.id_list);

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
				imageRenderer [positions [idx]].sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero);
				idx++;
				www = null;
			}
		}
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
		var url = "file://" + path;
		var www = new WWW(url);
		yield return www;

		var texture = www.texture;
		if (texture == null)
		{
			Debug.LogError("Failed to load texture url:" + url);
		}
		Debug.LogError ("1");
		output.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		//output.material.mainTexture = texture;
		Debug.LogError ("2");

		Picture p = master [index];
		Debug.LogError ("3");
		HttpConector http = new HttpConector ();
		Debug.LogError ("4");
		if (p != null) {
			Debug.LogError ("5");
			http.Delete (Const.PICTURE_DELETE_URL, p.id.ToString ());
		}
		Debug.LogError ("6");

		HttpItem r = http.PostImage (texture.EncodeToJPG(), frame.id, index);
		Debug.LogError ("7");

		Picture pp = JsonUtility.FromJson<Picture> (r.body);
		Debug.LogError ("8");

		p.id = pp.id;
		master [index] = p;

	}

	public void PushCommitButton()
	{
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
