using System.Collections.Generic;
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
        "Flip right tokens",
        "Flip cross tokens"
    };

    private CardAbilityEnum[] mCardListOfAbilityEnum = new CardAbilityEnum[] {
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
    };

    private List<List<DirectionEnum>> mCardListOfFlipDirection = new(){
        new List<DirectionEnum> { DirectionEnum.BOTTOM },
        new List<DirectionEnum> { DirectionEnum.TOP },
        new List<DirectionEnum>{ DirectionEnum.LEFT },
        new List<DirectionEnum> { DirectionEnum.RIGHT },
        new List<DirectionEnum> { 
            DirectionEnum.BOTTOM, 
            DirectionEnum.LEFT, 
            DirectionEnum.TOP, 
            DirectionEnum.RIGHT
        },
    };

    private LocalAssetFileNameEnum[] mListOfLocalAssetFileNameEnum = new LocalAssetFileNameEnum[] {
        LocalAssetFileNameEnum.YELLOW_CARD,
        LocalAssetFileNameEnum.RED_CARD,
        LocalAssetFileNameEnum.BLUE_CARD,
        LocalAssetFileNameEnum.ORANGE_CARD,
        LocalAssetFileNameEnum.GREEN_CARD
    };

    public string[] GetCardListOfNames(){
        return mCardListOfNames;
    }

    public CardModel GetCardModelByIndex(int index){
        return new CardModel(
            mCardListOfNames[index], 
            mCardListOfDescriptions[index], 
            mCardListOfAbilityEnum[index], 
            mCardListOfFlipDirection[index],
            mListOfLocalAssetFileNameEnum[index]
        );
    }
    
}