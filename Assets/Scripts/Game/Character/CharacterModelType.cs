namespace Game.Character
{
    public enum CharacterModelType
    {
        InGameCharacter,
        Enemy1,
        Enemy2,
        Enemy3,
        LobbyPlayerCharacter
    }
    
    public enum CharacterType
    {
        Player,
        Enemy
    }
    
    public enum CharacterStateType
    {
        Idle,
        Run,
        TakeAim,
        Fire,
        FireCompleated,
        Hit,
        Death,
        Victory,
        Reload
    }
}