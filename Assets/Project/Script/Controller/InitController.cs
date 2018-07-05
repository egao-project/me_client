using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;

public class InitController : BaseController {

	//canvas管理
	[SerializeField] private GameObject loadingCanvas;
	[SerializeField] private GameObject singinCanvas;
	[SerializeField] private GameObject loginCanvas;
	[SerializeField] private GameObject nextCanvas;

	//文字入力管理
	[SerializeField] private InputField useridInput;
	[SerializeField] private InputField passInput;

	//ボタン管理
	[SerializeField] private GameObject loginButton;
	[SerializeField] private GameObject messageButton;

	//テキスト管理
	[SerializeField] private Text messageText;

	// Use this for initialization
	public void Start () {
		ViewLoading ();

		//初期起動状態かローカルに保存しているTokenStringを確認
		if (PlayerPrefs.GetString ("TokenKey") == null) {
			//signup処理:TODO
		} else {
			//トークンキーが期限切れ及び一致しない場合の処理

		}

		Invoke("ViewLogin", 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
	}

	/// <summary>
	/// ログイン処理
	/// </summary>
	public void PushLoginButton(){
		nextCanvas.SetActive (true);

		User model = new User ();
		model.username = useridInput.text;
		model.password = passInput.text;

		HttpConector http = new HttpConector ();
		HttpItem r = http.Post (Const.LOGIN_URL,JsonUtility.ToJson (model));
		Debug.Log (r.code);
		Debug.Log (r.body);

		if (r.code == 200) {
			User u = JsonUtility.FromJson<User> (r.body);
			//ユーザ情報を保持
			u.username = useridInput.text;
			u.password = passInput.text;
			BaseController.SetUser(u);

			//Tokenキーを保存
			PlayerPrefs.SetString("TokenKey", u.token);


			Debug.Log (PlayerPrefs.GetString ("TokenKey"));

			SceneManager.LoadScene ("ListScene"); 
		} else {
			nextCanvas.SetActive (false);
			ViewMessage("ログインに失敗しました。。");
		}
	}
		
	/// <summary>
	/// メッセージ消す 
	/// </summary>
	public void PushMessageButton ()
	{
		messageButton.SetActive (false);
	}


/**private**/
	private void ViewMessage (string msg)
	{
		messageButton.SetActive (true);
		messageText.text = msg;
	}
		
	private void ViewLoading () {
		loadingCanvas.SetActive (true);
		singinCanvas.SetActive (false);
		loginCanvas.SetActive (false);
	}
	private void ViewSingin () {
		loadingCanvas.SetActive (false);
		singinCanvas.SetActive (true);
		loginCanvas.SetActive (false);
	}
	private void ViewLogin () {
		loadingCanvas.SetActive (false);
		singinCanvas.SetActive (false);
		loginCanvas.SetActive (true);
	}
	private void signup () {
		loadingCanvas.SetActive (false);
		singinCanvas.SetActive (false);
		loginCanvas.SetActive (false);
	}

}
