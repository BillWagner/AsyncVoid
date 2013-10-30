using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace AsyncVoid
{
    public class SampleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int LeftOperand { get; set; }
        public int RightOperand { get; set; }
        public int Answer { get; set; }
        public ObservableCollection<string> Messages { get { return messages; } }

        private ObservableCollection<string> messages = new ObservableCollection<string>();

        private readonly Brush brush1 = new SolidColorBrush(Colors.WhiteSmoke);
        private readonly Brush brush2 = new SolidColorBrush(Colors.Yellow);

        public Brush LeftBackground { get; set; }
        public Brush RightBackground { get; set; }
        public Brush AnswerBackground { get; set; }

        public int Progress { get; set; }
        public SampleViewModel()
        {
            LeftBackground = brush1;
            RightBackground = brush1;
            AnswerBackground = brush1;
        }
        
        public async void Update()
        {
            try
            {
                await UpdateImplAsync();
            }catch (Exception e)
            {
                messages.Add(e.ToString());
            }
        }

        private async Task UpdateImplAsync()
        {
            var generator = new Random();

            var reporter = new Progress<bool>((b) => 
            {
                Progress += 33;
                this.PropertyChanged(this, new PropertyChangedEventArgs("Progress"));
            });

            Progress = 0;
            this.PropertyChanged(this, new PropertyChangedEventArgs("Progress"));

            var leftTask = updateLeftAsync(generator, reporter);

            var rightTask = updateRightAsync(generator, reporter);

            await Task.WhenAll(leftTask, rightTask);

            await updateTotalAsync(generator, reporter);

            await Task.Delay(generator.Next(0, 2000));

            Progress = 100;
            this.PropertyChanged(this, new PropertyChangedEventArgs("Progress"));

            if (Answer != LeftOperand + RightOperand)
                throw new InvalidOperationException("This just failed");
        }

        private async Task updateTotalAsync(Random generator, IProgress<bool> reporter)
        {
            messages.Add("Updating total: Starting");
            await Task.Delay(generator.Next(0, 2000));
            reporter.Report(true);
            Answer = LeftOperand + RightOperand;
            AnswerBackground = switchBackground(AnswerBackground);
            this.PropertyChanged(this, new PropertyChangedEventArgs("AnswerBackground"));
            this.PropertyChanged(this, new PropertyChangedEventArgs("Answer"));
            messages.Add("Updating total: Finished");
        }

        private async Task updateRightAsync(Random generator, IProgress<bool> reporter)
        {
            messages.Add("Calculating Right Operand: Starting");
            await Task.Delay(generator.Next(0, 2000));
            reporter.Report(true);
            RightOperand = generator.Next(-1000, 1000);
            RightBackground = switchBackground(RightBackground);
            this.PropertyChanged(this, new PropertyChangedEventArgs("RightBackground"));
            this.PropertyChanged(this, new PropertyChangedEventArgs("RightOperand"));
            messages.Add("Calculating Right Operand:  Finished");
        }

        private async Task updateLeftAsync(Random generator, IProgress<bool> reporter)
        {
            messages.Add("Calculating Left Operand: Starting");
            await Task.Delay(generator.Next(0, 2000)).ConfigureAwait(false);
            reporter.Report(true);
            LeftOperand = generator.Next(-1000, 1000);
            LeftBackground = switchBackground(LeftBackground);
            this.PropertyChanged(this, new PropertyChangedEventArgs("LeftBackground"));
            this.PropertyChanged(this, new PropertyChangedEventArgs("LeftOperand"));
            messages.Add("Calculating Left Operand:  Finished");
        }

        private Brush switchBackground(Brush currentBrush)
        {
            if (currentBrush == brush1)
                return brush2;
            else
                return brush1;
        }
    }
}
