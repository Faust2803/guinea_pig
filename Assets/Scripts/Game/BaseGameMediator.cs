namespace Game
{
    public class BaseGameMediator
    {
        // public  BaseMediator()
        // {
        //     ProjectContext.Instance.Container.Inject(this);
        // }
        
        protected object _data;
        public BaseGameView View { get; set; }
       
    }
}