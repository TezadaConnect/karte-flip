using System;
using System.Collections.Generic;

class PlayerManager{
    private PlayerModel mPlayerOne;
	private PlayerModel mPlayerTwo;
    private static PlayerManager mPlayerManagerInstance;

    public PlayerManager(){
        PlayerSetup();
    }

    public static PlayerManager GetInstance(){
        mPlayerManagerInstance ??= new PlayerManager();
        return mPlayerManagerInstance;
    }

    private void PlayerSetup(){
        GameTurnManager turnManagerHolder = GameTurnManager.GetInstance();

		List<TokenColorEnum> allTokens = new List<TokenColorEnum>{ 
			TokenColorEnum.LIGHT_TOKEN, 
			TokenColorEnum.DARK_TOKEN 
		};

		TokenColorEnum randomToken = allTokens[new Random().Next(allTokens.Count)]; 

		// if(turnManagerHolder.GetGamePlayType() == GamePlayTypeEnum.VS_COMPUTER){
		// 	mPlayerOne = new PlayerModel(PlayerTypeEnum.PERSON, randomToken);
		// 	mPlayerTwo = new PlayerModel(
		// 		PlayerTypeEnum.COMPUTER, 
		// 		GetReverseTokenColor(randomToken)
		// 	);
		// }

		if(turnManagerHolder.GetGamePlayType() == GamePlayTypeEnum.VS_PERSON){
			//Put code here for player server 1v1 code
            return;
		}

		if(randomToken == TokenColorEnum.LIGHT_TOKEN){
			turnManagerHolder.SetPlayerTurn(mPlayerOne);
		} else {
			turnManagerHolder.SetPlayerTurn(mPlayerTwo);
		}
	}

    private TokenColorEnum GetReverseTokenColor(TokenColorEnum value){
		return value == TokenColorEnum.LIGHT_TOKEN ? TokenColorEnum.DARK_TOKEN : TokenColorEnum.LIGHT_TOKEN;
	}

    public void ResetPlayers(){
        PlayerSetup();
    }

    public PlayerModel GetPlayerOne(){
        return mPlayerOne;
    }

     public PlayerModel GetPlayerTwo(){
        return mPlayerTwo;
    }
}