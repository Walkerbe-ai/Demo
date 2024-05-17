using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoService.Data.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using TechnoService.Models;
using DevExpress.Mvvm;
using System.Windows.Automation;
namespace TechnoService.ViewModels
{
	public class DispatcherPanelViewModel : BindableBase
	{
		private readonly PageService _pageService;
		private readonly UserService _userService;
		private readonly OrderService _orderService;
		public ObservableCollection<Human> Employee { get; set; }
		public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<CatalogOrder> CatalogOrder { get; set; }
        public ObservableCollection<CatalogOrder> Notification { get; set; }
        public ObservableCollection<TypeProblem> TypeProblem { get; set; }
        public ObservableCollection<CountTypeProblem> CountTypeProblem { get; set; } = new ObservableCollection<CountTypeProblem>();
        public ObservableCollection<TypeEquipment> TypeEquipment { get; set; }
        public ObservableCollection<string> WorkStatus { get; set; } = new ObservableCollection<string> { "Выполнено", "В работе", "Не выполнено" };
        public ObservableCollection<string> Status { get; set; } = new ObservableCollection<string> { "Выполнено", "В работе", "В ожидании" };
        private List<string> _userLogin { get; set; } = new();
		public string SearchEmp
		{
			get { return GetValue<string>(); }
			set { SetValue(value, changedCallback: SearchEmployee); }
		}
		public string SearchOrd
		{
			get { return GetValue<string>(); }
			set { SetValue(value, changedCallback: SearchOrder); }
		}

		public DispatcherPanelViewModel(UserService userService, PageService pageService, OrderService orderService)
		{
			_pageService = pageService;
			_userService = userService;
			_orderService = orderService;
			Task.Run(async () =>
			{
				_userLogin = await _userService.GetAllEmployeeLogin();
				var typeProblem = await _orderService.GetTypeProblemsAsync();
				TypeProblem = new ObservableCollection<TypeProblem>(typeProblem);
                var typeEquipment = await _orderService.GetTypeEquipmentAsync();
                TypeEquipment = new ObservableCollection<TypeEquipment>(typeEquipment);
                var orders = await _orderService.GetOrdersAsync();
                Orders = new ObservableCollection<Order>(orders);
                SearchEmployee();
				SearchOrder();
                Notification = new ObservableCollection<CatalogOrder>(Global.Notifications.Distinct());

			});
        }
        
        public async void SearchEmployee()
		{
			var emp = await _userService.GetEmployeeAsync();
			Employee = new ObservableCollection<Human>(
				string.IsNullOrWhiteSpace(SearchEmp)
				? emp
				: emp.Where(p => p.FullnameUser.Contains(SearchEmp)).ToList());
			
		}
		public async void SearchOrder()
		{
			var orders = await _orderService.GetCatalogOrdersAsync();
			CatalogOrder = new ObservableCollection<CatalogOrder>(
				string.IsNullOrWhiteSpace(SearchOrd)
					? orders
					: orders.Where(p => p.IdOrdersNavigation.DescriptionApplication.Contains(SearchOrd) || p.IdOrdersNavigation.SerialNumber.ToString().Contains(SearchOrd))
			);
		}


		#region Employees

		public bool IsDialogAddEmployeesOpen { get; set; } = false;
		public string AddEmployeeFullName { get; set; }
		public string AddEmployeeLogin { get; set; }
		public string AddEmployeePassword { get; set; }

		public AsyncCommand AddCurrentEmployeeCommand => new(async () =>
		{
			await _userService.AddNewEmployee(AddEmployeeFullName, AddEmployeeLogin, AddEmployeePassword);
			var emp = await _userService.GetEmployeeAsync();
			Employee = new ObservableCollection<Human>(emp);
			IsDialogAddEmployeesOpen = false;
		});
		public DelegateCommand CreateEmployeeCommand => new(() =>
		{
			IsDialogAddEmployeesOpen = true;
		});
		#endregion

		#region Orders

		public bool IsDialogAddOrdersOpen { get; set; } = false;
		public string AddNameEquipment { get; set; }
		public TypeProblem AddIdTypeProblem { get; set; }
		public string AddNameClient { get; set; }
		public DateTime AddPeriodExecution { get; set; } = DateTime.Now;
		public TypeEquipment AddIdTypeEquipment { get; set; }
		public int AddNumber { get;set; }
		public string AddDescription { get; set; }
		public AsyncCommand AddOrderCommand => new(async () =>
		{
			await _orderService.AddNewOrder(AddNameEquipment, AddIdTypeProblem.IdTypeProblem, AddNameClient, AddPeriodExecution, AddIdTypeEquipment.IdTypeEquipment, AddNumber, AddDescription);
			var orders = await _orderService.GetOrdersAsync();
			Orders = new ObservableCollection<Order>(orders);
			IsDialogAddOrdersOpen = false;
		});
		public DelegateCommand CreateOrdersCommand => new(() =>
		{
            IsDialogAddOrdersOpen = true;
		});

