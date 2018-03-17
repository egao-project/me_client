using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpConector {

	const int MAX_WAIT = 100;
	const string METHOD_POST = "POST";
	const string METHOD_GET = "GET";

	public HttpItem Post(string url,string json)
	{
		return Do(url, json, METHOD_POST);
	}
	public HttpItem Get(string url,string param)
	{
		url = url + "?" + param;
		return Do(url, null, METHOD_GET);
	}

	public HttpItem Do(string url,string json,string method) {
		var request = new UnityWebRequest(url, method);
		if (json != null) {
			byte[] bodyRaw = Encoding.UTF8.GetBytes (json);
			request.uploadHandler = (UploadHandler)new UploadHandlerRaw (bodyRaw);
			request.SetRequestHeader ("Content-Type", "application/json");
		}
		request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer ();
		if (BaseController.user != null) {
			request.SetRequestHeader ("Authorization", "JWT " + BaseController.user.token);
		}

		// 下記でも可
		// UnityWebRequest request = new UnityWebRequest("http://example.com");
		// methodプロパティにメソッドを渡すことで任意のメソッドを利用できるようになった
		// request.method = UnityWebRequest.kHttpVerbGET;

		// リクエスト送信
		request.Send();

		int count = 0;
		while (!request.isDone || count < MAX_WAIT) {
			DelayMethod(0.5f);
			count++;
		}

		if (request.isError)
			Debug.Log (request.error);

		HttpItem ret = new HttpItem ();
		ret.code = request.responseCode;
		ret.body = request.downloadHandler.text;

		return ret;

	}

	private IEnumerator DelayMethod(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	}
}