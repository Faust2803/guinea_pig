namespace DataModels.CollectionsData
{
    public interface IHamsterVisualData
    {
        int Id { get; }
        public string SpriteName { get; }
        EquipItem[] Equipment { get; }
    }
}