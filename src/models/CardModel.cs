using System.Numerics;
using Godot;

class CardModel {
    private string mCardName;
    private string mCardDescription;
    private CardAbilityEnum mCardAbility;
    private DirectionEnum[] mCardListFlipDirections;
    private Vector2I mCardTileImageCoordinates;

    public CardModel(
        string cardName, 
        string cardDescription, 
        CardAbilityEnum cardAbility, 
        DirectionEnum[] cardListFlipDirections,
        Vector2I cardTileImageCoordinates
    ){
        mCardName = cardName;
        mCardDescription = cardDescription;
        mCardListFlipDirections = cardListFlipDirections;
        mCardAbility = cardAbility;
        mCardTileImageCoordinates = cardTileImageCoordinates;
    }

    public string GetCardName(){
        return mCardName;
    }

    public string GetCardDiscription(){
        return mCardDescription;
    }

    public CardAbilityEnum GetCardAbility(){
        return mCardAbility;
    }

    public DirectionEnum[] GetCardListFlipDirections(){
        return mCardListFlipDirections;
    }

    public Vector2I GetCardTileImageCoordinate(){
        return mCardTileImageCoordinates;
    }
}