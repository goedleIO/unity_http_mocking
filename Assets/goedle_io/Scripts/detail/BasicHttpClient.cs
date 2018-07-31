// ===============================
// AUTHOR     : Marc Müller goedle.io GmbH
// CREATE DATE     : 31.07.2018
// PURPOSE     : Basic HTTP Client to send a post request
//=================================

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
namespace goedle.detail
{
	public class BasicHttpClient : MonoBehaviour
    {

		public void sendPost(string content, IGoedleWebRequest gwr, IGoedleUploadHandler guh)
        {
			StartCoroutine(basicPostClient(content, gwr, guh));
        }
  
		public IEnumerator basicPostClient(string content, IGoedleWebRequest gwr, IGoedleUploadHandler guh)
        {
            guh.add(content);
			gwr.unityWebRequest = new UnityWebRequest();
			using (gwr.unityWebRequest)
            {
				gwr.method = "POST";
                gwr.url = "url";
                gwr.uploadHandler = guh.uploadHandler;
				gwr.unityWebRequest.SetRequestHeader("Content-Type", "application/json");
				gwr.chunkedTransfer = false;
                yield return gwr.unityWebRequest.SendWebRequest();
                if (gwr.isNetworkError || gwr.isHttpError)
                {
					Debug.LogError("isHttpError: " + gwr.isHttpError);
					Debug.LogError("isNetworkError: " + gwr.isNetworkError);
                }
            }
        }
    }
}


