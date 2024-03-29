using System;
using Godot;
using Godot.Collections;

public class CardModel{
    private string mCardName;
    private string mCardDescription;
    private CardAbilityEnum mCardAbility;
    private Array<DirectionEnum> mCardListFlipDirections;
    private LocalAssetFileNameEnum mCardFilenameEnum;
    
    public CardModel(
        string cardName, 
        string cardDescription, 
        CardAbilityEnum cardAbility, 
        Array<DirectionEnum> cardListFlipDirections,
        LocalAssetFileNameEnum cardFileNameEnum
    ){
        mCardName = cardName;
        mCardDescription = cardDescription;
        mCardListFlipDirections = cardListFlipDirections;
        mCardAbility = cardAbility;
        mCardFilenameEnum = cardFileNameEnum;
    }

    public Dictionary Serialize(){
        Dictionary serializeValue = new();
        serializeValue["card_name"] = mCardName;
        serializeValue["card_description"] = mCardDescription;
        serializeValue["card_ability"] = (int)mCardAbility;
        serializeValue["card_list_of_flip_direction"] = mCardListFlipDirections;
        serializeValue["card_filename_enum"] = (int)mCardFilenameEnum;
        return serializeValue;
    }

    public static CardModel Deserialize(Dictionary serializeValue){
        return new CardModel(
            (string)serializeValue["card_name"], 
            (string)serializeValue["card_description"], 
            (CardAbilityEnum)(int)serializeValue["card_ability"],
            (Array<DirectionEnum>)serializeValue["card_list_of_flip_direction"],
            (LocalAssetFileNameEnum)(int)serializeValue["card_filename_enum"]
        );
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

    public Array<DirectionEnum> GetCardListFlipDirections(){
        return mCardListFlipDirections;
    }

    public LocalAssetFileNameEnum GetCardFilenameEnum(){
        return mCardFilenameEnum;
    }
}