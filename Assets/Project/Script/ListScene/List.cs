using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class List : MonoBehaviour {

	[SerializeField] Image _initialImage;
	[SerializeField] Text _titleText;
	[SerializeField] Text _timeText;
	[SerializeField] Text _fileDitailText;
	[SerializeField] RectTransform _listScale;
	[SerializeField] Button _shareButton;
	private ListSceneManager _listSceneManager;

	public Button shareButton{
		get{ return _shareButton;}
	}

	const float HEIGHT_SIZE = 580; 

	void Awake(){
		SizeController ();
		_listSceneManager = GameObject.Find("Hand");
	}

	Image InitialImage {
		get{
			 return _initialImage;
		}
		set{
			_initialImage = value;
		}
	}

	Text TimeText{
		get{
			return _titleText;
		}
		set{
			_titleText = value;
		}
	}
	Text TitleText{
		get{
			return _timeText;
		}
		set{
			_timeText = value;
		}
	}
	Text FileDitailText
	{
		get{
			return _fileDitailText;
		}
		set{
			_fileDitailText = value;
		}
	}

	//リスト表示時の幅を設定
	void SizeController(){
		float width  = (float)Screen.width;
		_listScale.sizeDelta = new Vector2 (width, 580f);
	}
}
