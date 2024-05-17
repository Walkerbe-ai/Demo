using TechnoService.Data;
using System.Collections.Generic;

namespace TechnoService.Services
{
    public class UserService
    {
        private readonly DemoContext _demoContext;
        public UserService(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }
		public async Task<IEnumerable<Human>> GetEmployeeAsync()
		{
            using (var context = new DemoContext())
                return await context.Humans.Where(u => u.RoleUser == "Исполнитель").AsNoTracking().ToListAsync();
		}
        
        public async Task SaveChangesAsync() => await _demoContext.SaveChangesAsync();
		public async Task<List<string>> GetAllEmployeeLogin() => await _demoContext.Humans.Select(u => u.Login).AsNoTracking().ToListAsync();

		public async Task<bool> AuthorizationAsync(string username, string password)
		{
			var user = await _demoContext.Humans.SingleOrDefaultAsync(u => u.Login == username);
			if (user == null)
				return false;
			if (user.PasswordUser.Equals(password))
			{
				UserSetting.Default.Fullname = user.FullnameUser;
				UserSetting.Default.Role = user.RoleUser;
				UserSetting.Default.Login = user.Login;
				UserSetting.Default.Password = user.PasswordUser;
				UserSetting.Default.IdUser = user.IdUser;
				return true;
			}
			return false;
		}
		public async Task AddNewEmployee(string UserFullName, string UserLogin, string UserPassword)
		{
			await _demoContext.Humans.AddAsync(new Human
            {
				FullnameUser = UserFullName,
				Login = UserLogin,
				PasswordUser = UserPassword,
				RoleUser = "Исполнитель"
			});
			await _demoContext.SaveChangesAsync();
		}


	}
}
