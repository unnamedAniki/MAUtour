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
using Easing = Mapsui.Animations.Easing;
using Mapsui.Rendering.Skia;
using System.Reflection;
using System.Xml.Linq;
using MAUtour.ViewModels;
using CommunityToolkit.Maui.Views;
using MAUtour.Views.Dialogs;

namespace MAUtour;

public partial class MapPage : ContentPage
{
    //private readonly ApplicationContext context;
    private CancellationTokenSource? gpsCancelation;
    private ObservableCollection<Coordinate> Polyline = new ObservableCollection<Coordinate>();
    private bool isCreatedRoad = true;
    private GenericCollectionLayer<List<IFeature>> pinsLayer;
    private GenericCollectionLayer<List<IFeature>> roadlayer;
    private List<Sources> images = new List<Sources>();
    private string pinName = null;
    private string pinDescription = null;

    public MapPage()
    {
        this.BindingContext = new MapViewModel();
        InitializeComponent();
    }
    class Sources
    {
        public ImageSource source { get; set; }
    }
    protected override void OnAppearing()
    {
        images = new List<Sources>() 
        {
            new Sources()
            {
                source = ImageSource.FromResource("MAUtour.Resources.Images.default-image.jpg") 
            },
            new Sources()
            {
                source = ImageSource.FromResource("MAUtour.Resources.Images.default-image.jpg")
            },
            new Sources()
            {
                source = ImageSource.FromResource("MAUtour.Resources.Images.default-image.jpg")
            },
        };

        CarouselImages.ItemsSource = images;
        string roadName = null;
        string roadDescription = null;
        mapView.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        mapView.Map.Layers.Add(CreatePinsLayer());
        var murmanskBounds = GetLimitsOfMurmansk();
        mapView.IsNorthingButtonVisible = false;
        mapView.IsZoomButtonVisible = false;
        mapView.Map.Navigator.Limiter = new ViewportLimiterKeepWithinExtent();
        mapView.Map.Navigator.RotationLock = true;
        mapView.Map.Navigator.OverridePanBounds = murmanskBounds;
        mapView.Map.Home = n => n.ZoomToBox(murmanskBounds);
        mapView.MyLocationLayer.UpdateMyLocation(new Position(68.95997F, 33.07398F));
        FindButton.Clicked += FindButton_Clicked;
        disableRoadMode.Clicked += DisableRoadMode_Clicked;
        mapView.MapClicked += OnMapClicked;
        mapView.PinClicked += OnPinClicked;
        Coordinate _coords = new();
        GeometryFeature tempPin = new();
        addRoadPin.Clicked += (s, e) =>
        {
            pinsContext.IsVisible = true;
        };

        pinList.ItemSelected += (s, e) =>
        {
            if (_coords != null)
            {
                pinsLayer.Features.Remove(tempPin);
            }
            _coords = (Coordinate)e.SelectedItem;
            tempPin.Geometry = new Point(_coords.X, _coords.Y);
            tempPin.Styles = new Style[]
            {
                SymbolStyles.CreatePinStyle()
            };
            pinsLayer.Features.Add(tempPin);
            pinsLayer.DataHasChanged();
        };

        addPinButton.Clicked += (s, e) =>
        {
            pinsLayer.Features.Remove(tempPin);
            AddPin(_coords);
            pinsContext.IsVisible = false;
        };
        
        CloseButton.Clicked += (s, e) =>
        {
            pinsContext.IsVisible = false;
        };
        
        pinsLayer = new GenericCollectionLayer<List<IFeature>>
        {
            IsMapInfoLayer = true,
            MaxVisible = 100,
            Name = "Pin"
        };
        
        roadlayer = new GenericCollectionLayer<List<IFeature>>
        {
            IsMapInfoLayer = true,
            MaxVisible = 50,
            Name = "Roads"
        };
        //ShowMoreButton.Clicked += async (s, e) =>
        //{
        //    double i = MoreInfoPanel.HeightRequest;
        //    while (i < 450)
        //    {
        //        MoreInfoPanel.HeightRequest = i;
        //        await Task.Delay(5);
        //        i+=10;
        //    }
        //    ShowMoreButton.IsVisible = false;
        //    HideInfoButton.IsVisible = true;
        //};
        //HideInfoButton.Clicked += (s, e) =>
        //{
        //    MoreInfoPanel.IsVisible = false;
        //    HideInfoButton.IsVisible = false;
        //    ShowMoreButton.IsVisible = true;
        //    MoreInfoPanel.HeightRequest = 50;
        //};
        mapView.Map.Layers.Add(roadlayer);
        mapView.Map.Layers.Add(pinsLayer);
        mapView.Map.Info += async (s, e) =>
        {
            if (e.MapInfo?.WorldPosition == null) return;

            var vectorStyle = e.MapInfo?.Feature?.Styles.Where(s => s is CalloutStyle).Cast<CalloutStyle>().FirstOrDefault();
            MoreInfoPanel.IsVisible = false;
            MoreInfoPanel.HeightRequest = 50;
            if (vectorStyle != null)
            {
                vectorStyle.Enabled = !vectorStyle.Enabled;
                MoreInfoPanel.IsVisible = true;
                e.MapInfo?.Layer?.DataHasChanged();
                return;
            }

            if (roadContent.IsVisible == false)
            {
                AddPinDialog addDialog;
                Polyline.Clear();
                if (!await DisplayAlert("Новое место", "Добавить маршрут или метку?", "Маршрут", "Метку") && isCreatedRoad)
                {
                    addDialog = new AddPinDialog(true);
                    PinName.Text = "Наименование: " + pinName;
                    PinDescription.Text = "Описание: " + pinDescription;
                }
                else
                {
                    addDialog = new AddPinDialog(false);
                    roadContent.IsVisible = true;
                }

                await PopupExtensions.ShowPopupAsync(this, addDialog);
            }
            else
            {
                Polyline.Add(new Coordinate(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y));
                if (Polyline.Count > 1)
                {
                    pinList.ItemsSource = Polyline.Skip(1);
                    AddRoad(roadName);
                }
                else
                {
                    PinName.Text = "Наименование: " + roadName;
                    PinDescription.Text = "Описание: " + roadDescription;
                    AddStartPin(roadName, roadDescription, e);
                }
            }
            return;
        };
        StartGPS();
    }

