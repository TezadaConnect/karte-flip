using System;
using System.Numerics;
using Godot;

class CardCollection{
    private string[] mCardListOfNames = {
        "Yellow Card",
        "Red Card",
        "Blue Card",
        "Orange Card",
        "Green Card",
    };

    private string[] mCardListOfDescriptions = {
        "Flip bottom tokens",
        "Flip top tokens",
        "Flip left tokens",
        "Flip righ tokens",
        "Flip cross tokens"
    };

    private CardAbilityEnum[] mCardListOfAbilityEnum = new CardAbilityEnum[] {
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
    };

    private DirectionEnum[][] mCardListOfFlipDirection = {
        new DirectionEnum[] { DirectionEnum.BOTTOM },
        new DirectionEnum[] { DirectionEnum.TOP },
        new DirectionEnum[] { DirectionEnum.LEFT },
        new DirectionEnum[] { DirectionEnum.RIGHT },
        new DirectionEnum[] { 
            DirectionEnum.BOTTOM, 
            DirectionEnum.LEFT, 
            DirectionEnum.TOP, 
            DirectionEnum.RIGHT
        },
    };

    private Vector2I[] mCardListOfTileImagesCoordinate = {
        new Vector2I(4, 0),
        new Vector2I(3, 1),
        new Vector2I(3, 0),
        new Vector2I(2, 1),
        new Vector2I(4, 1),
    };


    public CardCollection(){
        
    }

    public string[] GetCardListOfNames(){
        return mCardListOfNames;
    }


    public CardModel GetCardModelByIndex(int index){
        return new CardModel(
            mCardListOfNames[index], 
            mCardListOfDescriptions[index], 
            mCardListOfAbilityEnum[index], 
            mCardListOfFlipDirection[index],
            mCardListOfTileImagesCoordinate[index]
        );
    }
    
}