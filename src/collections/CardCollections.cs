using Godot.Collections;

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

    private Array<Array<DirectionEnum>> mCardListOfFlipDirection = new(){
        new Array<DirectionEnum> { DirectionEnum.BOTTOM },
        new Array<DirectionEnum> { DirectionEnum.TOP },
        new Array<DirectionEnum> { DirectionEnum.LEFT },
        new Array<DirectionEnum> { DirectionEnum.RIGHT },
        new Array<DirectionEnum> { 
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