using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Markup;
using Mapsui;
using Mapsui.Layers;
using Mapsui.UI.Maui;
using Mapsui.Rendering.Skia;
using Microsoft.Maui.Animations;

namespace MAUtour;

public partial class MapPage : ContentPage
{
    private MapView view = new MapView();
    private Entry searchLabel;
    public MapPage()
    {
        InitializeComponent();

        searchLabel = new Entry { Text = "Search something" };
        var addButton = new Button { Text = "Add a pin" };
        var legendView = new ScrollView
        {
            Margin = new Thickness(20),
            Content = GetView()
        };
        view.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        addButton.Clicked += newPin;
        view.PinClicked += View_SelectedPinChanged;
        
        Content = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(20, GridUnitType.Auto)},
                new RowDefinition { Height = new GridLength(20, GridUnitType.Star)},
                new RowDefinition { Height = new GridLength(20, GridUnitType.Auto)},
                new RowDefinition { Height = new GridLength(150, GridUnitType.Absolute)}
            },
            Children = 
            {
                searchLabel.Row(0),
                view.Row(1),
                addButton.Row(2),
                legendView.Row(3)
            }
        };

    }
    private View GetView()
    {
        DataTemplate dataTemplate = new DataTemplate(() =>
        {
            BoxView boxView = new BoxView
            {
                HeightRequest = 32,
                WidthRequest = 32,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(5),
            };
            boxView.SetBinding(BoxView.ColorProperty, "Color");

            Label label = new Label
            {
                FontSize = 24,
                TextColor = Color.FromArgb("#FFFFFF"),
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(5),
            };
            label.SetBinding(Label.TextProperty, "Label");

            Label label1 = new Label
            {
                FontSize = 24,
                TextColor = Color.FromArgb("#FFFFFF"),
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(5),
            };
            label1.SetBinding(Label.TextProperty, "Position");

            StackLayout horizontalStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            horizontalStackLayout.Add(boxView);
            horizontalStackLayout.Add(label);
            horizontalStackLayout.Add(label1);
            return horizontalStackLayout;
        });
        StackLayout stackLayout = new StackLayout();
        BindableLayout.SetItemsSource(stackLayout, view.Pins);
        BindableLayout.SetItemTemplate(stackLayout, dataTemplate);
        return stackLayout;
    }
    private void newPin(object sender, EventArgs e)
    {
        var latRand = new Random(10);
        for (int i = 0; i < 5; i++)
        {
            var pin = new Pin()
            {
                Position = new Position(latRand.NextDouble() * 30, latRand.NextDouble() * 30),
                Label = "test" + i.ToString(),
                IsVisible = true,
                MinVisible = 0.5,
                Tag = "test",
                Address = "TestAddress",
                Color = Color.FromRgba(i * 50, i, i * 60, 255)
            };
            view.Pins.Add(pin);
        }
        
        view.RefreshData();
    }

    private async void View_SelectedPinChanged(object sender, PinClickedEventArgs e)
    {
        searchLabel.Text = e.Pin.Label;
        await Shell.Current.GoToAsync("pin/details");
        //await DisplayAlert(e.SelectedPin.Address, $"You just clicked a {e.SelectedPin.Label} pin.", "Ok");
    }
}