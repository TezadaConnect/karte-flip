using Godot;

public partial class NetworkingService : Node{
    private const int PORT = 8080;
	private const string SERVER_ADDRESS = "127.0.0.1";
	private Label _usernameLabel;
    public override void _Ready(){
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
		Multiplayer.ServerDisconnected += ServerDisconnected;
	}

	public void JoinAServer(){
		ENetMultiplayerPeer peer = new();
		Error status = peer.CreateClient(SERVER_ADDRESS, PORT);
		if(status != Error.Ok){
			GD.Print("Failed To Connect");
			return;
		}
		Multiplayer.MultiplayerPeer = peer;
	}

	/*
	* **********************************************
	*  NETWORK CONNECTION RESPONSE TO CLIENT
	* **********************************************
	*/
	private void ConnectedToServer(){
		GD.Print("Established server connection");
	}

	private void ConnectionFailed(){
		GD.Print("Failed to connect to the server.");
	}

	private void ServerDisconnected(){
		GD.Print("Server has disconnected,");
		RouteManager.GetIntance().MoveToScene(SceneFileNameEnum.LOBBY_SCENE, GetTree());
	}

	/*
	* **********************************************
	*  FOR GETTING USERID OF THE APPLICATION
	* **********************************************
	*/
	public string GetUserID(){
		return Multiplayer.GetUniqueId().ToString();
	}

	/*
	* **********************************************
	*  TO KNOW IF THE APPLICATION HOST A SERVER
	* **********************************************
	*/
	public bool GetIsServer(){
		return Multiplayer.IsServer();
	}
}