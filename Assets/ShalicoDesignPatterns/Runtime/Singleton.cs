namespace ShalicoDesignPatterns
{
    public class Singleton<T> where T : class, new()
    {
        private static T s_instance;
        public static T Instance => s_instance ??= new T();
        public static bool IsValid => s_instance != null;

        public static void Destroy()
        {
            s_instance = null;
        }
    }
}