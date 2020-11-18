using System.Collections.Generic;
using System.Linq;

public static class UIManager
{
    public static List<Window> windows = new List<Window>();

    public static T GetWindow<T>() where T : Window
    {
        return windows.OfType<T>().FirstOrDefault();
    }
    
    public static T[] GetWindows<T>() where T : Window
    {
        List<T> temp = new List<T>();

        foreach (var window in windows)
        {
            if (window as T)
            {
                temp.Add(window.GetComponent<T>());
            }
        }
        return temp.ToArray();
    }

    public static void RegisterWindow(Window window)
    {
        windows.Add(window);
        window.Close();
    }
        
    public static void UnregisterWindow(Window window)
    {
        windows.Remove(window);
    }
}