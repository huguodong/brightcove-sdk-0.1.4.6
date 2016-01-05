using System;
using System.Collections;
using System.Collections.Generic;

namespace com.brightcove.player.samples.ais.webview.basic
{

	using Intent = android.content.Intent;
	using Uri = android.net.Uri;
	using AsyncTask = android.os.AsyncTask;
	using Bundle = android.os.Bundle;
	using Log = android.util.Log;
	using CookieSyncManager = android.webkit.CookieSyncManager;
	using Toast = android.widget.Toast;

	using EventEmitter = com.brightcove.player.@event.EventEmitter;
	using Video = com.brightcove.player.model.Video;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;
	using Gson = com.google.gson.Gson;

	using HttpResponse = org.apache.http.HttpResponse;
	using CookieStore = org.apache.http.client.CookieStore;
	using HttpClient = org.apache.http.client.HttpClient;
	using HttpGet = org.apache.http.client.methods.HttpGet;
	using ClientContext = org.apache.http.client.protocol.ClientContext;
	using BasicCookieStore = org.apache.http.impl.client.BasicCookieStore;
	using DefaultHttpClient = org.apache.http.impl.client.DefaultHttpClient;
	using BasicClientCookie = org.apache.http.impl.cookie.BasicClientCookie;
	using BasicHttpContext = org.apache.http.protocol.BasicHttpContext;
	using EntityUtils = org.apache.http.util.EntityUtils;


	/// <summary>
	/// This app illustrates how to integrate Akamai Identity Services within a webview.
	/// 
	/// @author Billy Hnath (bhnath)
	/// </summary>
	public class MainActivity : BrightcovePlayer
	{
		private bool InstanceFieldsInitialized = false;

		public MainActivity()
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

		private const int WEBVIEW_ACTIVITY = 1;

		private EventEmitter eventEmitter;

		private string baseUrl;
		private string platformId;
		private string authorizationCookie = "";
		private string initUrl;
		private string chooseIdpUrl;
		private string authorizationResourceUrl;
		private string singleLogoutUrl;

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			// When extending the BrightcovePlayer, we must assign the BrightcoveVideoView
			// before entering the superclass. This allows for some stock video player lifecycle
			// management.
			ContentView = R.layout.ais_activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			eventEmitter = brightcoveVideoView.EventEmitter;
			base.onCreate(savedInstanceState);

			platformId = Resources.getString(R.@string.platform_id);
			baseUrl = Resources.getString(R.@string.base_url);

			// Basic REST API Calls (minimum required)
			initUrl = baseUrl + platformId + "/init/";
			chooseIdpUrl = baseUrl + platformId + "/chooser";
			authorizationResourceUrl = baseUrl + platformId + "/identity/resourceAccess/";
			singleLogoutUrl = baseUrl + platformId + "/slo/";

			// Initialize our cookie syncing mechanism for the webview and start the
			// authorization workflow.
			CookieSyncManager.createInstance(this);
			(new GetIdentityProvidersAsyncTask(this)).execute(chooseIdpUrl);

		}

		protected internal override void onActivityResult(int requestCode, int resultCode, Intent data)
		{
			Log.v(TAG, "onActivityResult: " + requestCode + ", " + resultCode + ", " + data);
			base.onActivityResult(requestCode, resultCode, data);

			string AIS_WEBVIEW_COOKIE = Resources.getString(R.@string.ais_webview_cookie);
			string resourceId = Resources.getString(R.@string.resource_id);

			// If the result back from the webview was OK, then try to access the asset via the
			// resource id. Otherwise alert the user that authorization was not successful.
			if (resultCode == RESULT_OK)
			{
				authorizationCookie = data.Extras.getString(AIS_WEBVIEW_COOKIE);
				(new ResourceAccessAsyncTask(this)).execute(authorizationResourceUrl + resourceId);
			}
			else
			{
				Toast.makeText(this, "Authorization was not successful.", Toast.LENGTH_SHORT).show();
			}
		}

