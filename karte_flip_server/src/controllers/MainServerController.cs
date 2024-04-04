using Godot;

public partial class MainServerController : Node{
	private PlayerTurnManager _matchHandlerService;
    private const int PORT = 0000;
	private const int MAX_PLAYERS = 1000;
    public override void _Ready(){
		// Connect the function to the signal
		GD.Print("Wecome to Karte Flip Server!");
		_matchHandlerService = GetNode<PlayerTurnManager>("/root/PLAYER_TURN_MANAGER");
		Multiplayer.PeerConnected += PeerConnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
		CreateAServer();
	}

    /*
	* ***********************************************
	*  JOINING, CREATING, LEAVING SERVER
	* ***********************************************
	*/
	public void CreateAServer(){
		ENetMultiplayerPeer peer = new();
		Error status = peer.CreateServer(PORT, MAX_PLAYERS);
		if(status != Error.Ok){
			GD.Print("Failed To Create");
			return;
		}
		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Server Created, Waiting for players");
	}

	/*
	* **********************************************
	*  NETWORK CONNECTION RESPONSE TO SERVER
	* **********************************************
	*/
	private void PeerConnected(long id){
		string userID = id.ToString();
		GD.Print("User: " + userID + " connected to the server");
		if(userID != null){
			_matchHandlerService.AddClientRecord(id);
			_matchHandlerService.FindMatchForClient(id);
		}
	}

	private void PeerDisconnected(long id){
		string userID = id.ToString();
		GD.Print("User: " + userID + " disconnected to the server");
		_matchHandlerService.RemoveClientRecordWhenDisconnected(id);
	}
}

