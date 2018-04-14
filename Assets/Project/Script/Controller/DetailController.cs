﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kakera;

public class DetailController : MonoBehaviour {

	[SerializeField]
	private Unimgpicker imagePicker;

	[SerializeField]
	private Image[] imageRenderer = new Image[5];

	private int index;

	// Use this for initialization
	IEnumerator Start () {
		Frame model = ListController.list [ListController.frame_idx];
		int idx = 0;
		foreach (string u in model.path_list.Split(',')) {
			WWW www = new WWW(u);
			// 画像ダウンロード完了を待機
			yield return www;
			var texture = www.texture;
			imageRenderer[idx].sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
			idx++;
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
		output.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		//output.material.mainTexture = texture;
	}

}
