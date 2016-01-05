using System;
using System.Collections.Generic;

namespace com.brightcove.player.samples.adobepass.webview.basic
{

	using Intent = android.content.Intent;
	using Bitmap = android.graphics.Bitmap;
	using SslError = android.net.http.SslError;
	using Bundle = android.os.Bundle;
	using Looper = android.os.Looper;
	using Base64 = android.util.Base64;
	using Log = android.util.Log;
	using SslErrorHandler = android.webkit.SslErrorHandler;
	using WebSettings = android.webkit.WebSettings;
	using WebView = android.webkit.WebView;
	using WebViewClient = android.webkit.WebViewClient;

	using AccessEnabler = com.adobe.adobepass.accessenabler.api.AccessEnabler;
	using AccessEnablerException = com.adobe.adobepass.accessenabler.api.AccessEnablerException;
	using IAccessEnablerDelegate = com.adobe.adobepass.accessenabler.api.IAccessEnablerDelegate;
	using Event = com.adobe.adobepass.accessenabler.models.Event;
	using MetadataKey = com.adobe.adobepass.accessenabler.models.MetadataKey;
	using MetadataStatus = com.adobe.adobepass.accessenabler.models.MetadataStatus;
	using Mvpd = com.adobe.adobepass.accessenabler.models.Mvpd;
	using EventEmitter = com.brightcove.player.@event.EventEmitter;
	using Catalog = com.brightcove.player.media.Catalog;
	using PlaylistListener = com.brightcove.player.media.PlaylistListener;
	using VideoFields = com.brightcove.player.media.VideoFields;
	using VideoListener = com.brightcove.player.media.VideoListener;
	using Playlist = com.brightcove.player.model.Playlist;
	using Video = com.brightcove.player.model.Video;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;


	/// <summary>
	/// This app illustrates how to integrate AdobePass within a webview.
	/// 
	/// @author Billy Hnath (bhnath)
	/// </summary>
	public class MainActivity : BrightcovePlayer, IAccessEnablerDelegate
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

		private const string STAGING_URL = "sp.auth-staging.adobe.com/adobe-services";
		private const int WEBVIEW_ACTIVITY = 1;

		private EventEmitter eventEmitter;
		private AccessEnabler accessEnabler;

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			// When extending the BrightcovePlayer, we must assign the BrightcoveVideoView
			// before entering the superclass. This allows for some stock video player lifecycle
			// management.
			ContentView = R.layout.adobepass_activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			eventEmitter = brightcoveVideoView.EventEmitter;
			base.onCreate(savedInstanceState);

			// configure the AdobePass AccessEnabler library

			try
			{
				accessEnabler = AccessEnabler.Factory.getInstance(this);
				if (accessEnabler != null)
				{
					accessEnabler.Delegate = this;
					accessEnabler.useHttps(true);
				}
			}
			catch (AccessEnablerException e)
			{
				Log.e(TAG, "Failed to initialize the AccessEnabler library: " + e.Message);
				return;
			}

			string requestorId = Resources.getString(R.@string.requestor_id);
			string credentialStorePassword = Resources.getString(R.@string.credential_store_password);
			System.IO.Stream credentialStore = Resources.openRawResource(R.raw.adobepass);

			// A signature must be passed along with the requestor id from a private key and a password.
			PrivateKey privateKey = extractPrivateKey(credentialStore, credentialStorePassword);

			string signedRequestorId = null;
			try
			{
				signedRequestorId = generateSignature(privateKey, requestorId);
			}
			catch (AccessEnablerException e)
			{
				Log.e(TAG, "Failed to generate signature.");
			}

			// The production URL is the default when no URL is passed. If
			// we are using a staging requestorID, we need to pass the staging
			// URL.
			List<string> spUrls = new List<string>();
			spUrls.Add(STAGING_URL);

			// Set the requestor ID.
			accessEnabler.setRequestor(requestorId, signedRequestorId, spUrls);

			// TODO (once we media API changes are made):
			// Media API call will return result with nulled out URL fields if the media
			// is protected. We need to make the adobepass calls to get the token for the media,
			// then make another Media API call with the adobepass token included (in the header or
			// a cookie) which will return a result with non-nulled URL fields.

			// 1. Ignore URL fields on the first call.
			// 2. Make the AdobePass calls
			// 3. Add token to next Media API call.

