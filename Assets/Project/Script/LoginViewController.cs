using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoginViewController : MonoBehaviour {

	[SerializeField]
	Button LoginButton;


	// Use this for initialization
	void Start () {
		//LoginButton.onClick.AddListener (Login);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//ログイン処理
	//TODO:バリデートの方法の確認
	void Login(){
		//SceneManager.LoadScene("ListScene"); 
	}
}
