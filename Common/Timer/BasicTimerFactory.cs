namespace Sowalabs.Bison.Common.Timer
{
    public class BasicTimerFactory : ITimerFactory
    {
        public ITimer CreateTimer()
        {
            return new BasicTimer();
        }
    }
}
