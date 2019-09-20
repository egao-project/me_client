﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Text;

public class APIController {

    //アルバムの最大数を定数化
    public const int MAX = 3;

    //テストコード
    public static void Login(User model,UnityAction successCall, UnityAction failedCall)
    {
        //TODO:他のところに処理が行かないか調査
        //nextCanvas.SetActive(true);

        HttpConector http = new HttpConector();
        HttpItem r = http.Post(Const.LOGIN_URL, JsonUtility.ToJson(model));
        Debug.Log(r.code);
        Debug.Log(r.body);
        Debug.Log("ConstURL is:" + Const.LOGIN_URL);

        if (r.code == 200)
        {
            User u = JsonUtility.FromJson<User>(r.body);
            //ユーザ情報を保持
            u.username = model.username;
            u.password = model.password;
            BaseController.SetUser(u);

            Debug.Log("ログイン成功");
            Debug.Log(PlayerPrefs.GetString("TokenKey"));


            //PlayerPrefsに保存
            //TODO:ハッシュ化が必要
            PlayerPrefs.SetString("TokenKey", u.token);
            PlayerPrefs.SetString("UserID", u.username);
            PlayerPrefs.SetString("UserPass", u.password);
            successCall();
        }
        else
        {
            Debug.Log("ログイン失敗");
            failedCall();
        }
    }



    /// <summary>
    /// APIサーバに接続　GET
    /// </summary>
    /// <param name="url">URL.</param>
    public void APILoad(string url , UnityAction<string> successCall, UnityAction failedCall)
    {
        HttpConector http = new HttpConector();
        HttpItem r = http.Get(Const.FRAME_URL, "username=" + BaseController.user.username);
        Debug.Log(r.code);
        Debug.Log(r.body);
        Debug.Log("ConstURL is:" + Const.LOGIN_URL);
        if (r.code == 200)
        {
            successCall(r.body);
        }
        else
        {
            failedCall();
        }

    }

    /// <summary>
    /// APIサーバに接続　POST
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="json">Json.</param>
    public static void APIPost(string url, string json , UnityAction<string> successCall, UnityAction failedCall)
    {
        HttpConector http = new HttpConector();
        HttpItem r = http.Post(url, json);
        Debug.Log(r.code);
        Debug.Log(r.body);
        Debug.Log("ConstURL is:" + Const.LOGIN_URL);
        if (r.code == 200 || r.code == 201)
        {
            successCall(r.body);
        }
        else
        {
            failedCall();
        }
    }

    public static void ImageDelete(Picture p, Picture pp, Texture2D texture, Frame frame, int index, UnityAction<string> successCall, UnityAction failedCall)
    {
        HttpConector http = new HttpConector();
        if (p != null)
        {
            http.Delete(Const.PICTURE_DELETE_URL, p.id.ToString());
        }
        else
        {
            p = new Picture();
        }

        HttpItem r = http.PostImage(texture.EncodeToJPG(), frame.id, index);

        pp = JsonUtility.FromJson<Picture>(r.body);
    }

    public static HttpItem GetFrameJson(UnityAction<string> successCall, UnityAction failedCall)
    {
        HttpConector http = new HttpConector();
        return http.Get(Const.FRAME_URL, "username=" + BaseController.user.username);
    }

    public static void AddFrame(Frame[] list, int i, UnityAction<string> successCall, UnityAction failedCall)
    {
        HttpConector http = new HttpConector();
        HttpItem r = http.PostFrom(Const.FRAME_ADD_URL, BaseController.user.username, i);
        if (r.code == 200)
        {
            list[i] = JsonUtility.FromJson<Frame>(r.body);
        }
    }

}
