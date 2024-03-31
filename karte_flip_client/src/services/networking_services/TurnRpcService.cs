using System.Linq;
using System.Threading.Tasks;
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
		private async void StartMatch(Array<Dictionary> players){ // invoked by: server
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
			RouteManager.GetIntance().MoveToScene(SceneFileNameEnum.MAIN_SCENE, GetTree());
			await Task.Delay(100);
			_mainTilemap = GetNode<GridGroundTilemap>("/root/MainScene/GridGroundTilemap");
			_turnDisplayTextureRect = GetNode<TextureRect>("/root/MainScene/HUDTextureRect/TurnTextureRect");
		}

		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
		private void RecieveDataToSpecificClient(Vector2I position, Dictionary card){ // invoked by: server
			AddTokenToTilemap(position, card);	
		}

		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
		private void ProcessTurnDataOnServer(Vector2I position, Dictionary card){} // invoked by: local

		/*
		* ********************************************************
		*	LOGICS
		* ********************************************************
		*/
		private void AddTokenToTilemap(Vector2I position, Dictionary card){
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

			RouteManager route = RouteManager.GetIntance();

			if(_currentPlayerTurn.TokenColor == TokenColorEnum.DARK_TOKEN){
				_turnDisplayTextureRect.Texture = route.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.BLACK_TOKEN);
			} else {
				_turnDisplayTextureRect.Texture = route.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.WHITE_TOKEN);
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