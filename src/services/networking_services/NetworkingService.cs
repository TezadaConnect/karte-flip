using Godot;

public partial class NetworkingService : Node{
    private const int PORT = 8080;
	private const string SERVER_ADDRESS = "127.0.0.1";
	private Label _usernameLabel;
    public override void _Ready(){
		// _usernameLabel = GetNode<Label>("/root/Node/NameLabel");
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
		peer.CreateServer(PORT, 2);
		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Server Created");
		// _usernameLabel.Text = "Player ID: " + GetUserID();
	}

	public void JoinAServer(){
		ENetMultiplayerPeer peer = new();
		peer.CreateClient(SERVER_ADDRESS, PORT);
		Multiplayer.MultiplayerPeer = peer;
		// _usernameLabel.Text = "Player ID: " + GetUserID();
	}

	public void LeaveServer(){
		Multiplayer.MultiplayerPeer = null;
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