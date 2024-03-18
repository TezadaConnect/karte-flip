using System.Collections.Generic;
using System.Numerics;
using Godot;

class CardModel {
    private string mCardName;
    private string mCardDescription;
    private CardAbilityEnum mCardAbility;
    private List<DirectionEnum> mCardListFlipDirections;
    LocalAssetFileNameEnum mCardFileNameEnum;
    
    public CardModel(
        string cardName, 
        string cardDescription, 
        CardAbilityEnum cardAbility, 
        List<DirectionEnum> cardListFlipDirections,
        LocalAssetFileNameEnum cardFileNameEnum
    ){
        mCardName = cardName;
        mCardDescription = cardDescription;
        mCardListFlipDirections = cardListFlipDirections;
        mCardAbility = cardAbility;
        mCardFileNameEnum = cardFileNameEnum;
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

    public List<DirectionEnum> GetCardListFlipDirections(){
        return mCardListFlipDirections;
    }

    public LocalAssetFileNameEnum GetCardFileNameEnum(){
        return mCardFileNameEnum;
    }
}