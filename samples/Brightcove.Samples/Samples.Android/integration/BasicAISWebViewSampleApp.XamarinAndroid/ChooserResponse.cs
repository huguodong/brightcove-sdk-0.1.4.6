using System.Collections.Generic;

namespace com.brightcove.player.samples.ais.webview.basic
{

	using SerializedName = com.google.gson.annotations.SerializedName;

	/// <summary>
	/// JSON response class for the /chooser/ REST API call.
	/// Note: this is not a completed class. It has been implemented
	/// enough for the purposes of this sample app.
	/// 
	/// @author bhnath (Billy Hnath)
	/// </summary>
	public class ChooserResponse
	{

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("grouped_idps") private java.util.Map<String, String> groupedIdps;
		private IDictionary<string, string> groupedIdps;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("_type") private String type;
		private string type;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("authenticated") private boolean authenticated;
		private bool authenticated;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("possible_idps") private java.util.Map<String, IdentityProvider> possibleIdps;
		private IDictionary<string, IdentityProvider> possibleIdps;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("footprints") private java.util.Map<String, String> footprints;
		private IDictionary<string, string> footprints;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("platform_id") private String platformId;
		private string platformId;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("preferred_idps") private java.util.Map<String, String> preferredIdps;
		private IDictionary<string, string> preferredIdps;

		public virtual string Type
		{
			get
			{
				return type;
			}
		}
		public virtual bool Authenticated
		{
			get
			{
				return authenticated;
			}
		}

		public virtual IDictionary<string, IdentityProvider> PossibleIdps
		{
			get
			{
				return possibleIdps;
			}
		}

		public class IdentityProvider
		{
			private readonly ChooserResponse outerInstance;

			public IdentityProvider(ChooserResponse outerInstance)
			{
				this.outerInstance = outerInstance;
			}


//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("url") private String url;
			internal string url;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("logos") private java.util.Map<String, String> logos;
			internal IDictionary<string, string> logos;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("display_name") private String displayName;
			internal string displayName;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("name") private String name;
			internal string name;

			public virtual string Url
			{
				get
				{
					return url;
				}
			}
			public virtual IDictionary<string, string> Logos
			{
				get
				{
					return logos;
				}
			}
			public virtual string DisplayName
			{
				get
				{
					return displayName;
				}
			}
			public virtual string Name
			{
				get
				{
					return name;
				}
			}
		}
	}

}