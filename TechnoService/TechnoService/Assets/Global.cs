using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoService.Assets
{
    public static class Global
    {
        public static ObservableCollection<CatalogOrder>? Notifications { get; set; } = new ObservableCollection<CatalogOrder>();
    }
}