			// Add a test video to the BrightcoveVideoView.
			IDictionary<string, string> options = new Dictionary<string, string>();
			IList<string> values = new List<string>(Arrays.asList(VideoFields.DEFAULT_FIELDS));
			Catalog catalog = new Catalog("ErQk9zUeDVLIp8Dc7aiHKq8hDMgkv5BFU7WGshTc-hpziB3BuYh28A..");
			catalog.findPlaylistByReferenceID("stitch", options, new PlaylistListenerAnonymousInnerClassHelper(this));
		}

		private class PlaylistListenerAnonymousInnerClassHelper : PlaylistListener
		{
			private readonly MainActivity outerInstance;

			public PlaylistListenerAnonymousInnerClassHelper(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public virtual void onPlaylist(Playlist playlist)
			{
				brightcoveVideoView.addAll(playlist.Videos);
			}

			public virtual void onError(string error)
			{
				Log.e(outerInstance.TAG, error);
			}
		}

		protected internal override void onActivityResult(int requestCode, int resultCode, Intent data)
		{
			Log.v(TAG, "onActivityResult: " + requestCode + ", " + resultCode + ", " + data);
			base.onActivityResult(requestCode, resultCode, data);
			if (resultCode == RESULT_CANCELED)
			{
				accessEnabler.SelectedProvider = null;
			}
			else if (resultCode == RESULT_OK)
			{
				accessEnabler.AuthenticationToken;
			}
		}

		// Make sure we log out once the application is killed.
		public override void onBackPressed()
		{
			Log.v(TAG, "onBackPressed");
			accessEnabler.logout();
			base.onBackPressed();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private String generateSignature(java.security.PrivateKey privateKey, String data) throws com.adobe.adobepass.accessenabler.api.AccessEnablerException
		private string generateSignature(PrivateKey privateKey, string data)
		{
			try
			{
				Signature rsaSigner = Signature.getInstance("SHA256WithRSA");
				rsaSigner.initSign(privateKey);
				rsaSigner.update(data.GetBytes());

				sbyte[] signature = rsaSigner.sign();
				return new string(Base64.encode(signature, Base64.DEFAULT));
			}
			catch (Exception e)
			{
				Log.e(TAG, e.ToString());
				throw new AccessEnablerException();
			}
		}

		private PrivateKey extractPrivateKey(System.IO.Stream PKCSFile, string password)
		{
			if (PKCSFile == null)
			{
				return null;
			}

			try
			{
				KeyStore keyStore = KeyStore.getInstance("PKCS12");
				keyStore.load(PKCSFile, password.ToCharArray());

				string keyAlias = null;
				IEnumerator<string> aliases = keyStore.aliases();
				while (aliases.MoveNext())
				{
					keyAlias = aliases.Current;
					if (keyStore.isKeyEntry(keyAlias))
					{
						break;
					}
				}

				if (keyAlias != null)
				{
					KeyStore.PrivateKeyEntry keyEntry = (KeyStore.PrivateKeyEntry) keyStore.getEntry(keyAlias, new KeyStore.PasswordProtection(password.ToCharArray()));
					return keyEntry.PrivateKey;
				}
			}
			catch (Exception e)
			{
				Log.e(TAG, e.Message);
			}
			return null;
		}

		public override int RequestorComplete
		{
			set
			{
				Log.v(TAG, "setRequestorComplete: " + value);
				if (value == AccessEnabler.ACCESS_ENABLER_STATUS_SUCCESS)
				{
					accessEnabler.Authentication;
				}
			}
		}

		public override void setAuthenticationStatus(int status, string errorCode)
		{
			Log.v(TAG, "setAuthenticationStatus: " + status + " , " + errorCode);
			if (status == AccessEnabler.ACCESS_ENABLER_STATUS_SUCCESS)
			{
				accessEnabler.getAuthorization("2149332630001");
			}
			else if (status == AccessEnabler.ACCESS_ENABLER_STATUS_ERROR)
			{
				Log.v(TAG, "setAuthenticationStatus: authentication failed.");
			}
		}

		public override void setToken(string token, string resourceId)
		{
			Log.v(TAG, "setToken: " + token + " ," + resourceId);
		}

		public override void tokenRequestFailed(string resourceId, string errorCode, string errorDescription)
		{
			Log.v(TAG, "tokenRequestFailed: " + resourceId + ", " + errorCode + ", " + errorDescription);
		}

		public override void selectedProvider(Mvpd mvpd)
		{
			Log.v(TAG, "selectedProvider: " + mvpd);
		}

		public override void displayProviderDialog(List<Mvpd> mvpds)
		{
			Log.v(TAG, "displayProviderDialog:" + mvpds);
			accessEnabler.SelectedProvider = mvpds[0].Id;
		}

		// Open the webview activity here to do the URL redirection for both
		// logging in and logging out.
		public override void navigateToUrl(string url)
		{
			Log.v(TAG, "navigateToUrl: " + url);
			Intent intent = new Intent(MainActivity.this, typeof(WebViewActivity));
			intent.putExtra("url", url);
			startActivityForResult(intent, WEBVIEW_ACTIVITY);
		}

		public override void sendTrackingData(Event @event, List<string> data)
		{
			Log.v(TAG, "sendTrackingData: " + @event + ", " + data);
		}

		public override void setMetadataStatus(MetadataKey metadataKey, MetadataStatus metadataStatus)
		{
			Log.v(TAG, "setMetadataStatus: " + metadataKey + ", " + metadataStatus);
		}

		public override void preauthorizedResources(List<string> resources)
		{
			Log.v(TAG, "preauthorizedResources:" + resources);
		}
	}

}