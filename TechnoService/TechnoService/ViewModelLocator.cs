using TechnoService.Data;

namespace TechnoService
{
    internal class ViewModelLocator
    {
        private static ServiceProvider? _provider;
        private static IConfiguration? _configuration;
        public static void Init()
        {
            _configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

            var services = new ServiceCollection();

            #region ViewModel

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<LoginViewModel>();
			services.AddTransient<DispatcherPanelViewModel>();
			services.AddTransient<EmployeePanelViewModel>();
            services.AddTransient<ManagerPanelViewModel>();


            #endregion

            #region Connection

            services.AddDbContext<DemoContext>(options =>
            {
                var conn = _configuration.GetConnectionString("LocalConnection");
                options.UseMySql(conn, ServerVersion.AutoDetect(conn));

            }, ServiceLifetime.Singleton);

            #endregion

            #region Services

            services.AddSingleton<PageService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<OrderService>();



            #endregion

            _provider = services.BuildServiceProvider();
            
        }
        public MainWindowViewModel? MainWindowViewModel => _provider?.GetRequiredService<MainWindowViewModel>();
        public LoginViewModel? LoginViewModel => _provider?.GetRequiredService<LoginViewModel>();
		public DispatcherPanelViewModel? DispatcherPanelViewModel => _provider?.GetRequiredService<DispatcherPanelViewModel>();
		public EmployeePanelViewModel? EmployeePanelViewModel => _provider?.GetRequiredService<EmployeePanelViewModel>();
        public ManagerPanelViewModel? ManagerPanelViewModel => _provider?.GetRequiredService<ManagerPanelViewModel>();
    }
}
