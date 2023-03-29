using Mapsui.Styles;
using Mapsui.Widgets;

namespace MAUtour
{
    public class CustomWidget : IWidget
    {
        public Mapsui.Widgets.HorizontalAlignment HorizontalAlignment { get; set; }
        public Mapsui.Widgets.VerticalAlignment VerticalAlignment { get; set; }
        public float MarginX { get; set; } = 20;
        public float MarginY { get; set; } = 20;
        public Mapsui.MRect? Envelope { get; set; }
        public bool HandleWidgetTouched(Mapsui.INavigator navigator, Mapsui.MPoint position)
        {
            navigator.CenterOn(0, 0);
            return true;
        }

        public Mapsui.Styles.Color? Color { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
