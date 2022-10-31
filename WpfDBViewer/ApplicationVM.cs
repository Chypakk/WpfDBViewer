using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfDBViewer
{
    class ApplicationVM : INotifyPropertyChanged
    {

        private Phone selectedPhone;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Phone> Phones { get; set; }

        public Phone SelectedPhone
        {
            get { return selectedPhone; }
            set
            {
                selectedPhone = value;
                OnPropertyChanged("SelectedPhone");
            }
        }

        public ApplicationVM()
        {
            Phones = new ObservableCollection<Phone>
            {
                new Phone {Name="iPhone 7", Company="Apple", Price=56000 },
                new Phone {Name="Galaxy S7 Edge", Company="Samsung", Price =60000 },
                new Phone {Name="Elite x3", Company="HP", Price=56000 },
                new Phone {Name="Mi5S", Company="Xiaomi", Price=35000 }
            };
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
