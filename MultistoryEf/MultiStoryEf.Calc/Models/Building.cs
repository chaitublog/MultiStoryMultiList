using System;
using System.Collections.Generic;
using System.Text;

namespace MultiStoryEf.Calc.Models
{
    public class Building
    {
        int NumFloors = 10;
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
                //_floors.Clear();
                if (_floors.Count == 0)
                {
                    for (int bs = -(NumBasemennts); bs < NumFloors + Math.Abs(NumBasemennts) - 1; bs++)
                    {
                        _floors.Add(new Floor(bs));
                    }
                }
                return this._floors;
            }


        }
    }
}