		// Make sure we log out once the application is killed.
		public override void onBackPressed()
		{
			Log.v(TAG, "onBackPressed:");
			base.onBackPressed();
			string AIS_TARGET_URL = Resources.getString(R.@string.ais_target_url);
			Intent intent = new Intent(MainActivity.this, typeof(WebViewActivity));
			intent.putExtra(AIS_TARGET_URL, singleLogoutUrl);
			startActivityForResult(intent, WEBVIEW_ACTIVITY);
		}

		public virtual string httpGet(string url)
		{

			string domain = Resources.getString(R.@string.ais_domain);
			string result = "";

			CookieStore cookieStore = new BasicCookieStore();
			BasicHttpContext localContext = new BasicHttpContext();
			localContext.setAttribute(ClientContext.COOKIE_STORE, cookieStore);

			// If we have a cookie stored, parse and use it. Otherwise, use a default http client.
			try
			{
				HttpClient httpClient = new DefaultHttpClient();
				HttpGet httpGet = new HttpGet(url);
				if (!authorizationCookie.Equals(""))
				{
					string[] cookies = authorizationCookie.Split(";", true);
					for (int i = 0; i < cookies.Length; i++)
					{
						string[] kvp = cookies[i].Split("=", true);
						if (kvp.Length != 2)
						{
							throw new Exception("Illegal cookie: missing key/value pair.");
						}
						BasicClientCookie c = new BasicClientCookie(kvp[0], kvp[1]);
						c.Domain = domain;
						cookieStore.addCookie(c);
					}
				}
				HttpResponse httpResponse = httpClient.execute(httpGet, localContext);
				result = EntityUtils.ToString(httpResponse.Entity);
			}
			catch (Exception e)
			{
				Log.e(TAG, e.LocalizedMessage);
			}

			return result;
		}

		private class GetIdentityProvidersAsyncTask : AsyncTask<string, Void, string>
		{
			private readonly MainActivity outerInstance;

			public GetIdentityProvidersAsyncTask(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			internal string AIS_TARGET_URL = Resources.getString(R.@string.ais_target_url);

			protected internal override string doInBackground(params string[] @params)
			{
				return outerInstance.httpGet(@params[0]);
			}

			protected internal virtual void onPostExecute(string jsonResponse)
			{
				Log.v(outerInstance.TAG, "onPostExecute:");

				// Parse the JSON response, get the first IDP and pass it to the webview activity
				// to load the login page.
				IList<string> idps = new List<string>();
				Gson gson = new Gson();
				ChooserResponse response = gson.fromJson(jsonResponse, typeof(ChooserResponse));

				IEnumerator it = response.PossibleIdps.SetOfKeyValuePairs().GetEnumerator();
				while (it.hasNext())
				{
					DictionaryEntry pairs = (DictionaryEntry)it.next();
					idps.Add(pairs.Key.ToString());
					it.remove();
				}

				Intent intent = new Intent(outerInstance, typeof(WebViewActivity));
				intent.putExtra(AIS_TARGET_URL, outerInstance.initUrl + idps[0]);
				startActivityForResult(intent, WEBVIEW_ACTIVITY);
			}
		}

		private class ResourceAccessAsyncTask : AsyncTask<string, Void, string>
		{
			private readonly MainActivity outerInstance;

			public ResourceAccessAsyncTask(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			protected internal override string doInBackground(params string[] @params)
			{
				return outerInstance.httpGet(@params[0]);
			}

			protected internal virtual void onPostExecute(string jsonResponse)
			{
				Log.v(outerInstance.TAG, "onPostExecute:");

				// Parse the JSON response, get the token and append it to the protected media url.
				// Then play add the video to the view and play it.
				Gson gson = new Gson();
				ResourceAccessResponse response = gson.fromJson(jsonResponse, typeof(ResourceAccessResponse));

				string url = Resources.getString(R.@string.protected_media_url);
				Uri.Builder builder = Uri.parse(url).buildUpon();
				builder.appendQueryParameter("hdnea", response.SecurityToken);
				url = builder.build().ToString();
				brightcoveVideoView.add(Video.createVideo(url));
				brightcoveVideoView.start();
			}
		}
	}

}