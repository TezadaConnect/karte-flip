using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

class GameTurnManager{
    private GameTurnEnum mTurnType;
    private CardModel[] mCardList;
    private CardModel mCurrentCard;
    private bool mIsPlayerTurn;
    private readonly Vector2I ATLAS_COORD_WHITE = new Vector2I(1, 0);
    private readonly Vector2I ATLAS_COORD_BLACK = new Vector2I(2, 0);
    private const int BOARD_TILE_COUNT = 49;

    private static GameTurnManager mGameTurnManagerInstance;

    public GameTurnManager(){
        mTurnType = GameTurnEnum.LIGHT_TURN;
        CardCollection cardCollection = new CardCollection();
        List<CardModel> cards = new List<CardModel>();
        for (int i = 0; i < cardCollection.GetCardListOfNames().Count(); i++){
            cards.Add(cardCollection.GetCardModelByIndex(i));
        }
        mCardList = cards.ToArray();
        mCurrentCard = GetCurrentCard();
        mIsPlayerTurn = true;
    }

    public static GameTurnManager GetInstance(){
        if(mGameTurnManagerInstance == null){
            mGameTurnManagerInstance = new GameTurnManager();
        }
        return mGameTurnManagerInstance;
    }

    public void SetTurnType(GameTurnEnum turnType){
        mTurnType = turnType;
    }

    public GameTurnEnum GetTurnType(){
        return mTurnType;
    }

    public Vector2I GetTileForDisplay(){
        if(mTurnType == GameTurnEnum.LIGHT_TURN){
            return ATLAS_COORD_WHITE;
        }
        return ATLAS_COORD_BLACK;
    }

    private CardModel GetRandomCard(){
        Random random = new Random();
        return mCardList[random.Next(0, 5)];
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

    public bool GetIsPlayerTurn(){
        return mIsPlayerTurn;
    }

    public void SetIsPlayerTurn(bool value){
        mIsPlayerTurn = value;
    }
}