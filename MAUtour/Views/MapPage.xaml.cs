using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Limiting;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.UI.Maui;

using NetTopologySuite.Geometries;

using SkiaSharp;
using Color = Mapsui.Styles.Color;
using Map = Mapsui.Map;
using Polyline = Mapsui.UI.Maui.Polyline;
using Point = NetTopologySuite.Geometries.Point;
using Position = Mapsui.UI.Maui.Position;
using Style = Mapsui.Styles.Style;
using Easing = Mapsui.Animations.Easing;
using System.Reflection;
using MAUtour.ViewModels;
using CommunityToolkit.Maui.Views;
using MAUtour.Views.Dialogs;
using MAUtour.Local.UnitOfWork.Interface;
using Mapsui.Nts.Extensions;
using MAUtour.Local.Models;

namespace MAUtour;

public partial class MapPage : ContentPage
{
    private CancellationTokenSource? gpsCancelation;
    private List<Coordinate> RoadPins = new();
    private int road_id = 0;
    private GenericCollectionLayer<List<IFeature>> pinsLayer;
    private GenericCollectionLayer<List<IFeature>> tempRoad;
    private List<Sources> images = new List<Sources>();
    private IUnitOfWork _context;
    public MapPage(IUnitOfWork unitOfWork)
    {
        _context = unitOfWork;
        BindingContext = new MapViewModel(unitOfWork);
        InitializeComponent();
    }

