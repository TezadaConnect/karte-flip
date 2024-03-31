using Godot.Collections;

public class CardModel{
    private string _cardName;
    private string _cardDescription;
    private CardAbilityEnum _cardAbility;
    private Array<DirectionEnum> _cardListFlipDirections;
    private LocalAssetFileNameEnum _cardFilenameEnum;

    // Getters and Setters
    public string CardName {
        get { return _cardName; }
        set { _cardName = value; }
    }
    public string CardDescription {
        get { return _cardDescription; }
        set { _cardDescription = value; }
    }
    public CardAbilityEnum CardAbility {
        get { return _cardAbility; }
        set { _cardAbility = value; }
    }
    public Array<DirectionEnum> CardListFlipDirections {
        get { return _cardListFlipDirections; }
        set { _cardListFlipDirections = value; }
    }
    public LocalAssetFileNameEnum CardFilename {
        get { return _cardFilenameEnum; }
        set { _cardFilenameEnum =  value; }
    }

    //Helper methods
    public Dictionary Serialize(){
        Dictionary serializeValue = new();
        serializeValue["card_name"] = _cardName;
        serializeValue["card_description"] = _cardDescription;
        serializeValue["card_ability"] = (int)_cardAbility;
        serializeValue["card_list_of_flip_direction"] = _cardListFlipDirections;
        serializeValue["card_filename_enum"] = (int)_cardFilenameEnum;
        return serializeValue;
    }
    public static CardModel Deserialize(Dictionary serializeValue){
        CardModel item = new(){
            CardName = (string)serializeValue["card_name"], 
            CardDescription = (string)serializeValue["card_description"], 
            CardAbility = (CardAbilityEnum)(int)serializeValue["card_ability"],
            CardListFlipDirections = (Array<DirectionEnum>)serializeValue["card_list_of_flip_direction"],
            CardFilename = (LocalAssetFileNameEnum)(int)serializeValue["card_filename_enum"]
        };
        return item;
    }
}