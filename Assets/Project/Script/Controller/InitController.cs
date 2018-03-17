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

		Invoke("ViewLogin", 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
	}

	/// <summary>
	/// ログイン処理
	/// </summary>
	public void PushLoginButton(){
		User model = new User ();
		model.username = useridInput.text;
		model.password = passInput.text;

		HttpConector http = new HttpConector ();
		HttpItem r = http.Post (Const.LOGIN_URL,JsonUtility.ToJson (model));
		Debug.Log (r.code);
		Debug.Log (r.body);

		if (r.code == 200) {
			User u = JsonUtility.FromJson<User> (r.body);
			u.username = useridInput.text;
			u.password = passInput.text;
			BaseController.SetUser(u);
			SceneManager.LoadScene ("ListScene"); 
		} else {
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

}
