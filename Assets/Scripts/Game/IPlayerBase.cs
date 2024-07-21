namespace Game.Jumper
{
    public interface IPlayerBase : IResourceHolder
    {
        int OwnerId { get; }
        int UniqueID { get; }
        string Nickname { get; }
        bool IsDead { get; }
        bool IsFellout { get; }

        void GameEndRpc(int winnerID);
        void SetGameEndedClientRpc(int winnerID);
    }
}