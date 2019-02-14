using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MultiStoryMultiElivator.Models
{
    public class Elevator : INotifyPropertyChanged
    {
        private int _CurrentFloor = 0;
        private ElevatorStatus _Status = ElevatorStatus.STOP;

        public int ID { get; set; }
        public int CurrentFloor {
            get
            {
                return this._CurrentFloor;
            }

            set
            {
                if (value != this._CurrentFloor)
                {
                    this._CurrentFloor = value;
                    NotifyPropertyChanged();
                }
            }
        }        
        public ElevatorStatus Status {
            get
            {
                return this._Status;
            }

            set
            {
                if (value != this._Status)
                {
                    this._Status = value;
                    NotifyPropertyChanged();
                }
            }

        }
        public List<int> Stops { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    public enum ElevatorStatus
    {
        UP,
        STOP,
        DOWN,
        LONGSTOP
    }
}
