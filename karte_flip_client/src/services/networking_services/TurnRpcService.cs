using System.Linq;
using Godot;
using Godot.Collections;

namespace KarteFlipClient{
	public partial class TurnRpcService : Node{
		// private TokenColorEnum _currentTokenTurn;
		private PlayerModel _currentPlayerTurn;
		private PlayerModel _playerOne;
		private PlayerModel _playerTwo;
		private GridGroundTilemap _mainTilemap;
		private TextureRect _turnDisplayTextureRect;
		private RouteManager _routeManager;

        public override void _Ready(){
            _routeManager = GetNode<RouteManager>(
				RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.ROUTE_MANAGER)
			);
        }

        /*
		* ********************************************************
		*	SETTERS AND GETTERS
		* ********************************************************
		*/
        public PlayerModel CurrentPlayerTurn {
			get { return _currentPlayerTurn; }
			set { _currentPlayerTurn = value; }
		}
		
		/*
		* ********************************************************
		*	RPC EXECUTER FUNCTIONS
		* ********************************************************
		*/
		public void ExecuteQuitMatch(){
			Multiplayer.MultiplayerPeer = null;
		}

		public void ExecuteAddTokenToTilemap(Vector2I position, CardModel card){
			Dictionary serializeValue = card.Serialize();
			RpcId(1, nameof(ProcessTurnDataOnServer), position, serializeValue);
		}

		/*
		* ********************************************************
		*	RPC FUNCTIONS
		* ********************************************************
		*/
		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
		private void StartMatch(Array<Dictionary> players){ // client function
			foreach(Dictionary element in players.ToList()){
				PlayerModel playerInfoHolder = PlayerModel.Deserialize(element);
				if(playerInfoHolder.TokenColor == TokenColorEnum.LIGHT_TOKEN){
					_currentPlayerTurn = playerInfoHolder;
				}
				if(playerInfoHolder.PlayerID == Multiplayer.GetUniqueId()){
					_playerOne = playerInfoHolder;
					continue;
				}
				_playerTwo = playerInfoHolder;
			}
			_routeManager.MoveToScene(SceneFilenameEnum.MAIN_SCENE, "Starting match, please wait.");
		}

		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
		private void RecieveDataToSpecificClient(Vector2I position, Dictionary card){ // client function
			AddTokenToTilemap(position, card);	
		}

		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
		private void DisconnectClient(){ // client function
			Multiplayer.MultiplayerPeer = null;
			_routeManager.MoveToScene(SceneFilenameEnum.LOBBY_SCENE, "Your opponent leave the match.");
		}

		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
		private void ProcessTurnDataOnServer(Vector2I position, Dictionary card){} // server function

		/*
		* ********************************************************
		*	LOGICS
		* ********************************************************
		*/
		private void AddTokenToTilemap(Vector2I position, Dictionary card){
			_mainTilemap ??= GetNode<GridGroundTilemap>("/root/MainScene/GridGroundTilemap");
			_turnDisplayTextureRect ??= GetNode<TextureRect>("/root/MainScene/HUDTextureRect/TurnTextureRect");
			
			CardModel cardModel = CardModel.Deserialize(card);
			
			TileHelper.AddAtlasToken(
				position, 
				_mainTilemap, 
				_currentPlayerTurn.TokenColor
			);
			TokenFlipService.FlipTokens(
				position, 
				_mainTilemap, 
				cardModel.CardListFlipDirections , 
				_currentPlayerTurn.TokenColor
			);
			_currentPlayerTurn = _currentPlayerTurn.Equals(_playerOne) ? _playerTwo : _playerOne;

			if(_currentPlayerTurn.TokenColor == TokenColorEnum.DARK_TOKEN){
				_turnDisplayTextureRect.Texture = RouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.BLACK_TOKEN);
			} else {
				_turnDisplayTextureRect.Texture = RouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.WHITE_TOKEN);
			}
		}

		public bool IsMyTurn(){
			if(_currentPlayerTurn.PlayerID == Multiplayer.GetUniqueId()){
				return true;
			}
			return false;
		}
	}
}