using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Markup;
using Mapsui;
using Mapsui.Layers;
using Mapsui.UI.Maui;

namespace MAUtour;

public partial class MapPage : ContentPage
{
    private MapView view = new MapView();
    private Entry searchLabel;
    public MapPage()
    {
        InitializeComponent();

        searchLabel = new Entry { Text = "Search something" };
        var footerLaber = new Button { Text = "Add a pin" };
        view.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        footerLaber.Clicked += newPin;

        Content = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(20, GridUnitType.Auto)},
                new RowDefinition { Height = new GridLength(20, GridUnitType.Star)},
                new RowDefinition { Height = new GridLength(20, GridUnitType.Auto)}
            },
            Children = 
            {
                searchLabel.Row(0),
                view.Row(1),
                footerLaber.Row(2)
            }
        };

        view.PinClicked += View_SelectedPinChanged;
    }

    private void newPin(object sender, EventArgs e)
    {
        var latRand = new Random(10);
        var testPolyline = new Polyline()
        {
            Label = "testPolyline",
            StrokeColor = Color.FromRgba(223, 33, 255, 255),
            StrokeWidth = 3
        };
        var testPolygon = new Polygon()
        {
            Label = "testPolygon",
            StrokeColor = Color.FromRgba(22, 40, 160, 255),
            FillColor = Color.FromRgba(20, 40, 180, 128)
        };
        for (int i = 0; i < 1000; i++)
        {
            var pin = new Pin()
            {
                Position = new Position(latRand.NextDouble() * 30, latRand.NextDouble() * 30),
                Label = "test" + i.ToString(),
                IsVisible = true,
                MinVisible = 0.5,
                Tag = "test",
                Address = "TestAddress",
                Color = Color.FromRgba(i*3,i,i*6,255)
            };
            view.Pins.Add(pin);
        }

        testPolyline.Positions.Add(new Position(latRand.NextDouble() * 2, latRand.NextDouble() * 2));
        testPolyline.Positions.Add(new Position(latRand.NextDouble() * 2, latRand.NextDouble() * 2));
        testPolyline.Positions.Add(new Position(latRand.NextDouble() * 2, latRand.NextDouble() * 2));
        testPolyline.Positions.Add(new Position(latRand.NextDouble() * 2, latRand.NextDouble() * 2));

        testPolygon.Positions.Add(new Position(latRand.NextDouble() * 2, latRand.NextDouble() * 2));
        testPolygon.Positions.Add(new Position(latRand.NextDouble() * 2, latRand.NextDouble() * 2));
        testPolygon.Positions.Add(new Position(latRand.NextDouble() * 2, latRand.NextDouble() * 2));
        testPolygon.Positions.Add(new Position(latRand.NextDouble() * 2, latRand.NextDouble() * 2));
        testPolygon.Positions.Add(new Position(latRand.NextDouble() * 2, latRand.NextDouble() * 2));
        view.Drawables.Add(testPolyline);
        view.Drawables.Add(testPolygon);
        view.RefreshData();
    }

    private async void View_SelectedPinChanged(object sender, PinClickedEventArgs e)
    {
        searchLabel.Text = e.Pin.Label;
        await Shell.Current.GoToAsync("pin/details");
        //await DisplayAlert(e.SelectedPin.Address, $"You just clicked a {e.SelectedPin.Label} pin.", "Ok");
    }
}