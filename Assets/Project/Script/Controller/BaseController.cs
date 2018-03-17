using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseController : MonoBehaviour {

	public static User user;

	// Use this for initialization
	public void Start () {
		if (user == null || user.token == null) {
			SceneManager.LoadScene ("InitScene");
		}
	}

	public static void SetUser(User u){
		user = u;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