    private void AddRoad(string roadName)
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

    private async void AddStartPin(string name, string description, MapInfoEventArgs e)
    {
        await Task.Run(() =>
        {
            var _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.default-pin.png");
            string imagePath = "MAUtour.Resources.Images.default-image.jpg";
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            var featureBitmapId = BitmapRegistry.Instance.Register(new Sprite(_atlasBitmapId, 0, 0, 21, 21, 1));
            SKBitmap resourceBitmap;
            using (Stream stream = assembly.GetManifestResourceStream(imagePath))
            {
                resourceBitmap = SKBitmap.Decode(stream);
            }
            var callbackImage = CreateCallbackImage(name, description, resourceBitmap);
            var imageId = BitmapRegistry.Instance.Register(callbackImage);
            var calloutStyle = CreateImageCalloutStyle(imageId);
            pinsLayer.Features.Add(new GeometryFeature
            {
                Geometry = new Point(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y),
                Styles = new Style[]
                {
                    new SymbolStyle { BitmapId = featureBitmapId },
                    calloutStyle
                },
            });
            pinsLayer.DataHasChanged();
        });
    }

    private async void AddPin(Coordinate e)
    {
        pinName = await DisplayPromptAsync("Новая метка", "Введите наименование метки", "Добавить", "Отмена", "Название...");
        pinDescription = await DisplayPromptAsync("Новая метка", "Введите описание метки", "Добавить", "Отмена", "Описание...");
        if (pinName == null)
            return;
        var typeofPin = await DisplayActionSheet("Выберите тип метки", "Отмена", "Обычный",
            buttons: new string[] { "Трасса", "Кемпинг", "Опасный участок", "Животные", "Красивое место" });
        if (typeofPin == null)
            return;
        await Task.Run(() =>
        {
            int _atlasBitmapId = 0;
            string imagePath = "MAUtour.";
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            switch (typeofPin)
            {
                case "Трасса":
                    _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.race-pin.png");
                    imagePath += "Resources.Images.race-image.jpg";
                    break;
                case "Кемпинг":
                    _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.green-pin.png");
                    imagePath += "Resources.Images.green-image.jpg";
                    break;
                case "Опасный участок":
                    _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.danger-pin.png");
                    imagePath += "Resources.Images.danger-image.png";
                    break;
                case "Животные":
                    _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.animal-pin.png");
                    imagePath += "Resources.Images.animal-image.png";
                    break;
                case "Красивое место":
                    _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.landscape-pin.png");
                    imagePath += "Resources.Images.landscape-image.png";
                    break;
                default:
                    _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.default-pin.png");
                    imagePath += "Resources.Images.default-image.jpg";
                    break;
            }
            var featureBitmapId = BitmapRegistry.Instance.Register(new Sprite(_atlasBitmapId, 0, 0, 21, 21, 1));
            SKBitmap resourceBitmap;
            using (Stream stream = assembly.GetManifestResourceStream(imagePath))
            {
                resourceBitmap = SKBitmap.Decode(stream);
            }
            var callbackImage = CreateCallbackImage(pinName, pinDescription, resourceBitmap);
            var imageId = BitmapRegistry.Instance.Register(callbackImage);
            var calloutStyle = CreateImageCalloutStyle(imageId);
            pinsLayer.Features.Add(new GeometryFeature
            {
                Geometry = new Point(e.X, e.Y),
                Styles = new Style[]
                {
                    new SymbolStyle { BitmapId = featureBitmapId },
                    calloutStyle
                },
            });
            pinsLayer.DataHasChanged();
        });
    }

