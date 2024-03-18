using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

class GameTurnManager{
    // private TokenColorEnum mTurnType;
    private GamePlayTypeEnum mGamePlayTypeEnum;
    private PlayerModel mPlayerTurn;
    private CardModel[] mCardList;
    private CardModel mCurrentCard;

    private readonly Vector2I ATLAS_COORD_WHITE = new(1, 0);
    private readonly Vector2I ATLAS_COORD_BLACK = new(2, 0);
    public const int BOARD_TILE_COUNT = 49;

    private static GameTurnManager mGameTurnManagerInstance;

    public GameTurnManager(){
        // mTurnType = TokenColorEnum.LIGHT_TOKEN;
        CardCollection cardCollection = new CardCollection();
        List<CardModel> cards = new List<CardModel>();
        for (int i = 0; i < cardCollection.GetCardListOfNames().Count(); i++){
            cards.Add(cardCollection.GetCardModelByIndex(i));
        }
        mCardList = cards.ToArray();
        mCurrentCard = GetCurrentCard();
    }

    public static GameTurnManager GetInstance(){
        mGameTurnManagerInstance ??= new GameTurnManager();
        return mGameTurnManagerInstance;
    }

    public Vector2I GetTileForDisplay(){
        if(mPlayerTurn.IsLightToken()){
            return ATLAS_COORD_WHITE;
        }
        return ATLAS_COORD_BLACK;
    }

    private CardModel GetRandomCard(){
        return mCardList[new Random().Next(0, 5)];
    }

    public CardModel GetCurrentCard(){
        return mCurrentCard;
    }

    public void SetCurrentCardWithRandomCard(){
        mCurrentCard = GetRandomCard();
    }

    public Vector2I GetWhiteAtlas(){
        return ATLAS_COORD_WHITE;
    }

    public Vector2I GetBlackAtlas(){
        return ATLAS_COORD_BLACK;
    }

    public PlayerModel GetPlayerTurn(){
        return mPlayerTurn;
    }

    public void SetPlayerTurn(PlayerModel value){
        mPlayerTurn = value;
    }

    public void ResetTurn(){
        mCurrentCard = GetCurrentCard();
    }

    public GamePlayTypeEnum GetGamePlayType(){
        return mGamePlayTypeEnum;
    }

    public void SetGamePlayType(GamePlayTypeEnum value){
        mGamePlayTypeEnum = value;
    }
}