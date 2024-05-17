using DevExpress.Mvvm.Native;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TechnoService.Data;

namespace TechnoService.Services
{
    public class OrderService
    {
		private readonly DemoContext _demoContext;
        public OrderService(DemoContext demoContext)
		{
			_demoContext = demoContext;
        }
        public async Task SaveChangesAsync() => await _demoContext.SaveChangesAsync();
        public async Task UpdateOrderAsync(Order order)
        {
            _demoContext.Orders.Update(order);
            await _demoContext.SaveChangesAsync();
        }
        public async Task UpdateCatalogOrderAsync(CatalogOrder order)
        {
            _demoContext.CatalogOrders.Update(order);
            await _demoContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            using (var context = new DemoContext())
            {
                return await context.Orders
                    .Include(i => i.IdTypeProblemNavigation)
                    .Include(i => i.IdTypeEquipmentNavigation)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }
        public async Task<IEnumerable<CatalogOrder>> GetCatalogOrdersAsync()
        {
            using (var context = new DemoContext())
            {
                return await context.CatalogOrders
                    .Include(i => i.IdEmployeeNavigation)
                    .Include(i => i.IdOrdersNavigation)
                    .Include(i => i.IdOrdersNavigation.IdTypeEquipmentNavigation)
                    .Include(i => i.IdOrdersNavigation.IdTypeProblemNavigation)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }
        public async Task<IEnumerable<CatalogOrder>> GetOrdersEmployeeAsync(int IdEmployee)
        {
            using (var context = new DemoContext())
            {
                return await _demoContext.CatalogOrders.Where(i => i.IdEmployee == IdEmployee && i.IdOrdersNavigation.Status != "Выполнено")
                    .Include(i => i.IdOrdersNavigation.IdTypeProblemNavigation)
                    .Include(i => i.IdOrdersNavigation.IdTypeEquipmentNavigation).AsNoTracking().ToListAsync();
            }
        }
        public async Task<IEnumerable<CatalogOrder>> GetCatalogOrdersManagerAsync()
        {
            using (var context = new DemoContext())
            {
                return await _demoContext.CatalogOrders.Where(i => i.IdOrdersNavigation.Status != "Выполнено")
                    .Include(i => i.IdOrdersNavigation.IdTypeProblemNavigation)
                    .Include(i => i.IdOrdersNavigation.IdTypeEquipmentNavigation).AsNoTracking().ToListAsync();
            }
        }
        public async Task<IEnumerable<Order>> GetOrdersManagerAsync()
        {
            using (var context = new DemoContext()) 
            {
                return await _demoContext.Orders.Where(i => i.Status != "Выполнено")
                    .Include(i => i.IdTypeProblemNavigation)
                    .Include(i => i.IdTypeEquipmentNavigation).AsNoTracking().ToListAsync();
            }
        }
        public async Task<IEnumerable<TypeProblem>> GetTypeProblemsAsync()
		{
            using (var context = new DemoContext())
            {
                return await _demoContext.TypeProblems.AsNoTracking().ToListAsync();
            }
        }
        public async Task<IEnumerable<TypeEquipment>> GetTypeEquipmentAsync()
        {
            using (var context = new DemoContext())
                return await _demoContext.TypeEquipments.AsNoTracking().ToListAsync();
        }
        public async Task AddNewOrder(string NameEquipment, int IdTypeProblem, string NameClient, 
			DateTime PeriodExecution, int IdTypeEquipment, int Number, string Description)
		{
			await _demoContext.Orders.AddAsync(new Order
            {
				DateAddition = DateTime.Now,
				NameEquipment = NameEquipment,
				IdTypeProblem = IdTypeProblem,
				Status = "В ожидании",
				NameClient = NameClient,
				Cost = 0, 
				WorkStatus = "Не выполнено", 
				PeriodExecution = PeriodExecution,
				IdTypeEquipment = IdTypeEquipment, 
				SerialNumber = Number, 
				DescriptionApplication = Description
			});
			await _demoContext.SaveChangesAsync();
		}
        public async Task AddNewCatalogOrder(int IdOrder, int IdEmployee)
        {
            await _demoContext.CatalogOrders.AddAsync(new CatalogOrder
            {
                IdOrders = IdOrder,
                IdEmployee = IdEmployee
            });
            await _demoContext.SaveChangesAsync();
        }
    }
}
