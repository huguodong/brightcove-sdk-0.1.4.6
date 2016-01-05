namespace com.brightcove.player.samples.cast.basic
{

	using ActionBarActivity = android.support.v7.app.ActionBarActivity;
	using Bundle = android.os.Bundle;

	/// <summary>
	/// This app illustrates how to use the Google Cast Plugin with the
	/// Brightcove Player for Android.
	/// 
	/// @author Billy Hnath (bhnath@brightcove.com)
	/// </summary>
	public class MainActivity : ActionBarActivity
	{
		protected internal override void onCreate(Bundle savedInstanceState)
		{
			base.onCreate(savedInstanceState);
			ContentView = R.layout.basic_cast;
		}
	}
}