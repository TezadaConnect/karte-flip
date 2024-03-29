using Godot;

public partial class NetworkingService : Node{
    private const int PORT = 8080;
	private const string SERVER_ADDRESS = "127.0.0.1";
	private Label _usernameLabel;
	private TurnRpcService turnRpcService;
    public override void _Ready(){
		// _usernameLabel = GetNode<Label>("/root/Node/NameLabel");
		turnRpcService = GetNode<TurnRpcService>(
			RouteManager.GetIntance().GetSingletonAutoLoad(SingletonAutoLoadEnum.TURN_RPC_SERVICE)
		);
		// Connect the function to the signal
		Multiplayer.PeerConnected += PeerConnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
		Multiplayer.ServerDisconnected += ServerDisconnected;
	}

    /*
	* ***********************************************
	*  JOINING, CREATING, LEAVING SERVER
	* ***********************************************
	*/
	public void CreateAServer(){
		ENetMultiplayerPeer peer = new();
		Error status = peer.CreateServer(PORT, 1);
		if(status != Error.Ok){
			GD.Print("Failed To Create");
			return;
		}
		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Server Created, Waiting for players");
		// _usernameLabel.Text = "Player ID: " + GetUserID();
	}

	public void JoinAServer(){
		ENetMultiplayerPeer peer = new();
		Error status = peer.CreateClient(SERVER_ADDRESS, PORT);
		if(status != Error.Ok){
			GD.Print("Failed To Connect");
			return;
		}
		Multiplayer.MultiplayerPeer = peer;
		// _usernameLabel.Text = "Player ID: " + GetUserID();
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void LeaveServer(){
		Multiplayer.MultiplayerPeer = null;
		turnRpcService.ExecuteQuitMatch();
	}

	// TO TERMINATE NETWORKING JUST SHIT NULL THE Multiplayer.MultiplayerPeer
	/*
	* **********************************************
	*  NETWORK CONNECTION RESPONSE TO SERVER
	* **********************************************
	*/
	private void PeerConnected(long id){
		string userID = id.ToString();
		GD.Print("User: " + userID + " connected to the server");
	}

	private void PeerDisconnected(long id){
		string userID = id.ToString();
		GD.Print("User: " + userID + " disconnected to the server");
	}

	/*
	* **********************************************
	*  NETWORK CONNECTION RESPONSE TO CLIENT
	* **********************************************
	*/
	private void ConnectedToServer(){
		GD.Print("Established server connection");
		turnRpcService.ExecuteStartMatch();
	}

	private void ConnectionFailed(){
		GD.Print("Failed to connect to the server.");
	}

	private void ServerDisconnected(){
		GD.Print("Server has disconnected,");
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