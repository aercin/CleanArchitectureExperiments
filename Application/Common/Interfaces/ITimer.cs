namespace Application.Common.Interfaces
{
    public interface ITimer
    { 
        public void Start();

        /// <returns>returns ElapsedMilliseconds</returns>
        public long Stop();
    }
}
