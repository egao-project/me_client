using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;

public class InitController : BaseController
{

    //canvas管理
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private GameObject singinCanvas;
    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject nextCanvas;
    [SerializeField] private GameObject signupCanvas;
    [SerializeField] private GameObject mailSignupCanvas;

    //文字入力管理
    [SerializeField] private InputField useridInput;
    [SerializeField] private InputField passInput;

    //文字入力管理(新規登録)
    [SerializeField] private InputField registUseridInput;
    [SerializeField] private InputField registPassInput;
    [SerializeField] private InputField registEmailInput;

    //ボタン管理
    [SerializeField] private GameObject loginButton;

    //ダイアログ表示クラス
    private DialogController dialogView;

    private string tokenKey;

    // Use this for initialization
    public void Start()
    {
        dialogView = GameObject.Find("DialogViewer").GetComponent<DialogController>();
        ViewLoading();
        tokenKey = PlayerPrefs.GetString("TokenKey");

        //初期起動状態かローカルに保存しているTokenStringを確認
        if (tokenKey == null || tokenKey == "")
        {
            Invoke("SignUp", 1.0f);
        }
        else if (AuthToken() == true)
        {
            //トークンキーが期限切れ及び一致しない場合の処理
            SceneManager.LoadScene("ListScene");
        }
    }

    /// <summary>
    /// ログイン処理
    /// </summary>
    public void PushLoginButton()
    {
        nextCanvas.SetActive(true);

        User model = new User();
        model.username = useridInput.text;
        model.password = passInput.text;

        APIController.Login(model,
            () => { SceneManager.LoadScene("ListScene"); }, //成功時一覧画面に遷移
            () =>
            {
                nextCanvas.SetActive(false);            //失敗時ポップアップを表示
                ViewMessage("ログインに失敗しました。。");
            });
    }

    /// <summary>
    /// 新規登録処理
    /// </summary>
    public void SignUpButton()
    {
        nextCanvas.SetActive(true);

        //入力されたデータをjsonに整形
        RegistUser model = new RegistUser();
        model.username = registUseridInput.text;
        model.password = registPassInput.text;
        model.email = registEmailInput.text;

        //API処理
        APIController.APIPost(Const.REGISTER, JsonUtility.ToJson(model),
            value =>
            {
                //次回アプリ起動時ログイン画面に遷移するための仮トークンキー
                PlayerPrefs.SetString("TokenKey", "NoFirstLogin");
                //サインイン画面に移動
                Invoke("ViewLogin", 1.0f);
            },
            () =>
            {
                nextCanvas.SetActive(false);
                ViewMessageSignUp("新規登録に失敗しました。");
            });
    }

    /// <summary>
    /// サインアップ画面で戻るボタン処理
    /// </summary>
    /// <param name="msg">Message.</param>
    public void RegisterBackButton()
    {
        ViewSingin();
    }

    /// <summary>
    /// 自動ログイン処理
    /// </summary>
    public void AutoLogin()
    {
        //端末内に保持しているログイン情報を取得
        User model = new User();
        //model.token = tokenKey;
        model.username = PlayerPrefs.GetString("UserID");
        model.password = PlayerPrefs.GetString("UserPass");

        nextCanvas.SetActive(true);

        APIController.Login(model,
            () => { SceneManager.LoadScene("ListScene"); },　//成功時一覧画面に遷移
            () =>
            {
                nextCanvas.SetActive(false);            //失敗時ログイン画面に遷移
                ViewLogin();
            }
            );
    }


    /**private**/
    private bool AuthToken()
    {
        if (tokenKey == "NoFirstLogin")
        {
            return false;
            //tokenキーの整合性の確認
        }
        else
        {

        }
        return false;
    }




    private void ViewMessage(string msg)
    {
        dialogView.MessageView(msg);
    }

    private void ViewMessageSignUp(string msg)
    {
        dialogView.MessageView(msg);
    }

    private void ViewLoading()
    {
        loadingCanvas.SetActive(true);
        singinCanvas.SetActive(false);
        loginCanvas.SetActive(false);
        signupCanvas.SetActive(false);
    }
    private void ViewSingin()
    {
        loadingCanvas.SetActive(false);
        singinCanvas.SetActive(false);
        loginCanvas.SetActive(true);
        signupCanvas.SetActive(false);
    }
    private void ViewLogin()
    {
        loadingCanvas.SetActive(false);
        singinCanvas.SetActive(false);
        loginCanvas.SetActive(true);
        signupCanvas.SetActive(false);
        mailSignupCanvas.SetActive(false);
    }
    private void SignUp()
    {
        loadingCanvas.SetActive(false);
        singinCanvas.SetActive(false);
        loginCanvas.SetActive(false);
        //signupCanvas.SetActive (true);
        //現状メールサインアップのみの実装
        signupCanvas.SetActive(false);
        mailSignupCanvas.SetActive(true);
    }

}
