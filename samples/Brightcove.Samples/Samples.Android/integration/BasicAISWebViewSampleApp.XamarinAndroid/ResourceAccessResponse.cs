namespace com.brightcove.player.samples.ais.webview.basic
{

	using SerializedName = com.google.gson.annotations.SerializedName;

	/// <summary>
	/// JSON response class for the /identity/resourceAccess/ REST API call.
	/// Note: this is not a completed class. It has been implemented
	/// enough for the purposes of this sample app.
	/// 
	/// @author bhnath (Billy Hnath)
	/// </summary>
	public class ResourceAccessResponse
	{

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("_type") private String type;
		private string type;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("resource") private String resource;
		private string resource;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("security_token") private String securityToken;
		private string securityToken;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("expires") private int expires;
		private int expires;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("servertime") private int serverTime;
		private int serverTime;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("platform_id") private String platformId;
		private string platformId;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("message") private String message;
		private string message;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SerializedName("authorization") private boolean authorization;
		private bool authorization;

		public virtual string Type
		{
			get
			{
				return type;
			}
		}

		public virtual string Resource
		{
			get
			{
				return resource;
			}
		}

		public virtual string SecurityToken
		{
			get
			{
				return securityToken;
			}
		}

		public virtual int Expires
		{
			get
			{
				return expires;
			}
		}

		public virtual int ServerTime
		{
			get
			{
				return serverTime;
			}
		}

		public virtual string PlatformId
		{
			get
			{
				return platformId;
			}
		}

		public virtual string Message
		{
			get
			{
				return message;
			}
		}

		public virtual bool Authorization
		{
			get
			{
				return authorization;
			}
		}
	}

}