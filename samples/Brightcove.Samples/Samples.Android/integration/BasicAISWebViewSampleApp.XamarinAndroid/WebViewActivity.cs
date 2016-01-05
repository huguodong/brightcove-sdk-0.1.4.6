namespace com.brightcove.player.samples.ais.webview.basic
{

	using Activity = android.app.Activity;
	using Intent = android.content.Intent;
	using Bitmap = android.graphics.Bitmap;
	using Uri = android.net.Uri;
	using SslError = android.net.http.SslError;
	using Bundle = android.os.Bundle;
	using Log = android.util.Log;
	using CookieManager = android.webkit.CookieManager;
	using CookieSyncManager = android.webkit.CookieSyncManager;
	using SslErrorHandler = android.webkit.SslErrorHandler;
	using WebView = android.webkit.WebView;
	using WebViewClient = android.webkit.WebViewClient;

	/// <summary>
	/// A webview activity to access ais login pages for credential entry.
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

			string AIS_TARGET_URL = Resources.getString(R.@string.ais_target_url);

			Intent intent = Intent;
			string url = intent.getStringExtra(AIS_TARGET_URL);

			ContentView = R.layout.ais_webview_activity_main;
			webView = (WebView) findViewById(R.id.sampleWebView);
			webView.WebViewClient = webViewClient;

			webView.Settings.JavaScriptEnabled = true;
			webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;

			// Append '?responsemethod=redirect' to the AIS call, otherwise we
			// will not get the proper redirects to complete authentication.
			Uri.Builder builder = Uri.parse(url).buildUpon();
			builder.appendQueryParameter("responsemethod", "redirect");
			url = builder.build().ToString();

			webView.loadUrl(url);
		}

		private readonly WebViewClient webViewClient = new WebViewClientAnonymousInnerClassHelper();

		private class WebViewClientAnonymousInnerClassHelper : WebViewClient
		{
			public WebViewClientAnonymousInnerClassHelper()
			{
			}


			public override bool shouldOverrideUrlLoading(WebView view, string url)
			{
				Log.d(outerInstance.TAG, "Loading URL: " + url);
				string AIS_REDIRECT_TOKEN = Resources.getString(R.@string.ais_redirect_token);
				string AIS_DOMAIN = Resources.getString(R.@string.ais_domain);
				string AIS_WEBVIEW_COOKIE = Resources.getString(R.@string.ais_webview_cookie);
				// Once we've hit the final redirect URL to complete authentication,
				// harvest the cookie from this webview and pass it back to the main
				// activity.
				if (url.Contains(AIS_REDIRECT_TOKEN) && url.Contains("aisresponse"))
				{
					string cookie = CookieManager.Instance.getCookie(AIS_DOMAIN);
					Intent result = new Intent(outerInstance, typeof(MainActivity));
					result.putExtra(AIS_WEBVIEW_COOKIE, cookie);
					CookieSyncManager.Instance.sync();
					setResult(RESULT_OK, result);
					finish();
					return true;
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