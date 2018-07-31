# Mocking HTTP Requests in Unity for Unit test 

The overall objective is to mock the [**UnityWebRequest**](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html).

We found a solution to mock HTTP requests in Unity to write unit test, with the help of [**Nsubstitute**](http://nsubstitute.github.io/) and a wrapper interface.

To test the **UnityWebRequest**, we use two interfaces to wrap the original **UnityWebRequest** and the [**UploadHandler**](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest-uploadHandler.html). These interfaces are [**IGoedleUploadHandler** ](https://github.com/goedleIO/unity_http_mocking/blob/master/Assets/goedle_io/Scripts/detail/GoedleUploadHandler.cs) and [**IGoedleWebRequest**](https://github.com/goedleIO/unity_http_mocking/blob/master/Assets/goedle_io/Scripts/detail/GoedleWebRequest.cs).

With the interfaces we are able to mock:

* Response Codes
* Content
* Header
* HTTP errors
* Networks errors
* Request methods

You find in this repository a minimal full running example, which can be opened as Unity project and directly execute with the [Unity Test Runner](https://docs.unity3d.com/Manual/testing-editortestsrunner.html). 

![Unity Test Runner](http://www.goedle.io/unity/unity_test_runner.png "Unity Test Runner")

Therefore, we created a basic [HTTP client](https://github.com/goedleIO/unity_http_mocking/blob/master/Assets/goedle_io/Scripts/detail/BasicHttpClient.cs) in Unity to test a **UnityWebRequest**. In the [unit test](https://github.com/goedleIO/unity_http_mocking/blob/master/Assets/goedle_io/Editor/Tests/TrackRequestTest.cs) we can now mock **UnityWebRequest** attributes and access passed content. The following example shows how to mock a web request and validate the send JSON content and HTTP errors:

##Test

```
[Test]
        public void checkBehaviorWebRequestSendPost()
        {
            BasicHttpClient http_client = (new GameObject("BasicHttpClient")).AddComponent<BasicHttpClient>();
            IGoedleUploadHandler guh = Substitute.For<IGoedleUploadHandler>();
            IGoedleWebRequest gw = Substitute.For<IGoedleWebRequest>();
            string stringContent = null;

            // Here we write the value which is passed through the add function to stringContent
            guh.add(Arg.Do<string>(x => stringContent = x));
            gw.isHttpError.Returns(false);
            gw.isNetworkError.Returns(false);
            http_client.sendPost("{\"test\": \"test\"}", gw, guh);
            var result = JSON.Parse(stringContent);
            Assert.AreEqual(result["test"].Value, "test");
        }
```

## IGoedleWebRequest

```
    public interface IGoedleWebRequest
    {
        bool isHttpError { get; }
        bool isNetworkError { get; }
        string url { get; set; }
        long responseCode { get; }
        string method { get; set; }
        bool chunkedTransfer{ get; set; }
        DownloadHandler downloadHandler { get; set; }
        UploadHandler uploadHandler { get; set; }
        UnityWebRequest unityWebRequest { get; set; }
        UnityWebRequestAsyncOperation SendWebRequest();
        void SetRequestHeader(string name, string value);
    }

    public class GoedleWebRequest : IGoedleWebRequest{

        UnityWebRequest _unityWebRequest { get; set; }

        public bool isNetworkError
        {
            get { return _unityWebRequest.isNetworkError; }

        }

        public bool isHttpError
        {
            get { return _unityWebRequest.isHttpError; }
        }

        public string url
        {
            get { return _unityWebRequest.url; }
            set { _unityWebRequest.url = value; }
        }

        public string method
        {
            get { return _unityWebRequest.method; }
            set { _unityWebRequest.method = value; }
        }

        public bool chunkedTransfer
        {
            get { return _unityWebRequest.chunkedTransfer; }
            set { _unityWebRequest.chunkedTransfer = value; }
        }

        public UnityWebRequest unityWebRequest
        {
            get { return _unityWebRequest; }
            set { _unityWebRequest = value; }

        }

        public DownloadHandler downloadHandler 
        { 
            get { return _unityWebRequest.downloadHandler;  }
            set { _unityWebRequest.downloadHandler = value;  }
        }

        public UploadHandler uploadHandler
        {
            get { return _unityWebRequest.uploadHandler; }
            set { _unityWebRequest.uploadHandler = value; }
        }

        public UnityWebRequestAsyncOperation SendWebRequest()
        {
            return _unityWebRequest.SendWebRequest();
        }

        public long responseCode
        {
            get { return _unityWebRequest.responseCode; }
        }

        public void SetRequestHeader(string name, string value)
        {
            _unityWebRequest.SetRequestHeader(name,value);
        }
    }
```

## IGoedleUploadHandler
```
    public interface IGoedleUploadHandler
    {
        UploadHandler uploadHandler { get; set; }
        void add(string stringContent);
        string getDataString();
    }

    public class GoedleUploadHandler : IGoedleUploadHandler
    {
        UploadHandler _uploadHandler { get; set; }
        public void add (string stringContent){
            byte[] byteContentRaw = new UTF8Encoding().GetBytes(stringContent);
            _uploadHandler = (UploadHandler)new UploadHandlerRaw(byteContentRaw);
        }

        public string getDataString()
        { 
            return Encoding.UTF8.GetString(_uploadHandler.data);
        }

        public UploadHandler uploadHandler
        {
            get { return _uploadHandler; }
            set { _uploadHandler = value; }
        }
    }
```

## Additional Information and Links

To create and validate JSON we are making use of the [**SIMPLEJSON**](http://wiki.unity3d.com/index.php/SimpleJSON) script. 

We are also using these testing framework in the [goedle.io Unity SDK](https://github.com/goedleIO/sdk_unity). You can also access the goedle.io Unity SDK via the [RAGE Platform](https://www.gamecomponents.eu/content/565) and you find a hands-on-example of on how to use the SDK [here](https://http://blog.goedle.io/2017/05/30/start-your-turbine-engines-goedle-io-now-supports-unity-and-offers-a-unity-tracking-sdk/?utm_source=blog&utm_medium=textlink&utm_campaign=github). 

The blog post [Fake it until you make it](https://http://blog.goedle.io) contains more detailed information and a description on how to create unit test for the **UnityWebRequest**.