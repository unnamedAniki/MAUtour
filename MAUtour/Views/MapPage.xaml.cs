using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Limiting;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.UI.Maui;

using MAUtour.Utils.DbConnect;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

using SkiaSharp;

using System.Collections.ObjectModel;

using Brush = Mapsui.Styles.Brush;
using Color = Mapsui.Styles.Color;
using Map = Mapsui.Map;
using Polyline = Mapsui.UI.Maui.Polyline;
using Point = NetTopologySuite.Geometries.Point;
using Position = Mapsui.UI.Maui.Position;
using Style = Mapsui.Styles.Style;

namespace MAUtour;

public partial class MapPage : ContentPage
{
    //private readonly ApplicationContext context;
    private CancellationTokenSource? gpsCancelation;
    private ObservableCollection<Coordinate> Polyline = new ObservableCollection<Coordinate>();
    private bool isCreatedRoad = true;
    private GenericCollectionLayer<List<IFeature>> pinsLayer;
    private GenericCollectionLayer<List<IFeature>> roadlayer;
    public MapPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        mapView.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        mapView.Map.Layers.Add(CreatePinsLayer());
        var murmanskBounds = GetLimitsOfMurmansk();
        mapView.Map.Navigator.Limiter = new ViewportLimiterKeepWithinExtent();
        mapView.Map.Navigator.RotationLock = true;
        mapView.Map.Navigator.OverridePanBounds = murmanskBounds;
        mapView.Map.Home = n => n.ZoomToBox(murmanskBounds);
        mapView.MyLocationLayer.UpdateMyLocation(new Position(68.95997F, 33.07398F));
        FindButton.Clicked += FindButton_Clicked;
        disableRoadMode.Clicked += DisableRoadMode_Clicked;
        CloseButton.Clicked += (s, e) =>
        {
            pinsContext.IsVisible = false;
        };
        pinsLayer = new GenericCollectionLayer<List<IFeature>>
        {
            IsMapInfoLayer = true,
            Name = "Pin"
        };
        roadlayer = new GenericCollectionLayer<List<IFeature>>
        {
            IsMapInfoLayer = true,
            Name = "Roads"
        };
        mapView.Map.Layers.Add(roadlayer);
        mapView.Map.Layers.Add(pinsLayer);
        string roadName = null;
        string pinName = null;
        mapView.Map.Info += async (s, e) =>
        {
            var vectorStyle = e.MapInfo?.Feature?.Styles.Where(s => s is CalloutStyle).Cast<CalloutStyle>().FirstOrDefault();
            if (vectorStyle != null && e.MapInfo?.Layer?.Name == "Roads")
            {
                vectorStyle.Enabled = !vectorStyle.Enabled;
                e.MapInfo?.Layer?.DataHasChanged();
                return;
            }
            var pinStyle = e.MapInfo?.Feature?.Styles.Where(s => s is CalloutStyle).Cast<CalloutStyle>().FirstOrDefault();
            if (pinStyle != null && e.MapInfo?.Layer?.Name == "Pin")
            {
                pinStyle.Enabled = !pinStyle.Enabled;
                e.MapInfo.Layer?.DataHasChanged();
                return;
            }
            if (e.MapInfo?.WorldPosition == null) return;
            if (roadContent.IsVisible == false)
            {
                Polyline.Clear();
                if (!await DisplayAlert("Новое место", "Добавить маршрут или метку?", "Маршрут", "Метку") && isCreatedRoad)
                {
                    if(pinName == null)
                    {
                        pinName = await DisplayPromptAsync("Новая метка", "Введите наименование метки", "Добавить", "Отмена", "Название...");
                    }
                    var callbackImage = CreateCallbackImage(pinName);
                    var bitmapId = BitmapRegistry.Instance.Register(callbackImage);
                    var calloutStyle = CreateCalloutStyle(bitmapId);
                    pinsLayer.Features.Add(new GeometryFeature
                    {
                        Geometry = new Point(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y),
                        Styles = new Style[]
                        {
                            SymbolStyles.CreatePinStyle(),
                            calloutStyle
                        },
                    });
                    pinsLayer.DataHasChanged();
                    roadContent.IsVisible = false;
                    return;
                }
                else
                {
                    roadName = await DisplayPromptAsync("Новый маршрут", "Введите наименование маршрута", "Добавить", "Отмена", "Название...");
                }
            }
            else
            {
                Polyline.Add(new Coordinate(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y));

                addRoadPin.Clicked += (s, e) =>
                {
                    pinsContext.IsVisible = true;
                    pinList.ItemsSource = Polyline;
                    pinList.ItemSelected += async (s, e) =>
                    {
                        var coords = (Coordinate)e.SelectedItem;
                        var text = await DisplayPromptAsync("Новая метка", "Введите наименование метки", "Добавить", "Отмена", "Название...");
                        var callbackImage = CreateCallbackImage(text);
                        var bitmapId = BitmapRegistry.Instance.Register(callbackImage);
                        var calloutStyle = CreateCalloutStyle(bitmapId);
                        pinsLayer.Features.Add(new GeometryFeature
                        {
                            Geometry = new Point(coords.X, coords.Y),
                            Styles = new Style[]
                            {
                                SymbolStyles.CreatePinStyle(),
                                calloutStyle
                            },
                        });
                        pinsLayer.DataHasChanged();
                    };
                };
                if (Polyline.Count > 1)
                {
                    roadlayer.Features.Add(new GeometryFeature
                    {
                        Styles = new Style[]
                        {
                             new VectorStyle
                             {
                                Line = new Pen(Color.Gray, 10) { PenStrokeCap = PenStrokeCap.Butt, StrokeJoin = StrokeJoin.Miter }
                             },
                             new CalloutStyle
                             {
                                Title = roadName,
                                TitleFont = { FontFamily = null, Size = 12, Italic = false, Bold = true },
                                TitleFontColor = Color.Gray,
                                MaxWidth = 120,
                                RectRadius = 10,
                                RotateWithMap = true,
                                ShadowWidth = 4,
                                Enabled = false,
                                SymbolOffset = new Offset(0, SymbolStyle.DefaultHeight * 1f)
                             }
                        },
                        Geometry = new LineString(Polyline.ToArray()),
                    });
                    roadlayer.DataHasChanged();
                }
                else
                {
                    var text = await DisplayPromptAsync("Новая метка", "Введите наименование метки", "Добавить", "Отмена", "Название...");
                    var callbackImage = CreateCallbackImage(text);
                    var bitmapId = BitmapRegistry.Instance.Register(callbackImage);
                    var calloutStyle = CreateCalloutStyle(bitmapId);
                    pinsLayer.Features.Add(new GeometryFeature
                    {
                        Geometry = new Point(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y),
                        Styles = new Style[]
                        {
                             SymbolStyles.CreatePinStyle(),
                             calloutStyle
                        },
                    });
                    pinsLayer.DataHasChanged();
                }
            }
            roadName = null;
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

    private static Style CreateCalloutStyle(int bitmapId)
    {
        var rand = new Random();
        var calloutStyle = new CalloutStyle { Content = bitmapId, ArrowPosition = rand.Next(1, 9) * 0.1f, RotateWithMap = true, Type = CalloutType.Custom };
        switch (rand.Next(0, 4))
        {
            case 0:
                calloutStyle.ArrowAlignment = ArrowAlignment.Bottom;
                calloutStyle.Offset = new Offset(0, SymbolStyle.DefaultHeight * 0.5f);
                break;
            case 1:
                calloutStyle.ArrowAlignment = ArrowAlignment.Left;
                calloutStyle.Offset = new Offset(SymbolStyle.DefaultHeight * 0.5f, 0);
                break;
            case 2:
                calloutStyle.ArrowAlignment = ArrowAlignment.Top;
                calloutStyle.Offset = new Offset(0, -SymbolStyle.DefaultHeight * 0.5f);
                break;
            case 3:
                calloutStyle.ArrowAlignment = ArrowAlignment.Right;
                calloutStyle.Offset = new Offset(-SymbolStyle.DefaultHeight * 0.5f, 0);
                break;
        }
        calloutStyle.RectRadius = 10; // Random.Next(0, 9);
        calloutStyle.ShadowWidth = 4; // Random.Next(0, 9);
        calloutStyle.StrokeWidth = 0;
        calloutStyle.Enabled = false;
        return calloutStyle;
    }

    private MemoryStream CreateCallbackImage(string city)
    {
        var rand = new Random();
        using var paint = new SKPaint
        {
            Color = new SKColor((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256)),
            Typeface = SKTypeface.FromFamilyName(null, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal,
                SKFontStyleSlant.Upright),
            TextSize = 20
        };

        SKRect bounds;
        using (var textPath = paint.GetTextPath(city, 0, 0))
        {
            // Set transform to center and enlarge clip path to window height
            textPath.GetTightBounds(out bounds);
        }

        using var bitmap = new SKBitmap((int)(bounds.Width + 1), (int)(bounds.Height + 1));
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear();
        canvas.DrawText(city, -bounds.Left, -bounds.Top, paint);
        var memStream = new MemoryStream();
        using (var wStream = new SKManagedWStream(memStream))
        {
            bitmap.Encode(wStream, SKEncodedImageFormat.Png, 100);
        }
        return memStream;
    }

    private static MRect GetLimitsOfMurmansk()
    {
        var (minX, minY) = SphericalMercator.FromLonLat(26.6748, 66.03587);
        var (maxX, maxY) = SphericalMercator.FromLonLat(40.86914, 70.2446);
        return new MRect(minX, minY, maxX, maxY);
    }

    private void DisableRoadMode_Clicked(object sender, EventArgs e)
    {
        roadContent.IsVisible = false;
    }

    private Layer CreatePinsLayer()
    {
        return new Layer
        {
            Name = "Pins",
            IsMapInfoLayer = true
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