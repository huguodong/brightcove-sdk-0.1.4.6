using System.Collections.Generic;

namespace com.brightcove.player.samples.cast.basic
{

	using Bundle = android.os.Bundle;
	using Resources = android.content.res.Resources;
	using Log = android.util.Log;
	using LayoutInflater = android.view.LayoutInflater;
	using Menu = android.view.Menu;
	using MenuInflater = android.view.MenuInflater;
	using View = android.view.View;
	using ViewGroup = android.view.ViewGroup;

	using GoogleCastComponent = com.brightcove.cast.GoogleCastComponent;
	using GoogleCastEventType = com.brightcove.cast.GoogleCastEventType;
	using EventEmitter = com.brightcove.player.@event.EventEmitter;
	using BrightcovePlayerFragment = com.brightcove.player.view.BrightcovePlayerFragment;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;
	using MiniController = com.google.sample.castcompanionlibrary.widgets.MiniController;


	/// <summary>
	/// Created by bhnath on 3/10/14.
	/// </summary>
	public class GoogleCastSampleFragment : BrightcovePlayerFragment
	{
		public static readonly string TAG = typeof(GoogleCastSampleFragment).Name;

		private GoogleCastComponent googleCastComponent;
		private MiniController miniController;
		private EventEmitter eventEmitter;

		public GoogleCastSampleFragment()
		{
			HasOptionsMenu = true;
		}

		public override View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Perform the internal wiring to be able to make use of the BrightcovePlayerFragment.
			View view = inflater.inflate(R.layout.basic_cast_fragment, container, false);
			brightcoveVideoView = (BrightcoveVideoView) view.findViewById(R.id.brightcove_video_view);
			eventEmitter = brightcoveVideoView.EventEmitter;
			base.onCreateView(inflater, container, savedInstanceState);

			// Initialize the android_cast_plugin which requires the application id of your Cast
			// receiver application.
			string applicationId = Resources.getString(R.@string.application_id);
			googleCastComponent = new GoogleCastComponent(eventEmitter, applicationId, Activity);

			// Initialize the MiniController widget which will allow control of remote media playback.
			miniController = (MiniController) view.findViewById(R.id.miniController1);
			IDictionary<string, object> properties = new Dictionary<string, object>();
			properties[GoogleCastComponent.CAST_MINICONTROLLER] = miniController;
			eventEmitter.emit(GoogleCastEventType.SET_MINI_CONTROLLER, properties);

			// Send the location of the media (url) and its metadata information for remote playback.
			Resources resources = Resources;
			string title = resources.getString(R.@string.media_title);
			string studio = resources.getString(R.@string.media_studio);
			string url = resources.getString(R.@string.media_url);
			string thumbnailUrl = resources.getString(R.@string.media_thumbnail);
			string imageUrl = resources.getString(R.@string.media_image);
			eventEmitter.emit(GoogleCastEventType.SET_MEDIA_METADATA, buildMetadataProperties("subTitle", title, studio, thumbnailUrl, imageUrl, url));

			brightcoveVideoView.VideoPath = url;

			return view;
		}

		private IDictionary<string, object> buildMetadataProperties(string subTitle, string title, string studio, string imageUrl, string bigImageUrl, string url)
		{
			Log.v(TAG, "buildMetadataProperties: subTitle " + subTitle + ", title: " + title + ", studio: " + studio + ", imageUrl: " + imageUrl + ", bigImageUrl: " + bigImageUrl + ", url: " + url);
			IDictionary<string, object> properties = new Dictionary<string, object>();
			properties[GoogleCastComponent.CAST_MEDIA_METADATA_SUBTITLE] = subTitle;
			properties[GoogleCastComponent.CAST_MEDIA_METADATA_TITLE] = title;
			properties[GoogleCastComponent.CAST_MEDIA_METADATA_STUDIO] = studio;
			properties[GoogleCastComponent.CAST_MEDIA_METADATA_IMAGE_URL] = imageUrl;
			properties[GoogleCastComponent.CAST_MEDIA_METADATA_BIG_IMAGE_URL] = bigImageUrl;
			properties[GoogleCastComponent.CAST_MEDIA_METADATA_URL] = url;
			return properties;
		}

		public override void onCreateOptionsMenu(Menu menu, MenuInflater inflater)
		{
			Log.v(TAG, "onCreateOptionsMenu");
			inflater.inflate(R.menu.main, menu);
			base.onCreateOptionsMenu(menu, inflater);

			IDictionary<string, object> properties = new Dictionary<string, object>();
			properties[GoogleCastComponent.CAST_MENU] = menu;
			properties[GoogleCastComponent.CAST_MENU_RESOURCE_ID] = R.id.media_router_menu_item;
			eventEmitter.emit(GoogleCastEventType.SET_CAST_BUTTON, properties);
		}
	}

}