using Microsoft.Expression.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Threading;
using Microsoft.Expression.Media;
using System.Windows.Threading;
using System.Media;

namespace WPFChallenge
{
    /// <summary>
    /// Interaction logic for OptionCircle.xaml
    /// </summary>
    public partial class OptionCircle : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private System.Timers.Timer _timer;

        private string _selectedOption;
        public string SelectedOption
        {
            private set
            {
                _selectedOption = value;
                RefreshSurroundingArcs(Options.IndexOf(value));
                OnPropertyChanged("SelectedOption");
            }
            get
            {
                return _selectedOption;
            }
        }

        private List<string> _options;
        public List<string> Options
        {
            set
            {
                _options = value.Select(x => x.ToUpper()).ToList();
                SelectedOption = _options.First();
            }
            get
            {
                return _options;
            }
        }

        public OptionCircle()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void btnCircle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += new ElapsedEventHandler(ChangeSelectedOption);
            _timer.Enabled = true;
            _timer.Start();
        }

        private void btnCircle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private void ChangeSelectedOption(object sender, ElapsedEventArgs e)
        {
            var currentIndex = Options.IndexOf(SelectedOption);
            SelectedOption = currentIndex == Options.Count - 1 ? Options.First() : Options[currentIndex + 1];
        }

        public void GetRandomValue()
        {
            var rnd = new Random();
            var count = 0;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                count++;
                SelectedOption = Options[rnd.Next(Options.Count)];
                if (count == 27)
                {
                    timer.Stop();
                }
            };
        }

        private void RefreshSurroundingArcs(int p)
        {
            Thread t = new Thread(RefreshArcsThreadSafe);
            t.SetApartmentState(ApartmentState.STA);

            t.Start();
        }

        private void RefreshArcsThreadSafe(object obj)
        {
            this.Dispatcher.BeginInvoke((ThreadStart)delegate
            {
                selectedPathFigureCollection.Clear();
                pathFigureCollection.Clear();

                Point center = new Point(79, 77);
                double radius = 65;

                var angleSize = 360 / Options.Count;
                var selectedOptionIndex = Options.IndexOf(SelectedOption);

                // Iterate each option and calculate begin and end angle
                for (int i = 0; i < Options.Count; i++)
                {
                    var endAngle = angleSize * i + angleSize;

                    var startPointX = center.X + radius * Math.Cos(angleSize * i * Math.PI / 180);
                    var startPointY = center.Y + radius * Math.Sin(angleSize * i * Math.PI / 180);
                    var startPoint = new Point(startPointX, startPointY);

                    // Iterate each option and generate points to be connected between the begin and end angles
                    for (int j = angleSize * i; j < endAngle - 10; j++)
                    {
                        var arcSeg = new ArcSegment();
                        arcSeg.SweepDirection = SweepDirection.Clockwise;


                        var nextPointX = center.X + radius * Math.Cos(j * Math.PI / 180);
                        var nextPointY = center.Y + radius * Math.Sin(j * Math.PI / 180);
                        var nextPoint = new Point(nextPointX, nextPointY);

                        arcSeg.Point = nextPoint;
                        arcSeg.Size = new Size(130, 130);

                        // Insert into the path figure for the selected option
                        if (i == selectedOptionIndex)
                        {
                            var selectedPathFigure = new PathFigure();
                            selectedPathFigure.StartPoint = startPoint;
                            selectedPathFigure.Segments.Add(arcSeg);
                            selectedPathFigureCollection.Add(selectedPathFigure);
                        }
                        // Insert into the path figure for the unselected options
                        else
                        {
                            var pathFigure = new PathFigure();
                            pathFigure.StartPoint = startPoint;
                            pathFigure.Segments.Add(arcSeg);
                            pathFigureCollection.Add(pathFigure);
                        }

                        startPoint = nextPoint;
                    }


                }
            });
        }

    }
}
