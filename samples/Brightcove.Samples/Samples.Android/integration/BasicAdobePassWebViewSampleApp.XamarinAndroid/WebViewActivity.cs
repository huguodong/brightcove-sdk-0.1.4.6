namespace com.brightcove.player.samples.adobepass.webview.basic
{

	using Activity = android.app.Activity;
	using Intent = android.content.Intent;
	using Bitmap = android.graphics.Bitmap;
	using SslError = android.net.http.SslError;
	using Bundle = android.os.Bundle;
	using Log = android.util.Log;
	using SslErrorHandler = android.webkit.SslErrorHandler;
	using WebView = android.webkit.WebView;
	using WebViewClient = android.webkit.WebViewClient;

	using AccessEnabler = com.adobe.adobepass.accessenabler.api.AccessEnabler;

	/// <summary>
	/// A webview activity to access the adobepass login pages for credential entry.
	/// 
	/// @author Billy Hnath (bhnath)
	/// </summary>
	public class WebViewActivity : Activity
	{
		private bool InstanceFieldsInitialized = false;

		public WebViewActivity()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

		private void InitializeInstanceFields()
		{
			TAG = this.GetType().Name;
		}


		private string TAG;

		private WebView webView;

		public override void onCreate(Bundle savedInstanceState)
		{
			base.onCreate(savedInstanceState);

			Intent intent = Intent;
			string url = intent.getStringExtra("url");

			ContentView = R.layout.adobepass_webview_activity_main;
			webView = (WebView) findViewById(R.id.sampleWebView);
			webView.WebViewClient = webViewClient;

			webView.Settings.JavaScriptEnabled = true;
			webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;

			Log.v(TAG, "Loading: " + url);
			webView.loadUrl(url);
		}

		private readonly WebViewClient webViewClient = new WebViewClientAnonymousInnerClassHelper();

		private class WebViewClientAnonymousInnerClassHelper : WebViewClient
		{
			public WebViewClientAnonymousInnerClassHelper()
			{
			}

			public virtual bool shouldOverrideUrlLoading(WebView view, string url)
			{
				Log.d(outerInstance.TAG, "Loading URL: " + url);

				// if we detect a redirect to our application URL, this is an indication
				// that the authN workflow was completed successfully
				if (url.Equals(URLDecoder.decode(AccessEnabler.ADOBEPASS_REDIRECT_URL)))
				{

					// the authentication workflow is now complete - go back to the main activity
					Intent result = new Intent(outerInstance, typeof(MainActivity));
					setResult(RESULT_OK, result);
					finish();
				}

				return false;
			}

			public override void onReceivedSslError(WebView view, SslErrorHandler handler, SslError error)
			{
				Log.d(outerInstance.TAG, "Ignoring SSL certificate error.");
				handler.proceed();
			}

			public override void onReceivedError(WebView view, int errorCode, string description, string failingUrl)
			{
				Log.d(outerInstance.TAG, description);
				Log.d(outerInstance.TAG, failingUrl);
				base.onReceivedError(view, errorCode, description, failingUrl);
			}

			public override void onPageStarted(WebView view, string url, Bitmap favicon)
			{
				Log.d(outerInstance.TAG, "Page started: " + url);
				base.onPageStarted(view, url, favicon);
			}

			public override void onPageFinished(WebView view, string url)
			{
				Log.d(outerInstance.TAG, "Page loaded: " + url);
				base.onPageFinished(view, url);
			}
		}
	}

}