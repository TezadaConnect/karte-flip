using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class TurnRpcService : Node{
	private List<PlayerModel> _listOfPlayers;
	private TokenColorEnum _currentTokenTurn;

    public override void _Ready(){
        _listOfPlayers = new List<PlayerModel>(2);
		_currentTokenTurn = TokenColorEnum.LIGHT_TOKEN;
    }

	/*
	* ********************************************************
	*	SETTERS AND GETTERS
	* ********************************************************
	*/
	public TokenColorEnum CurrentTokenTurn {
		get { return _currentTokenTurn; }
		set { _currentTokenTurn = value; }
	}
	
	/*
	* ********************************************************
	*	RPC EXECUTER FUNCTIONS
	* ********************************************************
	*/
	public void ExecuteStartMatch(){
		Rpc(nameof(StartMatch));
	}

	public void ExecuteQuitMatch(){
		Rpc(nameof(QuitMatch));
	}

	public void ExecuteAddTokenToTilemap(Vector2I position, CardModel card){
		Dictionary serializeValue = card.Serialize();
		Rpc(nameof(AddTokenToTilemap), position, serializeValue);
	}

	/*
	* ********************************************************
	*	RPC LOGIC
	* ********************************************************
	*/
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void StartMatch(){
		List<TokenColorEnum> allTokens = new List<TokenColorEnum>{ 
			TokenColorEnum.LIGHT_TOKEN, 
			TokenColorEnum.DARK_TOKEN 
		};

		TokenColorEnum randomToken = allTokens[new Random().Next(allTokens.Count)];

		if(Multiplayer.IsServer()){
			_listOfPlayers.Add(new PlayerModel(){
				PlayerID = Multiplayer.GetUniqueId(), 
				PlayerType = PlayerTypeEnum.PERSON, 
				TokenColor = randomToken
			});
		} else {
			TokenColorEnum tokenHolder = randomToken == TokenColorEnum.DARK_TOKEN ? TokenColorEnum.LIGHT_TOKEN : TokenColorEnum.DARK_TOKEN;
			_listOfPlayers.Add(new PlayerModel(){
				PlayerID = Multiplayer.GetUniqueId(), 
				PlayerType = PlayerTypeEnum.PERSON, 
				TokenColor = tokenHolder
			});
		}

		RouteManager.GetIntance().MoveToScene(SceneFileNameEnum.MAIN_SCENE, GetTree());
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void QuitMatch(){
		RouteManager.GetIntance().MoveToScene(SceneFileNameEnum.LOBBY_SCENE, GetTree());
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void AddTokenToTilemap(Vector2I position, Dictionary card){
		GD.Print(card);
		CardModel cardModel = CardModel.Deserialize(card);
		GridGroundTilemap holder = GetNode<GridGroundTilemap>("/root/MainScene/GridGroundTilemap");
		TileHelper.AddAtlasFromGameTurnManagerToTilemap(position, holder, _currentTokenTurn);
		TokenFlipService.FlipTokens(position, holder, cardModel.CardListFlipDirections , _currentTokenTurn);
		_currentTokenTurn = _currentTokenTurn == TokenColorEnum.LIGHT_TOKEN ? TokenColorEnum.DARK_TOKEN : TokenColorEnum.LIGHT_TOKEN;
	}
}