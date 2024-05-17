using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoService.Services;

namespace TechnoService.ViewModels
{
	public class EmployeePanelViewModel : BindableBase
	{
		private readonly PageService _pageService;
		private readonly UserService _userService;
        private readonly OrderService _orderService;
        public ObservableCollection<CatalogOrder> Orders { get; set; }
        public ObservableCollection<TypeEquipment> TypeEquipment { get; set; }
        public ObservableCollection<TypeProblem> TypeProblem { get; set; }
        public ObservableCollection<string> WorkStatus { get; set; } = new ObservableCollection<string> { "Выполнено", "В работе", "Не выполнено" };

        public EmployeePanelViewModel(UserService userService, PageService pageService, OrderService orderService)
		{
			_pageService = pageService;
			_userService = userService;
            _orderService = orderService;
            Task.Run(async () =>
            {
				var typeProblem = await _orderService.GetTypeProblemsAsync();
				TypeProblem = new ObservableCollection<TypeProblem>(typeProblem);
                var typeEquipment = await _orderService.GetTypeEquipmentAsync();
                TypeEquipment = new ObservableCollection<TypeEquipment>(typeEquipment);
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
            var orders = await _orderService.GetOrdersEmployeeAsync(UserSetting.Default.IdUser);
            Orders = new ObservableCollection<CatalogOrder>(
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
        public string Status { get; set; }
        public string? EditComment { get; set; }
        public string? EditStatus { get; set; }
        public string? EditNameClient { get; set; }
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
            Status = SelectedOrder.IdOrdersNavigation.WorkStatus;
            EditDateAddition = SelectedOrder.IdOrdersNavigation.DateAddition as DateTime?;
            EditNameEquipment = SelectedOrder.IdOrdersNavigation.NameEquipment;
            EditIdTypeProblem = SelectedOrder.IdOrdersNavigation.IdTypeProblemNavigation != null
                ? TypeProblem.FirstOrDefault(tp => tp.IdTypeProblem == SelectedOrder.IdOrdersNavigation.IdTypeProblemNavigation.IdTypeProblem) : TypeProblem[0];
            EditComment = SelectedOrder.IdOrdersNavigation.CommentApplication;
            EditCost = SelectedOrder.IdOrdersNavigation.Cost != null ? (decimal)SelectedOrder.IdOrdersNavigation.Cost : 0;
            EditDateEnd = SelectedOrder.IdOrdersNavigation.DateEnd as DateTime?;
            EditTimeWork = SelectedOrder.IdOrdersNavigation.TimeWork.HasValue
                    ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, SelectedOrder.IdOrdersNavigation.TimeWork.Value.Hour, SelectedOrder.IdOrdersNavigation.TimeWork.Value.Minute, SelectedOrder.IdOrdersNavigation.TimeWork.Value.Second)
                    : (DateTime?)null;
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
                var item = Orders.First(i => i.IdOrdersNavigation.IdApplication == SelectedOrder.IdOrdersNavigation.IdApplication);
                var index = Orders.IndexOf(item);
                item.IdOrdersNavigation.CommentApplication = EditComment;
                item.IdOrdersNavigation.Cost = EditCost;
                item.IdOrdersNavigation.TimeWork = EditTimeWork.HasValue
                        ? new TimeOnly(EditTimeWork.Value.Hour, EditTimeWork.Value.Minute, EditTimeWork.Value.Second)
                        : (TimeOnly?)null;
                item.IdOrdersNavigation.WorkStatus = EditWorkStatus;
                if (item.IdOrdersNavigation.WorkStatus == "Выполнено") item.IdOrdersNavigation.DateEnd = DateTime.Now;
                Orders.RemoveAt(index);
                Orders.Insert(index, item);
                if (item.IdOrdersNavigation.WorkStatus != Status)
                {
                    Global.Notifications.Add(item);
                }
                await _orderService.UpdateCatalogOrderAsync(item);
                IsDialogEditOrderOpen = false;
            }
        });
        public DelegateCommand LogoutCommand => new(() =>
        {
            UserSetting.Default.Reset();
            _pageService.ChangePage(new Login());
        });
    }

}
