package md51430c58e17cae80d0d18a8a89f933d5a;


public class ActivityPlayerSimpleFromCatalog_PlaylistListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.brightcove.player.media.PlaylistListener,
		com.brightcove.player.media.ErrorListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onPlaylist:(Lcom/brightcove/player/model/Playlist;)V:GetOnPlaylist_Lcom_brightcove_player_model_Playlist_Handler:BrightcoveSDK.Player.Media.IPlaylistListenerInvoker, Brightcove.Bindings.XamarinAndroid\n" +
			"n_onError:(Ljava/lang/String;)V:GetOnError_Ljava_lang_String_Handler:BrightcoveSDK.Player.Media.IErrorListenerInvoker, Brightcove.Bindings.XamarinAndroid\n" +
			"";
		mono.android.Runtime.register ("QuickStart.Player.XamarinAndroid.ActivityPlayerSimpleFromCatalog+PlaylistListener, QuickStart.Player.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ActivityPlayerSimpleFromCatalog_PlaylistListener.class, __md_methods);
	}


	public ActivityPlayerSimpleFromCatalog_PlaylistListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ActivityPlayerSimpleFromCatalog_PlaylistListener.class)
			mono.android.TypeManager.Activate ("QuickStart.Player.XamarinAndroid.ActivityPlayerSimpleFromCatalog+PlaylistListener, QuickStart.Player.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onPlaylist (com.brightcove.player.model.Playlist p0)
	{
		n_onPlaylist (p0);
	}

	private native void n_onPlaylist (com.brightcove.player.model.Playlist p0);


	public void onError (java.lang.String p0)
	{
		n_onError (p0);
	}

	private native void n_onError (java.lang.String p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
