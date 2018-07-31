// ===============================
// AUTHOR     : Marc Müller goedle.io GmbH
// CREATE DATE     : 31.07.2018
// PURPOSE     : Test class to Unit test the UnityWebRequest and the content in the UploadHandler
//=================================

#define TESTING

namespace goedle.detail
{
	using UnityEngine;
    using NUnit.Framework;
    using NSubstitute;
	using SimpleJSON;

    [TestFixture]
	public class TrackRequestTest
    {
		public BasicHttpClient _http_client;
        IGoedleUploadHandler _guh = null;
        IGoedleWebRequest _gw = null;

		[SetUp]
        public void SetUp()
        {
			_http_client = (new GameObject("BasicHttpClient")).AddComponent<BasicHttpClient>();
            _guh = Substitute.For<IGoedleUploadHandler>();
			_gw = Substitute.For<IGoedleWebRequest>();
        }

		[Test]
        public void checkBehaviorWebRequestSendPost()
        {
			string stringContent = null;
			// Here we write the value which is passed through the add function to stringContent
			_guh.add(Arg.Do<string>(x => stringContent = x));
			_gw.isHttpError.Returns(false);
			_gw.isNetworkError.Returns(false);
			_http_client.sendPost("{\"test\": \"test\"}", _gw, _guh);
            var result = JSON.Parse(stringContent);
            Assert.AreEqual(result["test"].Value, "test");
        }
    }
}