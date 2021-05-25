using Application.Common.Interfaces;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class TimerService : ITimer
    {
        private readonly Stopwatch _timer;

        public TimerService()
        {
            this._timer = new Stopwatch();
        }

        public void Start()
        {
            this._timer.Start();
        }

        public long Stop()
        {
            this._timer.Stop();

            return this._timer.ElapsedMilliseconds;
        }
    }
}
