
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace MauiBugsRadioButton
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Get the logger used by MAUI. It is LoggerProviderDebugView in the debug mode
            // and MauiAppBuilder.NullLogger in the release mode.
            var logger = Application.Current?.FindMauiContext()?.CreateLogger<RadioButton>();
            string loggerName = logger?.ToString() ?? "NULL";
            AndroidLog($"The logger is: {loggerName}");

            // The following line crashes on x86_64.
            logger?.LogWarning("Warning - {RuntimePlatform} does not support View as the {PropertyName} property of RadioButton; the return value of the ToString() method will be displayed instead.", DeviceInfo.Platform, ContentProperty.PropertyName);
        }

        public static void AndroidLog(string s)
        {
#if ANDROID
            Android.Util.Log.Info("MauiLoggerTest", s);
#endif
        }

    }

    public static class MauiInternalExtensions
    {
        internal static IMauiContext? FindMauiContext(this Element element, bool fallbackToAppMauiContext = false)
        {
            if (element is Microsoft.Maui.IElement fe && fe.Handler?.MauiContext != null)
                return fe.Handler.MauiContext;

            foreach (var parent in element.GetParentsPath())
            {
                if (parent is Microsoft.Maui.IElement parentView && parentView.Handler?.MauiContext != null)
                    return parentView.Handler.MauiContext;
            }

            return fallbackToAppMauiContext ? Application.Current?.FindMauiContext() : default;
        }

        internal static ILogger<T>? CreateLogger<T>(this Element element, bool fallbackToAppMauiContext = true) =>
            element.FindMauiContext(fallbackToAppMauiContext)?.CreateLogger<T>();

        internal static ILogger<T>? CreateLogger<T>(this IMauiContext context) =>
            context.Services.CreateLogger<T>();

        internal static ILogger<T>? CreateLogger<T>(this IServiceProvider services) =>
            services.GetService<ILogger<T>>();

        internal static IEnumerable<Element> GetParentsPath(this Element self)
        {
            Element current = self;

            while (!IsApplicationOrNull(current.RealParent))
            {
                current = current.RealParent;
                yield return current;
            }
        }

        internal static bool IsApplicationOrNull(object? element) =>
            element == null || element is IApplication;
    }
}