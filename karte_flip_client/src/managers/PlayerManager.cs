using System;
using System.Collections.Generic;
using Godot;
using KarteFlipClient;

public partial class PlayerManager : Node2D {
	private PlayerModel _currentPlayerTurn;
    private PlayerModel _playerOne;
	private PlayerModel _playerTwo;

	private readonly List<TokenColorEnum> _allTokens = new List<TokenColorEnum>{ 
		TokenColorEnum.LIGHT_TOKEN, 
		TokenColorEnum.DARK_TOKEN 
	};

	public PlayerModel CurrentPlayerTurn {
		get { return _currentPlayerTurn; }
		set { _currentPlayerTurn = value; }
	}

	public PlayerModel PlayerOne {
		get { return _playerOne; }
		set { _playerOne = value; }
	}

	public PlayerModel PlayerTwo{
		get { return _playerTwo; }
		set { _playerTwo = value; }
	}

	public void InitComputerVsPlayer(){
		
		TokenColorEnum randomToken = _allTokens[new Random().Next(_allTokens.Count)];

		TokenColorEnum reverseOfRandomToken = 
			randomToken == TokenColorEnum.DARK_TOKEN ? 
			TokenColorEnum.LIGHT_TOKEN : 
			TokenColorEnum.DARK_TOKEN;

		_playerOne = new PlayerModel(){
			PlayerID = 1, 
			PlayerType = PlayerTypeEnum.PERSON, 
			TokenColor = randomToken
		};

		_playerTwo = new PlayerModel(){
			PlayerID = 2, 
			PlayerType = PlayerTypeEnum.COMPUTER, 
			TokenColor = reverseOfRandomToken
		};

		_currentPlayerTurn = _playerOne.TokenColor == TokenColorEnum.LIGHT_TOKEN ? _playerOne : _playerTwo;
	}
}