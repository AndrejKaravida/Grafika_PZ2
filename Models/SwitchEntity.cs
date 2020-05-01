using System.Windows.Media;

namespace PZ2.Models
{
    public class SwitchEntity : PowerEntity
    {
        public SwitchEntity()
        {
            Color = Brushes.BlueViolet;
        }

        private string status;

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }
    }
}
