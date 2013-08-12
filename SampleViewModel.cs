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

        public SampleViewModel()
        {
            LeftBackground = brush1;
            RightBackground = brush1;
            AnswerBackground = brush1;
        }
        
        public async Task Update()
        {
            var generator = new Random();

            await updateLeft(generator);

            await updateRight(generator);

            await updateTotal(generator);

            await Task.Delay(1000);
            if (Answer != LeftOperand + RightOperand)
                throw new InvalidOperationException("This just failed");
        }

        private async Task updateTotal(Random generator)
        {
            messages.Add("Updating total: Starting");
            await Task.Delay(generator.Next(0, 1000));
            Answer = LeftOperand + RightOperand;
            AnswerBackground = switchBackground(AnswerBackground);
            this.PropertyChanged(this, new PropertyChangedEventArgs("AnswerBackground"));
            this.PropertyChanged(this, new PropertyChangedEventArgs("Answer"));
            messages.Add("Updating total: Finished");
        }

        private async Task updateRight(Random generator)
        {
            messages.Add("Calculating Right Operand: Starting");
            await Task.Delay(generator.Next(0,1000));
            RightOperand = generator.Next(-1000, 1000);
            RightBackground = switchBackground(RightBackground);
            this.PropertyChanged(this, new PropertyChangedEventArgs("RightBackground"));
            this.PropertyChanged(this, new PropertyChangedEventArgs("RightOperand"));
            messages.Add("Calculating Right Operand:  Finished");
        }

        private async Task updateLeft(Random generator)
        {
            messages.Add("Calculating Left Operand: Starting");
            await Task.Delay(generator.Next(0, 1000));
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
