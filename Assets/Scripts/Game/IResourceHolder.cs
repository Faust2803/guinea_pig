namespace Game.Jumper
{
    public interface IResourceHolder
    {
        int PeansCurrency { get; }
        int CornsCurrency { get; }
        int SeedsCurrency { get; }

        int BeansCurrency { get; internal set; }
        int GoldenBeansCurrency { get; internal set; }
    }
}