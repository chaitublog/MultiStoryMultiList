using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace MultiStoryEf.Calc.Models
{
    public class Elevator : INotifyPropertyChanged
    {        
        private ElevatorStatus _Status = ElevatorStatus.STOP;
        public int ID { get; set; }
        public Elevator()
        {
        }

        public ElevatorStatus Status
        {
            get
            {
                return this._Status;
            }

            set
            {
                if (value != this._Status)
                {
                    this._Status = value;

                    NotifyPropertyChanged("Status");
                    // PropertyChanged(this, new PropertyChangedEventArgs("Status"));

                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                Console.WriteLine("Elevator " + this.ID.ToString() + ":" + this.Status);

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

