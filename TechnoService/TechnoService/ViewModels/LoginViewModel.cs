namespace TechnoService.ViewModels
{
    public class LoginViewModel : BindableBase
    {
		private readonly UserService _userService;
		private readonly PageService _pageService;
		public string Username { get; set; }
		public string Password { get; set; }
		public string ErrorMessage { get; set; }
		public string ErrorMessageButton { get; set; }
		public LoginViewModel(PageService pageService, UserService userService)
        {
            _pageService = pageService;
			_userService = userService;
		}
		public DelegateCommand SignInCommand => new(async () =>
		{
			await Task.Run(async () =>
			{
				if (await _userService.AuthorizationAsync(Username, Password))
				{
					ErrorMessageButton = string.Empty;
					await Application.Current.Dispatcher.InvokeAsync(async () =>
					{
						if (UserSetting.Default.Role == "Диспетчер")
							_pageService.ChangePage(new DispatcherPanel());
						else if(UserSetting.Default.Role == "Менеджер")
                            _pageService.ChangePage(new ManagerPanel());
                        else _pageService.ChangePage(new EmployeePanel());

					});
				}
				else
					ErrorMessageButton = "Неверный логин или пароль";
			});
		}, bool () =>
		{
			if (string.IsNullOrWhiteSpace(Username)
				|| string.IsNullOrWhiteSpace(Password))
			{
				ErrorMessage = "Пустые поля";
				ErrorMessageButton = ErrorMessageButton != string.Empty ? string.Empty : ErrorMessageButton;
			}
			else if (Username.Length < 6)
			{
				ErrorMessage = "Логин слишком короткий";
			}
			else
				ErrorMessage = string.Empty;

			return ErrorMessage == string.Empty;
		});



	}
}
