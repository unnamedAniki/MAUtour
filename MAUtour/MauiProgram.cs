using CommunityToolkit.Maui;

using MAUtour.Local.DBConnect;
using MAUtour.Local.UnitOfWork;
using MAUtour.Local.UnitOfWork.Interface;
using MAUtour.Utils.DbConnect;

using Microsoft.EntityFrameworkCore;
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
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
			.UseSkiaSharp(true)
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services
			.AddDbContext<LocalContext>()
            .AddSingleton<IUnitOfWork, UnitOfWork>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
