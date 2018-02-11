using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperTextViewer2 : MonoBehaviour {
	[SerializeField]
	RegexHypertext _text;

	const string RegexURL = "パスワードを忘れた方はこちら";

	void Start()
	{
		_text.SetClickableByRegex(RegexURL, Color.white, url => Debug.Log(url));
	}
}
