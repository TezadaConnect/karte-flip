class GameTurnManager{
    private GameTurnEnum mTurnType;

    private GameTurnManager mGameTurnManagerInstance;

    public GameTurnManager(){
        mTurnType = GameTurnEnum.LIGHT_TURN;
    }

    public GameTurnManager getInstance(){
        if(mGameTurnManagerInstance == null){
            mGameTurnManagerInstance = new GameTurnManager();
        }
        return mGameTurnManagerInstance;
    }
}