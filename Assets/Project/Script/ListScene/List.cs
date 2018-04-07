using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class List : MonoBehaviour {

	[SerializeField] Image _initialImage;
	[SerializeField] Text _titleText;
	[SerializeField] RectTransform _listScale;
	[SerializeField] Button _shareButton;
	[SerializeField] Button _addImageButton;

	private ListSceneManager _listSceneManager;

	public Button shareButton{
		get{ return _shareButton;}
	}

	const float HEIGHT_SIZE = 580; 

	void Awake(){
		SizeController ();
	}

	public Image InitialImage {
		get{
			 return _initialImage;
		}
		set{
			_initialImage = value;
		}
	}

	public Text TitleText{
		get{
			return _titleText;
		}
		set{
			_titleText = value;
		}
	}

	//リスト表示時の幅を設定
	//ToDo:幅の処理をInstantiateする側に移動
	void SizeController(){
		//float width  = (float)Screen.width;
		//_listScale.sizeDelta = new Vector2 (width, 580f);
	}
}
