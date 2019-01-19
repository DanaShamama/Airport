using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirportClient.Models
{
    public class ViewStation : INotifyPropertyChanged
    {
        public bool IsAvailable { get; set; }

        public int Id { get; set; }
        private Visibility _visibility;
        public Visibility Visibility
        {
            get
            {
                if (IsAvailable)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
            set
            {
                _visibility = value;
                OnPropertyChanged("Visibility");
            }            
        }

        private string _currentFlightId;

        public string CurrentFlightId
        {
            get
            {
                if (_currentFlightId == null)
                {
                    return "";
                }
                return _currentFlightId;
            }
            set
            {
                _currentFlightId = value;
                OnPropertyChanged("CurrentFlightId");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// a handler to get the name of the property that changed in the view
        /// </summary>
        /// <param name="propertyName">string, the name of the property that have changed</param>
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewStation()
        {
            IsAvailable = true;
        }
    }
}
