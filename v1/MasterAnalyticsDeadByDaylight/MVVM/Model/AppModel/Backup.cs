using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    public class Backup : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isCheck;
        public bool IsCheck
        {
            get => _isCheck;
            set
            {
                if (_isCheck != value)
                {
                    _isCheck = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        private byte[] _statusImage;
        public byte[] StatusImage
        {
            get => _statusImage;
            set
            {
                if (_statusImage != value)
                {
                    _statusImage = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
