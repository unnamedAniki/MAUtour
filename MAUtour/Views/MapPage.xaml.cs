using ExCSS;

using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.UI;
using Mapsui.UI.Maui;

using MAUtour.Utils.DbConnect;

using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Shapes;

using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

using SkiaSharp;

using static Microsoft.Maui.ApplicationModel.Permissions;

using Brush = Mapsui.Styles.Brush;
using Color = Mapsui.Styles.Color;
using Polyline = Mapsui.UI.Maui.Polyline;
using Position = Mapsui.UI.Maui.Position;

namespace MAUtour;

public partial class MapPage : ContentPage
{
    //private readonly ApplicationContext context;
    private CancellationTokenSource? gpsCancelation;
    private List<Coordinate> Polyline = new List<Coordinate>();
    private bool isCreatedRoad = true;
    private ILayer roadlayer;
    private GenericCollectionLayer<List<IFeature>> pinLayer;
    public MapPage()
    {
        InitializeComponent();

    }

    protected override void OnAppearing()
    {
        mapView.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        mapView.Map.Layers.Add(new WritableLayer()
        {
            IsMapInfoLayer = true,
            Name = "test`"
        });

        //mapView.Map.Info += NewPin;
        //mapView.Map.Limiter.ZoomLimits = new Mapsui.UI.MinMax(1,100);
        mapView.MyLocationLayer.UpdateMyLocation(new Position(68.95997F, 33.07398F));
        FindButton.Clicked += FindButton_Clicked;
        pinLayer = new GenericCollectionLayer<List<IFeature>>
        {
            Style = SymbolStyles.CreatePinStyle(),
            MaxVisible = 100,
            Name = "Pins"
        };
        mapView.Map.Layers.Add(pinLayer);
        mapView.Map.Info += async (s, e) =>
        {
            disableRoadMode.Clicked += DisableRoadMode_Clicked;
            if (e.MapInfo?.WorldPosition == null) return;
            if (roadContent.IsVisible == false)
            {
                Polyline.Clear();
                if (!await DisplayAlert("Новая метка", "Добавить маршрут или метку?", "Маршрут", "Метку") && isCreatedRoad)
                {
                    isCreatedRoad = false;
                    pinLayer?.Features.Add(new GeometryFeature
                    {
                        Geometry = new NetTopologySuite.Geometries.Point(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y),
                    });
                    
                    pinLayer?.DataHasChanged();
                    roadContent.IsVisible = false;
                    return;
                }
            }
            else
            {
                Polyline.Add(new Coordinate(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y));
                if (Polyline.Count > 1)
                {
                    roadlayer = CreateLayer();
                    mapView.Map.Layers.Add(roadlayer);
                }
                else
                {
                    pinLayer?.Features.Add(new GeometryFeature
                    {
                        Geometry = new NetTopologySuite.Geometries.Point(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y),
                    });
                }
                pinLayer?.DataHasChanged();
            }
            roadContent.IsVisible = true;

            return;
        };

        mapView.IsNorthingButtonVisible = false;
        mapView.IsZoomButtonVisible = false;
        //mapView.SelectedPinChanged += View_SelectedPinChanged;
        mapView.MapClicked += OnMapClicked;
        mapView.PinClicked += OnPinClicked;
        StartGPS();
    }

    private void DisableRoadMode_Clicked(object sender, EventArgs e)
    {
        roadContent.IsVisible = false;
        isCreatedRoad = true;
    }

    public ILayer CreateLayer()
    {
        return new Layer("Polygons")
        {
            DataSource = new MemoryProvider(CreatePolygon()),
            Style = null,
            MaxVisible = 50
        };
    }
    private IFeature CreatePolygon()
    {
        return new GeometryFeature
        {
            Geometry = new LineString(Polyline.ToArray()),
            Styles = new[]
            {
                new VectorStyle
                {
                    Line = new Pen(Color.Gray, 10) {PenStrokeCap = PenStrokeCap.Butt, StrokeJoin = StrokeJoin.Miter}
                }
            },
        };
    }
        
    private void FindButton_Clicked(object sender, EventArgs e)
    {
        searchLabel.IsVisible = true;
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

    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        mapContext.IsVisible = false;
    }

    //private void NewPin(object sender, EventArgs e)
    //{
    //    var pins = context.UserPins.ToList();
    //    foreach (var dbPin in pins)
    //    {
    //        var pin = new Pin()
    //        {
    //            Position = new Position(dbPin.Latitude, dbPin.Longitude),
    //            Label = dbPin.Id.ToString(),
    //            Address = dbPin.Description,
    //            IsVisible = true,
    //            MinVisible = 0.5,
    //            Color = new Microsoft.Maui.Graphics.Color(10, 10, 60)
    //        };

    //        mapView.Pins.Add(pin);
    //    }
    //    mapView.RefreshData();
    //}

    //private void View_SelectedPinChanged(object sender, SelectedPinChangedEventArgs e)
    //{
    //    var selectedPin = context.UserPins.Include(p => p.Type).Where(p => p.Id == int.Parse(e.SelectedPin.Label)).FirstOrDefault();
    //    nameLabel.Text = selectedPin.Name;
    //    descriptionLabel.Text = selectedPin.Description;
    //    additionalInfo.Text = selectedPin.Type.Name;
    //}

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
    private async void MyLocationPositionChanged(Microsoft.Maui.Devices.Sensors.Location e)
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