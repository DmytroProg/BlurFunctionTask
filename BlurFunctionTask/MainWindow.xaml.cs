using GraphicsCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlurFunctionTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isPainting;
        private int _canvasWidth = 50;
        private int _canvasHeight = 40;
        private Rectangle _selectedBrushRect;
        private readonly Brush[] _brushes;
        private Rectangle[,] _dotsMap;

        public MainWindow()
        {
            InitializeComponent();
            _brushes = [
                Brushes.Red, 
                Brushes.Blue, 
                Brushes.Green, 
                Brushes.Yellow,
                Brushes.Purple,
                Brushes.Pink,
                Brushes.Orange,
                Brushes.White,
                Brushes.Black,
                Brushes.Gray,
                ];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCanvas();
            LoadColorsPanel();
        }

        private void LoadCanvas()
        {
            _dotsMap = new Rectangle[50, 40];

            for(int x = 0; x < _canvasWidth; x++)
            {
                for(int y = 0; y < _canvasHeight; y++)
                {
                    _dotsMap[x, y] = new Rectangle()
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,
                        StrokeThickness = .2,
                    };

                    canvas.Children.Add(_dotsMap[x, y]);

                    Canvas.SetLeft(_dotsMap[x, y], x * 10);
                    Canvas.SetTop(_dotsMap[x, y], y * 10);
                }
            }
        }

        private void LoadColorsPanel()
        {
            foreach (Brush brush in _brushes)
            {
                var rect = new Rectangle
                {
                    Width = 30,
                    Height = 30,
                    Fill = brush,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Margin = new Thickness(5)
                };
                rect.MouseUp += Rect_MouseUp;

                if (_selectedBrushRect == null)
                {
                    _selectedBrushRect = rect;
                    _selectedBrushRect.Stroke = Brushes.DarkGoldenrod;
                }

                colorsPanel.Children.Add(rect);
            }
        }

        private void Rect_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var rect = (sender as Rectangle);
            _selectedBrushRect.Stroke = Brushes.Black;
            _selectedBrushRect = rect;
            _selectedBrushRect.Stroke = Brushes.DarkGoldenrod;
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isPainting = true;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e) => PaintDot(e.GetPosition(canvas));

        private void PaintDot(Point position)
        {
            if (!_isPainting) return;

            var x = (int)(position.X / 10);
            var y = (int)(position.Y / 10);

            _dotsMap[x, y].Fill = _selectedBrushRect.Fill;
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PaintDot(e.GetPosition(canvas));
            _isPainting = false;
        }

        private void cleanBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach(var dot in _dotsMap)
            {
                dot.Fill = Brushes.White;
            }
        }

        private void blurBtn_Click(object sender, RoutedEventArgs e)
        {
            var pictureMatrix = new ColorData[_canvasWidth, _canvasHeight];

            for (int x = 0; x < _canvasWidth; x++)
            {
                for (int y = 0; y < _canvasHeight; y++)
                {
                    var color = (_dotsMap[x, y].Fill as SolidColorBrush).Color;
                    pictureMatrix[x, y] = new ColorData(color.R, color.G, color.B);
                }
            }

            try
            {
                var bluredPicture = GraphicsFunctions.BlurPicture(pictureMatrix, _canvasWidth, _canvasHeight);

                for (int x = 0; x < _canvasWidth; x++)
                {
                    for (int y = 0; y < _canvasHeight; y++)
                    {
                        var color = Color.FromRgb(bluredPicture[x, y].R, bluredPicture[x, y].G, bluredPicture[x, y].B);
                        _dotsMap[x, y].Fill = new SolidColorBrush(color);
                    }
                }
            }
            catch(Exception ex)
            {
                return;
            }
        }
    }
}