using System;

class CardCollection{
    private string[] mCardListOfNames = {
        "Yellow Card",
        "Red Card",
        "Blue Card",
        "Orange Card",
        "Green Card",
    };

    private string[] mCardListOfDescriptions = {
        "Flip one chosen side of the card",
        "Flip vertical domain",
        "Freezes one chosen side of the card for 1 turn, preventing the card from Flipping",
        "Flip all the tokens surrounding it",
        "Flip horizontal domain"
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
            mCardListOfFlipDirection[index]
        );
    }
    
}