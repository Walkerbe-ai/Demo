using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoService.ViewModels
{
    public class ManagerPanelViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly UserService _userService;
        private readonly OrderService _orderService;
        public ObservableCollection<CatalogOrder> CatalogOrder { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<TypeEquipment> TypeEquipment { get; set; }
        public ObservableCollection<Human> Employee { get; set; }
        public ObservableCollection<TypeProblem> TypeProblem { get; set; }
        public ObservableCollection<string> WorkStatus { get; set; } = new ObservableCollection<string> { "Выполнено", "В работе", "Не выполнено" };
        public ObservableCollection<string> Status { get; set; } = new ObservableCollection<string> { "Выполнено", "В работе", "В ожидании" };
        public ManagerPanelViewModel(UserService userService, PageService pageService, OrderService orderService)
        {
            _pageService = pageService;
            _userService = userService;
            _orderService = orderService;
            Task.Run(async () =>
            {
                var emp = await _userService.GetEmployeeAsync();
                Employee = new ObservableCollection<Human>(emp);
                var typeProblem = await _orderService.GetTypeProblemsAsync();
                TypeProblem = new ObservableCollection<TypeProblem>(typeProblem);
                var typeEquipment = await _orderService.GetTypeEquipmentAsync();
                TypeEquipment = new ObservableCollection<TypeEquipment>(typeEquipment);
                var orders = await _orderService.GetOrdersManagerAsync();
                Orders = new ObservableCollection<Order>(orders);
                SearchOrder();
            });
        }
        public string SearchOrd
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: SearchOrder); }
        }
        public async void SearchOrder()
        {
            var orders = await _orderService.GetCatalogOrdersManagerAsync();
            CatalogOrder = new ObservableCollection<CatalogOrder>(
                string.IsNullOrWhiteSpace(SearchOrd)
                    ? orders
                    : orders.Where(p => p.IdOrdersNavigation.DescriptionApplication.Contains(SearchOrd) || p.IdOrdersNavigation.SerialNumber.ToString().Contains(SearchOrd))
            );
        }
        public CatalogOrder SelectedOrder { get; set; }

        // Редактирование
        public bool IsDialogEditOrderOpen { get; set; } = false;
        public DateTime? EditDateAddition { get; set; }
        public string? EditNameEquipment { get; set; }
        public TypeProblem? EditIdTypeProblem { get; set; }
        public string? EditComment { get; set; }
        public string? EditStatus { get; set; }
        public string? EditNameClient { get; set; }
        public Human? EditIdEmployee { get; set; }
        public decimal EditCost { get; set; }
        public DateTime? EditDateEnd { get; set; }
        public DateTime? EditTimeWork { get; set; }
        public string? EditWorkStatus { get; set; }
        public DateTime? EditPeriodExecution { get; set; }
        public TypeEquipment? EditIdTypeEquipment { get; set; }
        public int EditNumber { get; set; }
        public string? EditDescription { get; set; }
        public DelegateCommand EditOrderCommand => new(() =>
        {
            if (SelectedOrder == null)
                return;

            EditDateAddition = SelectedOrder.IdOrdersNavigation.DateAddition as DateTime?;
            EditNameEquipment = SelectedOrder.IdOrdersNavigation.NameEquipment;
            EditIdTypeProblem = SelectedOrder.IdOrdersNavigation.IdTypeProblemNavigation != null
                ? TypeProblem.FirstOrDefault(tp => tp.IdTypeProblem == SelectedOrder.IdOrdersNavigation.IdTypeProblemNavigation.IdTypeProblem) : TypeProblem[0];
            EditComment = SelectedOrder.IdOrdersNavigation.CommentApplication;
            EditStatus = SelectedOrder.IdOrdersNavigation.Status;
            EditNameClient = SelectedOrder.IdOrdersNavigation.NameClient;
            EditCost = SelectedOrder.IdOrdersNavigation.Cost != null ? (decimal)SelectedOrder.IdOrdersNavigation.Cost : 0;
            EditDateEnd = SelectedOrder.IdOrdersNavigation.DateEnd as DateTime?;
            EditTimeWork = SelectedOrder.IdOrdersNavigation.TimeWork.HasValue ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, SelectedOrder.IdOrdersNavigation.TimeWork.Value.Hour, SelectedOrder.IdOrdersNavigation.TimeWork.Value.Minute, SelectedOrder.IdOrdersNavigation.TimeWork.Value.Second) : (DateTime?)null;
            EditIdEmployee = SelectedOrder != null ? Employee.FirstOrDefault(tp => tp.IdUser == SelectedOrder.IdEmployee) : Employee[0];
            EditWorkStatus = SelectedOrder.IdOrdersNavigation.WorkStatus;
            EditPeriodExecution = SelectedOrder.IdOrdersNavigation.PeriodExecution as DateTime?;
            EditIdTypeEquipment = SelectedOrder.IdOrdersNavigation.IdTypeEquipmentNavigation != null ? TypeEquipment.FirstOrDefault(tp => tp.IdTypeEquipment == SelectedOrder.IdOrdersNavigation.IdTypeEquipmentNavigation.IdTypeEquipment) : TypeEquipment[0];
            EditNumber = SelectedOrder.IdOrdersNavigation.SerialNumber != null ? (int)SelectedOrder.IdOrdersNavigation.SerialNumber : 0;
            EditDescription = SelectedOrder.IdOrdersNavigation.DescriptionApplication;
            IsDialogEditOrderOpen = true;
        });
        public AsyncCommand SaveCurrentOrderCommand => new(async () =>
        {
            if (SelectedOrder != null)
            {
                var item = CatalogOrder.First(i => i.IdOrdersNavigation.IdApplication == SelectedOrder.IdOrdersNavigation.IdApplication);
                var index = CatalogOrder.IndexOf(item);
                //item.IdEmployee = EditIdEmployee.IdUser;
                item.IdOrdersNavigation.PeriodExecution = EditPeriodExecution as DateTime?;
                CatalogOrder.RemoveAt(index);
                CatalogOrder.Insert(index, item);
                await _orderService.UpdateCatalogOrderAsync(item);
                IsDialogEditOrderOpen = false;
            }
        });
        public DelegateCommand LogoutCommand => new(() =>
        {
            UserSetting.Default.Reset();
            _pageService.ChangePage(new Login());
        });
        public bool IsDialogAppointEmployeeOrderOpen { get; set; } = false;
        public Order? SelectOrder { get; set; }
        public Human? SelectedEmployee { get; set; }
        public DelegateCommand AppointEmployeeOrderCommand => new(() =>
        {
            SelectOrder = Orders[0];
            SelectedEmployee = Employee[0];
            IsDialogAppointEmployeeOrderOpen = true;
        });
        public AsyncCommand AddCatalogOrderCommand => new(async () =>
        {
            await _orderService.AddNewCatalogOrder(SelectOrder.IdApplication, SelectedEmployee.IdUser);
            IsDialogAppointEmployeeOrderOpen = false;
            var orders = await _orderService.GetCatalogOrdersManagerAsync();
            CatalogOrder = new ObservableCollection<CatalogOrder>(orders);
        });
    }
}
