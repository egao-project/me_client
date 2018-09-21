using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class HyperTextViewer : MonoBehaviour
{
	[SerializeField]
	RegexHypertext _text;

	const string RegexURL = "利用規約";
	const string RegexHashtag = "プライバシーポリシー";

	void Start()
	{
		_text.SetClickableByRegex(RegexURL, Color.blue, url => Debug.Log(url));
		_text.SetClickableByRegex(RegexHashtag, Color.blue, hashtag => Debug.Log(hashtag));
	}
}