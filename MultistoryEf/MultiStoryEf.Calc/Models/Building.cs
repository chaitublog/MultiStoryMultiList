using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MultiStoryEf.Calc.Models
{
    public class Building
    {
        int NumFloors = 10;
        int NumBasemennts = 2;
        public Floor elevatorFloor { get; set; }
        public List<Floor> floors = null;
        public Building()
        {
            floors = new List<Floor>();
                for (int bs = -(NumBasemennts); bs < NumFloors + Math.Abs(NumBasemennts) - 1; bs++)
                {
                    floors.Add(new Floor(bs));
                }
        }
        public ObservableCollection<Elevator> NotUsedElevators = new ObservableCollection<Elevator>();
       
        
    }
}
