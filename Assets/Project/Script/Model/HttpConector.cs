using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpConector {

	const int MAX_WAIT = 100;

	public HttpItem Post(string url,string json)
	{
		var request = new UnityWebRequest(url, "POST");
		byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
		request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

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