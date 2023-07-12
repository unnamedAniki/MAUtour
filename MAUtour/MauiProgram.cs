using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using MAUtour.Utils.DbConnect;

using Microsoft.Extensions.Logging;

using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MAUtour;

public static class MauiProgram
{
	private static readonly string host = "localhost";
	private static readonly string port = "5014";
	static string ApiUrl = $"http://{host}:{port}/GPS/"; 
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp(true)
			.UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