    private async Task<string> AddPin(MapInfoEventArgs e)
    {
        pinName = await DisplayPromptAsync("Новая метка", "Введите наименование метки", "Добавить", "Отмена", "Название...", maxLength: 20);
        if (pinName == null)
            return null;
        pinDescription = await DisplayPromptAsync("Новая метка", "Введите описание метки", "Добавить", "Отмена", "Описание...", maxLength: 20);
        if (pinDescription == null)
            return null;
        var typeofPin = await DisplayActionSheet("Выберите тип метки", "Отмена", "Обычный",
            buttons: new string[] { "Трасса", "Кемпинг", "Опасный участок", "Животные", "Красивое место" });
        if (typeofPin == null)
            return null;
        int _atlasBitmapId = 0;
        string imagePath = "MAUtour.";
        Assembly assembly = GetType().GetTypeInfo().Assembly;
        switch (typeofPin)
        {
            case "Трасса":
                _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.race-pin.png");
                imagePath += "Resources.Images.race-image.jpg";
                break;
            case "Кемпинг":
                _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.green-pin.png");
                imagePath += "Resources.Images.green-image.jpg";
                break;
            case "Опасный участок":
                _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.danger-pin.png");
                imagePath += "Resources.Images.danger-image.png";
                break;
            case "Животные":
                _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.animal-pin.png");
                imagePath += "Resources.Images.animal-image.png";
                break;
            case "Красивое место":
                _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.landscape-pin.png");
                imagePath += "Resources.Images.landscape-image.png";
                break;
            default:
                _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.default-pin.png");
                imagePath += "Resources.Images.default-image.jpg";
                break;
        }
        var featureBitmapId = BitmapRegistry.Instance.Register(new Sprite(_atlasBitmapId, 0, 0, 21, 21, 1));
        SKBitmap resourceBitmap;
        using (Stream stream = assembly.GetManifestResourceStream(imagePath))
        {
            resourceBitmap = SKBitmap.Decode(stream);
        }
        var callbackImage = CreateCallbackImage(pinName, pinDescription, resourceBitmap);
        var imageId = BitmapRegistry.Instance.Register(callbackImage);
        var calloutStyle = CreateImageCalloutStyle(imageId);
        pinsLayer.Features.Add(new GeometryFeature
        {
            Geometry = new Point(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y),
            Styles = new Style[]
            {
                new SymbolStyle { BitmapId = featureBitmapId },
                calloutStyle
            },
        });
        pinsLayer.DataHasChanged();
        return pinName;
    }

    private static Style CreateImageCalloutStyle(int bitmapId)
    {
        var calloutStyle = new CalloutStyle
        {
            Content = bitmapId,
            RotateWithMap = true,
            Enabled = false,
            ArrowPosition = 4,
            Type = CalloutType.Custom
        }; 
        calloutStyle.ArrowAlignment = ArrowAlignment.Top;
        calloutStyle.Offset = new Offset(0, -SymbolStyle.DefaultHeight * 0.5f);
        calloutStyle.RectRadius = 10;   
        calloutStyle.ShadowWidth = 4; 
        calloutStyle.StrokeWidth = 0;
        return calloutStyle;
    }

    private MemoryStream CreateCallbackImage(string name, string description, SKBitmap image)
    {
        using var textPaint = new SKPaint
        {
            Color = new SKColor(0, 0, 0),
            Typeface = SKTypeface.FromFamilyName(null, SKFontStyleWeight.Light, SKFontStyleWidth.SemiExpanded,
                SKFontStyleSlant.Upright),
            TextSize = 18
        };
        using var paint = new SKPaint
        {
            Color = new SKColor(255, 255, 255)
        };
        image = image.Resize(new SKSizeI(300, 200), SKFilterQuality.High);
        SKRect bounds;
        using (var textPath = textPaint.GetTextPath("Test: Cool place for me", 0, 0))
        {
            textPath.GetTightBounds(out bounds);
        }
        using var canvas = new SKCanvas(image);
        canvas.DrawRegion(new SKRegion(new SKRectI(0, image.Height-48, image.Width, image.Width-48)), paint);
        canvas.DrawText($"Наименование: {name}", -bounds.Left, -bounds.Top + 156, textPaint);
        canvas.DrawText($"Описание: {description}", -bounds.Left, -bounds.Top + 178, textPaint);
        var memStream = new MemoryStream();
        using (var wStream = new SKManagedWStream(memStream))
        {
            image.Encode(wStream, SKEncodedImageFormat.Png, 100);
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
        //searchLabel.IsVisible = true;
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