using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace MultiStoryMultiElivator.Models
{
    public class Elevator : INotifyPropertyChanged
    {
        private Floor _CurrentFloor = null;
        private ElevatorStatus _Status = ElevatorStatus.STOP;
      
        public int ID { get; set; }
        public List<int> UpStops { get; set; }
        public List<int> DownStops { get; set; }

        public Floor CurrentFloor {
            get
            {
                return this._CurrentFloor;
            }

            set
            {
                if (value != this._CurrentFloor)
                {
                    this._CurrentFloor = value;                    
                    NotifyPropertyChanged("CurrentFloor");
                   
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
                Console.WriteLine("Elevator "+ this.ID.ToString() + " Position :"+ this.CurrentFloor.FloorNum + ":" + this.Status);
                               
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
