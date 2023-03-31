using ExCSS;

using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.UI;
using Mapsui.UI.Maui;

using MAUtour.Utils.DbConnect;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

using SkiaSharp;

using Brush = Mapsui.Styles.Brush;
using Color = Mapsui.Styles.Color;

namespace MAUtour;

public partial class MapPage : ContentPage
{
    private readonly ApplicationContext context;
    private CancellationTokenSource? gpsCancelation;
    private List<Pin> Pins= new List<Pin>();
    public MapPage()
    {
        InitializeComponent();
        context = new ApplicationContext();
        mapView.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        mapView.Map.Layers.Add(new WritableLayer()
        {
            IsMapInfoLayer = true,
            Name = "test"
        });
        var test = new LineString(Pins.Select(p=>p.Feature.Geometry.Coordinates));
        mapView.Map.Layers.Add(new Polygon { Feature = new GeometryFeature() });

        mapView.Map.Info += NewPin;
        //mapView.Map.Limiter.ZoomLimits = new Mapsui.UI.MinMax(1,100);
        mapView.MyLocationLayer.UpdateMyLocation(new Position(68.95997F, 33.07398F));
        FindButton.Clicked += FindButton_Clicked;
        var layer = new GenericCollectionLayer<List<IFeature>>
        {
            Style = SymbolStyles.CreatePinStyle()
        };
        mapView.Map.Layers.Add(layer);

        mapView.Map.Info += async (s, e) =>
        {
            if (e.MapInfo?.WorldPosition == null) return;
            if (!await DisplayAlert("Новая метка", "Хотите создать новый маршрут?", "Да", "Нет")) return;
            // Add a point to the layer using the Info position
            layer?.Features.Add(new GeometryFeature
            {
                Geometry = new NetTopologySuite.Geometries.Point(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y)
            });
            Pins.Add(new Pin() { Position = new Position(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y) });
            
            // To notify the map that a redraw is needed.
            layer?.DataHasChanged();
            return;
        };

        mapView.IsNorthingButtonVisible = false;
        mapView.IsZoomButtonVisible = false;
        mapView.SelectedPinChanged += View_SelectedPinChanged;
        mapView.MapClicked += OnMapClicked;
        mapView.PinClicked += OnPinClicked;
        StartGPS();
    }

    private void FindButton_Clicked(object sender, EventArgs e)
    {
        searchLabel.IsVisible = true;
    }

    private void AddButton_Clicked(object sender, System.EventArgs e)
    {
        // Get a random location for the new pin
        double latitude = 0f;
        double longitude = 0f;

        // Create a new pin object and add it to the pins list
        Pin newPin = new Pin()
        {
            Label = "New Pin",
            Position = new Position(latitude, longitude)
        };
        Pins.Add(newPin);

        // Add the new pin to the map control
        MapControl mapControl = (MapControl)Content;
        MemoryProvider layerProvider = (MemoryProvider)mapControl.Map.Layers[0];
        layerProvider.Features.Add();
    }

    private void OnPinClicked(object? sender, PinClickedEventArgs e)
    {
        if (e.Pin != null)
        {
            if (e.NumOfTaps == 2)
            {
                e.Pin.IsVisible = false;
            }
            if (e.NumOfTaps == 1)
            {
                mapContext.IsVisible = true;
            } 
        }
        e.Handled = true;
    }

    private static MemoryStream CreateCallbackImage(Pin pin)
    {
        using var paint = new SKPaint
        {
            Color = new SKColor((byte)10, (byte)10, (byte)10),
            Typeface = SKTypeface.FromFamilyName(null, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal,
                SKFontStyleSlant.Upright),
            TextSize = 20
        };

        SKRect bounds;
        using (var textPath = paint.GetTextPath(pin.Address, 0, 0))
        {
            // Set transform to center and enlarge clip path to window height
            textPath.GetTightBounds(out bounds);
        }

        using var bitmap = new SKBitmap((int)(bounds.Width + 1), (int)(bounds.Height + 1));
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear();
        canvas.DrawText(pin.Address, -bounds.Left, -bounds.Top, paint);
        var memStream = new MemoryStream();
        using (var wStream = new SKManagedWStream(memStream))
        {
            bitmap.Encode(wStream, SKEncodedImageFormat.Png, 100);
        }
        return memStream;
    }

    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        mapContext.IsVisible = false;
    }

    private void NewPin(object sender, EventArgs e)
    {
        var pins = context.UserPins.ToList();
        foreach (var dbPin in pins)
        {
            var pin = new Pin()
            {
                Position = new Position(dbPin.Latitude, dbPin.Longitude),
                Label = dbPin.Id.ToString(),
                Address = dbPin.Description,
                IsVisible = true,
                MinVisible = 0.5,
                Color = new Microsoft.Maui.Graphics.Color(10, 10, 60)
            };
            
            mapView.Pins.Add(pin);
        }
        mapView.RefreshData();
    }

    private void View_SelectedPinChanged(object sender, SelectedPinChangedEventArgs e)
    {
        var selectedPin = context.UserPins.Include(p=>p.Type).Where(p => p.Id == int.Parse(e.SelectedPin.Label)).FirstOrDefault();
        nameLabel.Text = selectedPin.Name;
        descriptionLabel.Text = selectedPin.Description;
        additionalInfo.Text = selectedPin.Type.Name;
    }

    [Obsolete]
    public async void StartGPS()
    {
        try
        {
            this.gpsCancelation?.Dispose();
            this.gpsCancelation = new CancellationTokenSource();

            await Task.Run(async () =>
            {
                while (!gpsCancelation.IsCancellationRequested)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
#if __MAUI__ // WORKAROUND for Preview 11 will be fixed in Preview 13 https://github.com/dotnet/maui/issues/3597
                    if (Application.Current == null)
                        return;

                    await Application.Current.Dispatcher.DispatchAsync(async () =>
                    {
#else
                    await Device.InvokeOnMainThreadAsync(async () => {
#endif
                        var location = await Geolocation.GetLocationAsync(request, this.gpsCancelation.Token)
                            .ConfigureAwait(false);
                        if (location != null)
                        {
                            MyLocationPositionChanged(location);
                        }
                    }).ConfigureAwait(false);

                    await Task.Delay(200).ConfigureAwait(false);
                }
            }, gpsCancelation.Token).ConfigureAwait(false);
        }
        catch 
        {
            
        }
    }
    private async void MyLocationPositionChanged(Location e)
    {
        try
        {
            await Application.Current?.Dispatcher?.DispatchAsync(() =>
            {
                mapView?.MyLocationLayer.UpdateMyLocation(new Position(e.Latitude, e.Longitude));
                if (e.Course != null)
                {
                    mapView?.MyLocationLayer.UpdateMyDirection(e.Course.Value, 0);
                }

                if (e.Speed != null)
                {
                    mapView?.MyLocationLayer.UpdateMySpeed(e.Speed.Value);
                }

            })!;
        }
        catch 
        {
           
        }
    }
}