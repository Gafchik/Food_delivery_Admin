
using Food_delivery_library.Telegram_Bot;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace Food_delivery_Admin
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        TG_Bot bot = new TG_Bot();
    }
}
//  Host\\bin\\Debug\\Host.exe