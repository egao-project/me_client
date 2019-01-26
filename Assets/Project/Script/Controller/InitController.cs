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
	[SerializeField] private GameObject signupCanvas;
	[SerializeField] private GameObject mailSignupCanvas;

	//文字入力管理
	[SerializeField] private InputField useridInput;
	[SerializeField] private InputField passInput;

	//文字入力管理(新規登録)
	[SerializeField] private InputField registUseridInput;
	[SerializeField] private InputField registPassInput;
	[SerializeField] private InputField registEmailInput;

	//ボタン管理
	[SerializeField] private GameObject loginButton;
	[SerializeField] private GameObject messageButton;
	[SerializeField] private GameObject messageButtonSignup;

	//テキスト管理
	[SerializeField] private Text messageText;

	private string tokenKey;

	// Use this for initialization
	public void Start () {
		ViewLoading ();
		tokenKey = PlayerPrefs.GetString ("TokenKey");

		//初期起動状態かローカルに保存しているTokenStringを確認
		if (tokenKey == null || tokenKey == "") 
		{
			Invoke ("SignUp", 1.0f);
		}
		else if (AuthToken () == true) 
		{
			//トークンキーが期限切れ及び一致しない場合の処理
			SceneManager.LoadScene ("ListScene");
		}
		else 
		{
			Invoke ("ViewLogin", 1.0f);
		}
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
		Debug.Log ("ConstURL is:" + Const.LOGIN_URL);

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
	/// 新規登録処理
	/// </summary>
	public void SignUpButton(){
		nextCanvas.SetActive (true);

		RegistUser model = new RegistUser ();
		model.username = registUseridInput.text;
		model.password = registPassInput.text;
		model.email = registEmailInput.text;

		HttpConector http = new HttpConector ();
		HttpItem r = http.Post (Const.REGISTER,JsonUtility.ToJson (model));
		Debug.Log (r.code);
		Debug.Log (r.body);

		if (r.code == 201) {
			
			//次回アプリ起動時ログイン画面に遷移するための仮トークンキー
			PlayerPrefs.SetString("TokenKey", "NoFirstLogin");

			//サインイン画面に移動
			Invoke ("ViewLogin", 1.0f);

		} else {
			nextCanvas.SetActive (false);
			ViewMessageSignUp("新規登録に失敗しました。");
		}
	}
		
	/// <summary>
	/// メッセージ消す 
	/// </summary>
	public void PushMessageButton ()
	{
		messageButton.SetActive (false);
	}

	/// <summary>
	/// メッセージ消す 
	/// </summary>
	public void PushMessageButtonSignUp ()
	{
		messageButtonSignup.SetActive (false);
	}

	/// <summary>
	/// サインアップ画面で戻るボタン処理
	/// </summary>
	/// <param name="msg">Message.</param>
	public void RegisterBackButton ()
	{
		ViewSingin ();
	}


/**private**/
	private bool AuthToken()
	{
		if (tokenKey == "NoFirstLogin")
		{
			return false;
			//tokenキーの整合性の確認
		} 
		else 
		{


		}
		return false;
	}


	private void ViewMessage (string msg)
	{
		messageButton.SetActive (true);
		messageText.text = msg;
	}

	private void ViewMessageSignUp (string msg)
	{
		messageButtonSignup.SetActive (true);
		messageText.text = msg;
	}
		
	private void ViewLoading () {
		loadingCanvas.SetActive (true);
		singinCanvas.SetActive (false);
		loginCanvas.SetActive (false);
		signupCanvas.SetActive (false);
	}
	private void ViewSingin () {
		loadingCanvas.SetActive (false);
		singinCanvas.SetActive (false);
		loginCanvas.SetActive (true);
		signupCanvas.SetActive (false);
	}
	private void ViewLogin () {
		loadingCanvas.SetActive (false);
		singinCanvas.SetActive (false);
		loginCanvas.SetActive (true);
		signupCanvas.SetActive (false);
		mailSignupCanvas.SetActive (false);
	}
	private void SignUp () {
		loadingCanvas.SetActive (false);
		singinCanvas.SetActive (false);
		loginCanvas.SetActive (false);
		//signupCanvas.SetActive (true);
		//現状メールサインアップのみの実装
		signupCanvas.SetActive (false);
		mailSignupCanvas.SetActive (true);
	}

}
