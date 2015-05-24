using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFChallenge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            OptionCircle1.Options = new List<string> { "one of a kind", "small batch", "large batch", "mass market" };
            OptionCircle2.Options = new List<string> { "savory", "sweet", "umami" };
            OptionCircle3.Options = new List<string> { "crunchy", "mushy", "smooth" };
            OptionCircle4.Options = new List<string> { "spicy", "mild" };
            OptionCircle5.Options = new List<string> { "a little", "a lot" };
            OptionCircle6.Options = new List<string> { "breakfast", "brunch", "lunch", "snack", "dinner" };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void btnShuffle_Click(object sender, RoutedEventArgs e)
        {
            // Create a thread
            var soundThread = new Thread(new ThreadStart(() =>
            {
                var mplayer = new MediaPlayer();
                mplayer.Open(new Uri(@"sounds/countdown.mp3", UriKind.Relative));
                mplayer.Play();

                System.Windows.Threading.Dispatcher.Run();
            }));
            soundThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
            soundThread.IsBackground = true;
            soundThread.Start();

            var optionCircles = GetLogicalChildCollection<OptionCircle>(mainGrid);
            foreach (var optionCircle in optionCircles)
            {
                optionCircle.GetRandomValue();
            }
        }

        private static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Great choice! Let's move on to the next screen.", "Confirmation", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
