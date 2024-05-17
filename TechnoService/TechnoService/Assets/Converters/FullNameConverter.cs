namespace TechnoService.Assets.Converters
{
	public class FullNameConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values[0] != null && values[1] != null && values[2] != null)
				return values[0] + " " + values[1]+  " " + values[2];
			return "";
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			var strValue = value as string;
			if (!string.IsNullOrWhiteSpace(strValue))
			{
				var values = strValue.Split(' ');
				return new object[] { values[0], values[1], values[2] };
			}
			return new object[] { null, null };
		}
	}
}
