using System.Configuration;
using System.Data;
using System.Windows;
using TechnoService.Data.Models;

namespace TechnoService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			ViewModelLocator.Init();
			base.OnStartup(e);
		}
	}

}
