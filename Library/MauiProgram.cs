using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using LibraryDAL;
using LibraryDAL.EFModels;

namespace Library
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<LibraryDbContext>();
            builder.Services.AddSingleton<IDataBaseFacade<Person>, PersonFacade>();
            builder.Services.AddSingleton<IDataBaseFacade<Book>, BookFacade>();
            builder.Services.AddSingleton<IDataBaseFacade<BookReservation>, BookReservationFacade>();

            builder.Services.AddSingleton<MainPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