    async void PrepareData()
    {
        tempRoad.Features.Clear();
        var test = await _context.routesRepository.GetAllAsync();
        foreach (var route in test)
        {
            var currentRoad = await _context.routesRepository.GetAllPinsOfRouteAsync(route.Id);
            RoadPins.AddRange(currentRoad.DistinctBy(p=>p.Latitude).Select(p=> new Coordinate { X=p.Latitude, Y=p.Longitude }));
            AddStartPin(route.Name, route.Description, RoadPins.First());
            AddRoad(RoadPins, route.RouteTypeId);
            RoadPins.Clear();
        }
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
        closeRoad.Clicked += DisableRoadMode_Clicked;
        mapView.MapClicked += OnMapClicked;
        mapView.PinClicked += OnPinClicked;
        Coordinate _coords = new();
        GeometryFeature tempPin = new();
        addRoadPin.Clicked += (s, e) =>
        {
            pinsContext.IsVisible = true;
        };
        RecordButton.Clicked += (s, e) =>
        {
            roadContent.IsVisible = true;
            CreateRoadByLocation();
        };
        saveRoad.Clicked += async (s, e) =>
        {
            var cleared_Pins = RoadPins.DistinctBy(p=>p.CoordinateValue);
            tempRoad.Features.RemoveAt(tempRoad.Features.Count-1);
            List<RoutePins> pins = new();
            foreach(var item in cleared_Pins)
            {
                pins.Add(
                    new RoutePins
                    {
                        RoutesId = road_id,
                        Latitude = item.X,
                        Longitude = item.Y,
                    });
            }
            var result = await _context.routesRepository.AddRoutePinsAsync(pins);
            if (result)
                await _context.CommitAsync();
            roadContent.IsVisible = false;
            RoadPins.Clear();
            PrepareData();
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

        tempRoad = new GenericCollectionLayer<List<IFeature>>
        {
            IsMapInfoLayer = true,
            MaxVisible = 50,
            Name = $"temp"
        };

        PrepareData();
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
        mapView.Map.Layers.Add(tempRoad);
        mapView.Map.Layers.Add(pinsLayer);
        mapView.Map.Info += async (s, e) =>
        {
            if (e.MapInfo?.WorldPosition == null) return;
            var lineStyle = e.MapInfo?.Feature?.Styles.Where(s=>s is VectorStyle).Cast<VectorStyle>().FirstOrDefault();
            var vectorStyle = e.MapInfo?.Feature?.Styles.Where(s => s is CalloutStyle).Cast<CalloutStyle>().FirstOrDefault();
            MoreInfoPanel.IsVisible = false;
            MoreInfoPanel.HeightRequest = 50;
            if (lineStyle != null && vectorStyle != null)
            {
                mapView.Map.Navigator.CenterOnAndZoomTo(e.MapInfo.WorldPosition, 1, 500, Easing.SinIn);
                await Task.Delay(550);
                vectorStyle.Enabled = !vectorStyle.Enabled;
                MoreInfoPanel.IsVisible = !MoreInfoPanel.IsVisible;
                e.MapInfo?.Layer?.DataHasChanged();
                return;
            }
            if (!roadContent.IsVisible)
            {
                ChooseActionDialog addDialog;
                RoadPins.Clear();
                addDialog = new ChooseActionDialog(this, _context, e.MapInfo.WorldPosition);
                var result = await PopupExtensions.ShowPopupAsync(this, addDialog);
                
                if (result is bool boolResult)
                {
                    await AddInfoToMap(e, boolResult);
                }
            }
            else
            {
                RoadPins.Add(new MPoint(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y).ToCoordinate());
                if (RoadPins.Count > 1)
                {
                    pinList.ItemsSource = RoadPins.Skip(1);
                    AddRoad(RoadPins);
                }
                else
                {
                    AddStartPin(roadName, roadDescription, RoadPins.First());
                }
            }
            return;
        };
        
        StartGPS();
    }

    private async void CreateRoadByLocation()
    {
        RoadPins.Clear();
        while (roadContent.IsVisible)
        {
            await Task.Delay(5000);
            RoadPins.Add(mapView?.MyLocationLayer.MyLocation.ToCoordinate());
            if (RoadPins.Count > 1)
            {
                AddRoad(RoadPins);
            }
        }
    }

    private async Task AddInfoToMap(MapInfoEventArgs e, bool boolResult)
    {
        if (boolResult)
        {
            AddPinDialog addPin;
            addPin = new AddPinDialog(_context, e.MapInfo.WorldPosition);
            var pin_result = await PopupExtensions.ShowPopupAsync(this, addPin);
            if (pin_result is MPoint addedPin)
            {
                AddPin(addedPin.ToCoordinate());
            }
        }
        else
        {
            AddRouteDialog addRoute;
            addRoute = new AddRouteDialog(_context);
            var route_result = await PopupExtensions.ShowPopupAsync(this, addRoute);
            if (route_result is bool)
            {
                road_id = await _context.routesRepository.GetLastAddedRouteAsync();
                roadContent.IsVisible = true;
            }
        }
    }

    private void AddRoad(IEnumerable<Coordinate> roadPins, int routeType = 0)
    {
        var roadStyle = new VectorStyle();
        switch (routeType)
        {
            case 1:
                roadStyle.Line = new Pen(Color.Orange, 5)
                {
                    PenStrokeCap = PenStrokeCap.Butt,
                    StrokeJoin = StrokeJoin.Bevel
                };
                break;
            case 2:
                roadStyle.Line = new Pen(Color.Green, 5)
                {
                    PenStrokeCap = PenStrokeCap.Butt,
                    StrokeJoin = StrokeJoin.Miter
                };
                break;
            case 3:
                roadStyle.Line = new Pen(Color.Yellow, 5)
                {
                    PenStrokeCap = PenStrokeCap.Square,
                    StrokeJoin = StrokeJoin.Miter
                };
                break;
            default:
                roadStyle.Line = new Pen(Color.Gray, 5)
                {
                    PenStrokeCap = PenStrokeCap.Round,
                    PenStyle = PenStyle.ShortDash,
                    StrokeJoin = StrokeJoin.Round
                };
                break;
        }
        var feature = new GeometryFeature
        {
            Styles = new Style[]
            {
                roadStyle
            },
        };
        feature.Geometry = new LineString(roadPins.ToArray());
        tempRoad.Features.Add(feature);
        tempRoad.DataHasChanged();
    }

    private async void AddStartPin(string name, string description, Coordinate coordinate)
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
                Geometry = new Point(coordinate.X, coordinate.Y),
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
        await Task.Run(() =>
        {
            int _atlasBitmapId = 0;
            string imagePath = "MAUtour.";
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.default-pin.png");
            imagePath += "Resources.Images.default-image.jpg";
            //switch (typeofPin)
            //{
            //    case "Трасса":
            //        _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.race-pin.png");
            //        imagePath += "Resources.Images.race-image.jpg";
            //        break;
            //    case "Кемпинг":
            //        _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.green-pin.png");
            //        imagePath += "Resources.Images.green-image.jpg";
            //        break;
            //    case "Опасный участок":
            //        _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.danger-pin.png");
            //        imagePath += "Resources.Images.danger-image.png";
            //        break;
            //    case "Животные":
            //        _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.animal-pin.png");
            //        imagePath += "Resources.Images.animal-image.png";
            //        break;
            //    case "Красивое место":
            //        _atlasBitmapId = typeof(MapPage).LoadBitmapId("Resources.Images.landscape-pin.png");
            //        imagePath += "Resources.Images.landscape-image.png";
            //        break;
            //    default:

            //        break;
            //}
            var featureBitmapId = BitmapRegistry.Instance.Register(new Sprite(_atlasBitmapId, 0, 0, 21, 21, 1));
            SKBitmap resourceBitmap;
            using (Stream stream = assembly.GetManifestResourceStream(imagePath))
            {
                resourceBitmap = SKBitmap.Decode(stream);
            }
            var callbackImage = CreateCallbackImage("", "", resourceBitmap);
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

    private void OnPinClicked(object sender, PinClickedEventArgs e)
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

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}