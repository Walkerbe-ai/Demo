namespace TechnoService.Assets.Converters
{
	public class FormatSNConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = value as string;
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}

			Regex regex = new Regex("[^а-яА-Я]+"); 
			return regex.Replace(str, "");
		}
	}
}
