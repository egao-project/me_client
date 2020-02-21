using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{

    [SerializeField] private GameObject APILoadingObject;
    [SerializeField] private GameObject SceneLoadingObject;
    [SerializeField] private GameObject MessageLoadingObject;
    [SerializeField] private Text MessageText;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    /// <summary>
    /// APIを走らせているときのローディング画面を表示、非表示する
    /// </summary>
    /// <param name="view">If set to <c>true</c> view.</param>
    public void APILoading(bool view = false)
    {
        if (view)
        {
            APILoadingObject.SetActive(true);
        }
        else
        {
            APILoadingObject.SetActive(false);
        }
    }


    /// <summary>
    /// シーン間の移動中に最前衛でアニメーションする
    /// </summary>
    /// <param name="view">If set to <c>true</c> view.</param>
    public void SceneLoading(bool view = false)
    {
        if (view)
        {
            SceneLoadingObject.SetActive(true);
        }
        else
        {
            SceneLoadingObject.SetActive(false);
        }
    }

    /// <summary>
    /// メッセージダイアログ表示
    /// </summary>
    /// <param name="msg">ダイアログに表示するメッセージを代入</param>
    /// <param name="view">If set to <c>true</c> view.</param>
    public void MessageView(string msg = "")
    {
        MessageLoadingObject.SetActive(true);
        MessageText.text = msg + "\n\nTap";
    }

    // <summary>
    // Messages the close.
    // </summary>
    // <param name="msg">Message.</param>
    public void MessageClose()
    {
        MessageLoadingObject.SetActive(false);
        MessageText.text = "";
    }
}
