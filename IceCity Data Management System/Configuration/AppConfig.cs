
namespace IceCity_Data_Management_System.Configuration
{
    public sealed class AppConfig
    {
        private static AppConfig? _instance;
        private static readonly Lock _lock = new();

        public decimal CriticalTemperature { get; private set; }
        public string LogsFolder { get; private set; }

        private AppConfig(IConfiguration configuration)
        {
            CriticalTemperature = configuration.GetValue<decimal>("IceCitySettings:CriticalTemperature");
            LogsFolder = configuration.GetValue<string>("IceCitySettings:LogsFolder") ?? "Logs";
        }

        public static AppConfig GetInstance(IConfiguration configuration)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new AppConfig(configuration);
                }
            }
            return _instance;
        }
    }
}
