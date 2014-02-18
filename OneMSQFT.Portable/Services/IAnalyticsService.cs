namespace OneMSQFT.Common.Services
{
    public interface IAnalyticsService
    {
        void Configure();
        void StartSession();
        void StopSession();
    }
}