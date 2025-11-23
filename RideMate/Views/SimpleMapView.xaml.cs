namespace RideMate.Views;

// Simple map view without external dependencies
public partial class SimpleMapView : ContentView
{
    public SimpleMapView()
    {
        InitializeComponent();
    }

    // Add a pin to the map at specific coordinates
    public void AddPin(string label, double x, double y, Color color)
    {
        var pin = new Label
        {
            Text = "📍",
            FontSize = 30,
            TextColor = color
        };

        var labelText = new Label
        {
            Text = label,
            FontSize = 12,
            TextColor = Colors.Black,
            BackgroundColor = Colors.White,
            Padding = 5
        };

        var stack = new VerticalStackLayout
        {
            Spacing = 0,
            Children = { pin, labelText }
        };

        AbsoluteLayout.SetLayoutBounds(stack, new Rect(x, y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        MapContainer.Children.Add(stack);
    }

    // Draw a line between two points
    public void DrawRoute(double x1, double y1, double x2, double y2, Color color)
    {
        var line = new BoxView
        {
            Color = color,
            WidthRequest = CalculateDistance(x1, y1, x2, y2),
            HeightRequest = 4,
            Rotation = CalculateAngle(x1, y1, x2, y2)
        };

        AbsoluteLayout.SetLayoutBounds(line, new Rect(x1, y1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        MapContainer.Children.Add(line);
    }

    // Clear all pins and routes
    public void ClearMap()
    {
        MapContainer.Children.Clear();
    }

    // Helper: Calculate distance between two points
    private double CalculateDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }

    // Helper: Calculate angle between two points
    private double CalculateAngle(double x1, double y1, double x2, double y2)
    {
        return Math.Atan2(y2 - y1, x2 - x1) * 180 / Math.PI;
    }
}
