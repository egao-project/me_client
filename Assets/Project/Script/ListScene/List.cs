using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class List : MonoBehaviour {

	[SerializeField] Image _initialImage;
	[SerializeField] Text _titleText;
	[SerializeField] Text _timeText;
	[SerializeField] Text _fileDitailText;

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
}
