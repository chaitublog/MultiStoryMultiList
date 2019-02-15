using System;
using System.Collections.Generic;
using System.Text;

namespace MultiStoryMultiElivator.Models
{
    public class Building
    {
        int NumFloors= 10;
        int NumBasemennts = 2;
        public Floor elevatorFloor { get; set; }
        public Building()
        {

        }

        private List<Floor> _floors = new List<Floor>();
        public List<Floor> floors
        {
            get
            {
               
                for (int bs = -(NumBasemennts); bs <= NumFloors + NumBasemennts; bs++)
                {
                    _floors.Add(new Floor(bs));
                }
                return this._floors;
            }
           

        }
    }
}
