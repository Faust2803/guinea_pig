namespace Game.Character
{
    public enum CharacterType
    {
        InGameCharacter,
        Enemy1,
        Enemy2,
        Enemy3,
        LobbyPlayerCharacter
    }
    
    public enum CharacterStateType
    {
        Idle,
        Run,
        TakeAim,
        Fire,
        Hit,
        Death
    }
}