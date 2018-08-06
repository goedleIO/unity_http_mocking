# Mocking HTTP Requests in Unity for Unit Tests

The overall objective is to mock the [**UnityWebRequest**](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html).

We found a solution to mock HTTP requests in Unity to write unit test, with the help of [**Nsubstitute**](http://nsubstitute.github.io/) and a wrapper interface.

To test the **UnityWebRequest**, we use two interfaces to wrap the original **UnityWebRequest** and the [**UploadHandler**](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest-uploadHandler.html). These interfaces are [**IGoedleUploadHandler** ](https://github.com/goedleIO/unity_http_mocking/blob/master/Assets/goedle_io/Scripts/detail/GoedleUploadHandler.cs) and [**IGoedleWebRequest**](https://github.com/goedleIO/unity_http_mocking/blob/master/Assets/goedle_io/Scripts/detail/GoedleWebRequest.cs).

With these interfaces we are able to mock:

* Response Codes
* Content
* Header
* HTTP errors
* Networks errors
* Request methods

Therefore, we created a basic [HTTP client](https://github.com/goedleIO/unity_http_mocking/blob/master/Assets/goedle_io/Scripts/detail/BasicHttpClient.cs) in Unity to test a **UnityWebRequest**. In the [unit test](https://github.com/goedleIO/unity_http_mocking/blob/master/Assets/goedle_io/Editor/Tests/TrackRequestTest.cs) we can now mock **UnityWebRequest** attributes and access passed content. The following example shows how to mock a web request and validate the send JSON content and HTTP errors:

## Example

You find in this repository a minimal full running example, which can be opened as Unity project and directly execute with the [Unity Test Runner](https://docs.unity3d.com/Manual/testing-editortestsrunner.html). 

### Test
```C#
[Test]
public void checkBehaviorWebRequestSendPost()
{
    BasicHttpClient http_client = (new GameObject("BasicHttpClient")).AddComponent();
    IGoedleUploadHandler guh = Substitute.For();
    IGoedleWebRequest gw = Substitute.For();
    string stringContent = null;
    // Here we write the value which is passed through the add function to stringContent
    guh.add(Arg.Do(x => stringContent = x));
    gw.isHttpError.Returns(false);
    gw.isNetworkError.Returns(false);
    http_client.sendPost("{\"test\": \"test\"}", gw, guh);
    var result = JSON.Parse(stringContent);
    Assert.AreEqual(result["test"].Value, "test");
}
```

### Directory Structure

```bash
├───Assets/
│   ├───goedle_io/
│   │   ├───Editor/
│   │   │   ├───Tests/
│   │   │   │   ├───TrackRequestTest.cs
│   │   │   ├───NSubstitute.dll
│   │   ├───Plugins/
│   │   │   ├───SimpleJSON.cs
│   │   ├───Scripts/
│   │   │   ├───detail/
│   │   │   │   ├───BasicHttpClient.cs
│   │   │   │   ├───GoedleUploadHandler.cs
│   │   │   │   ├───GoedleWebRequest.cs
```

* TrackRequestTest.cs -> Holds an example unit test
* BasicHttpClient.cs -> Holds the HTTP client that we want to test
* GoedleUploadHandler.cs -> Holds the interface for wrapped `UploadHandler`
* GoedleWebRequest.cs -> Holds the interface for the wrapped `UnityWebRequest` 

When you open this project in Unity you directly run the Unity Test Runner.

![Unity Test Runner](http://www.goedle.io/unity/unity_test_runner.png "Unity Test Runner")


## Additional Information and Links

To create and validate JSON we are making use of the [**SIMPLEJSON**](http://wiki.unity3d.com/index.php/SimpleJSON) script. 

We are also using these testing framework in the [goedle.io Unity SDK](https://github.com/goedleIO/sdk_unity). You can also access the goedle.io Unity SDK via the [RAGE Platform](https://www.gamecomponents.eu/content/565) and you find a hands-on-example of on how to use the SDK [here](https://http://blog.goedle.io/2017/05/30/start-your-turbine-engines-goedle-io-now-supports-unity-and-offers-a-unity-tracking-sdk/?utm_source=blog&utm_medium=textlink&utm_campaign=github). 

The blog post [Fake it until you make it](https://blog.goedle.io/2018/08/06/mocking-web-requests-in-unity-fake-it-until-you-make-it/?utm_source=github&utm_medium=textlink&utm_campaign=unityunittest) contains more detailed information and a description on how to create unit test for the **UnityWebRequest**.
