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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFChallenge
{
    /// <summary>
    /// Interaction logic for OptionCircle.xaml
    /// </summary>
    public partial class OptionCircle : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Timer _timer;

        private string _selectedOption;
        public string SelectedOption
        {
            private set
            {
                _selectedOption = value;
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
            _timer = new Timer(1000);
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

        public void GetRandomValue() { 
            var rnd = new Random();
            SelectedOption = Options[rnd.Next(Options.Count)];
        }

    }
}