		public CatalogOrder SelectedOrder { get; set; }

		// Редактирование
		public bool IsDialogEditOrderOpen { get; set; } = false;
        public DateTime? EditDateAddition { get; set; }
        public string? EditNameEquipment { get; set; }
		public TypeProblem? EditIdTypeProblem { get; set; } 
        public string? EditComment { get; set; }
        public string? EditStatus { get; set; }
        public string? EditNameClient { get; set; }
        public decimal EditCost { get; set; }
		public DateTime? EditDateEnd { get; set; }
        public DateTime? EditTimeWork { get; set; }
		public Human? EditIdEmployee { get; set; }
        public string? EditWorkStatus { get; set; }
        public DateTime? EditPeriodExecution { get; set; }
		public TypeEquipment? EditIdTypeEquipment { get; set; }
		public int EditNumber { get; set; }
		public string? EditDescription { get; set; }

		public DelegateCommand EditOrderCommand => new(async() =>
		{
			if (SelectedOrder == null)
				return;
			SearchEmp = string.Empty;
            var emp = await _userService.GetEmployeeAsync();
			Employee = new ObservableCollection<Human>(emp);
            EditDateAddition = SelectedOrder.IdOrdersNavigation.DateAddition as DateTime?;
			EditNameEquipment = SelectedOrder.IdOrdersNavigation.NameEquipment;
			EditIdTypeProblem = SelectedOrder.IdOrdersNavigation.IdTypeProblemNavigation != null
				? TypeProblem.FirstOrDefault(tp => tp.IdTypeProblem == SelectedOrder.IdOrdersNavigation.IdTypeProblemNavigation.IdTypeProblem): TypeProblem[0];
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
                item.IdOrdersNavigation.DateAddition = EditDateAddition;
                item.IdOrdersNavigation.NameEquipment = EditNameEquipment;
                item.IdOrdersNavigation.IdTypeProblem = EditIdTypeProblem.IdTypeProblem;
                item.IdOrdersNavigation.Status = EditStatus;
                item.IdOrdersNavigation.NameClient = EditNameClient;
				item.IdEmployee = EditIdEmployee.IdUser;
				item.IdOrdersNavigation.PeriodExecution = EditPeriodExecution as DateTime?;
                item.IdOrdersNavigation.IdTypeEquipment = EditIdTypeEquipment.IdTypeEquipment;
                item.IdOrdersNavigation.SerialNumber = EditNumber;
                item.IdOrdersNavigation.DescriptionApplication = EditDescription;
                CatalogOrder.RemoveAt(index);
                CatalogOrder.Insert(index, item);
                await _orderService.UpdateCatalogOrderAsync(item);
                IsDialogEditOrderOpen = false;
            }
        });
		#endregion

		public bool IsDialogStatisticsOpen { get; set; } = false;
        public int NumberCompletedApplications { get; set; }
		public TimeOnly AverageTimeCompleteApplication { get; set; }



        public DelegateCommand StatisticsCommand => new(() =>
		{
			CountTypeProblem.Clear();
            NumberCompletedApplications = Orders.Where(x => x.Status == "Выполнено").Count();
            double totalSeconds = Orders.Where(order => order.TimeWork.HasValue && order.Status == "Выполнено").Sum(order => order.TimeWork.Value.ToTimeSpan().TotalSeconds);
            int count = Orders.Count(order => order.TimeWork.HasValue);
            double averageSeconds = totalSeconds / count;
            TimeSpan averageTimeSpan = TimeSpan.FromSeconds(averageSeconds);
            AverageTimeCompleteApplication = TimeOnly.FromTimeSpan(averageTimeSpan);
			foreach (var typeProblem in TypeProblem)
			{
				var countTypeProblem = new CountTypeProblem
				{
					IdTypeProblem = typeProblem.IdTypeProblem,
					NameTypeProblem = typeProblem.NameTypeProblem,
					Count = Orders.Where(o => o.IdTypeProblem == typeProblem.IdTypeProblem).Count()
                };
                CountTypeProblem.Add(countTypeProblem);
            }
            IsDialogStatisticsOpen = true;
        });
        public DelegateCommand LogoutCommand => new(() =>
        {
            UserSetting.Default.Reset();
            _pageService.ChangePage(new Login());
        });
        public bool IsDialogShowNotificationsOpen { get; set; } = false;
        public string InfoNotification { get; set; } 
        public int NotificationCount { get; set; } = Global.Notifications.Count;
        public DelegateCommand ShowNotificationsCommand => new(() =>
        {
			if (NotificationCount == 0) InfoNotification = "Уведомлений пока нет";
			else InfoNotification = "Уведомления";
            IsDialogShowNotificationsOpen = true;
        });
        public DelegateCommand CloseDialogNotificationCommand => new(() =>
        {
			NotificationCount = 0;
            IsDialogShowNotificationsOpen = false;
        });
    }
}
