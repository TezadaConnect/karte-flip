using Godot;
using Godot.Collections;

class CardCollection{
    private readonly string[] _cardListOfNames = {
        "Yellow Card",
        "Red Card",
        "Blue Card",
        "Orange Card",
        "Green Card",
    };

    private readonly string[] _cardListOfDescriptions = {
        "Flip bottom tokens",
        "Flip top tokens",
        "Flip left tokens",
        "Flip right tokens",
        "Flip cross tokens"
    };

    private readonly CardAbilityEnum[] _cardListOfAbilityEnum = new CardAbilityEnum[] {
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
        CardAbilityEnum.FLIP,
    };

    private readonly Array<Array<DirectionEnum>> _cardListOfFlipDirection = new(){
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

    private readonly LocalAssetFileNameEnum[] _listOfLocalAssetFileNameEnum = new LocalAssetFileNameEnum[] {
        LocalAssetFileNameEnum.YELLOW_CARD,
        LocalAssetFileNameEnum.RED_CARD,
        LocalAssetFileNameEnum.BLUE_CARD,
        LocalAssetFileNameEnum.ORANGE_CARD,
        LocalAssetFileNameEnum.GREEN_CARD
    };

    public string[] GetCardListOfNames(){
        return _cardListOfNames;
    }

    public CardModel GetCardModelByIndex(int index){

        CardModel item = new(){
            CardName = _cardListOfNames[index], 
            CardDescription = _cardListOfDescriptions[index], 
            CardAbility = _cardListOfAbilityEnum[index], 
            CardListFlipDirections = _cardListOfFlipDirection[index],
            CardFilename = _listOfLocalAssetFileNameEnum[index]
        };
        GD.Print(item.Serialize());
        return item;
    }
}