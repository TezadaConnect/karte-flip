class CardModel {
    private string mCardName;
    private string mCardDescription;
    private CardAbilityEnum mCardAbility;
    private DirectionEnum[] mCardListFlipDirections;

    public CardModel(string cardName, string cardDescription, CardAbilityEnum cardAbility, DirectionEnum[] cardListFlipDirections){
        mCardName = cardName;
        mCardDescription = cardDescription;
        mCardListFlipDirections = cardListFlipDirections;
        mCardAbility = cardAbility;
    }

    public string GetName(){
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
}