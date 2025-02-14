namespace CoreGame
{
	public static class Locator<T>
	{
		private static T _instance;
		public static void Set(T instance)
		{
			_instance = instance;
		}

		public static T Instance => _instance;
	}
}