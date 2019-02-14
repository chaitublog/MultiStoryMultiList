using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


using MultiStoryMultiElivator.Models;
namespace MultiStoryMultiElivator.Services
{
    class ElevatorService:IElevatorService
    {
     int TravelTimebwFloor = 3;
     int AvgStopTime = 5;
      public  float? TimetoReachDestinationFloor(Elevator elevator,Floor Destination)
      {
            if(elevator.Status != ElevatorStatus.LONGSTOP)
            {
                if (elevator.Status == ElevatorStatus.DOWN && Destination.FloorNum < elevator.CurrentFloor)
                {
                  return  (Destination.FloorNum - elevator.CurrentFloor) + elevator.Stops.Count * AvgStopTime;
                }
                else if (elevator.Status == ElevatorStatus.DOWN && Destination.FloorNum > elevator.CurrentFloor)
                {
                    return  (elevator.CurrentFloor - elevator.Stops.Min())* TravelTimebwFloor + (Destination.FloorNum -elevator.Stops.Min())* TravelTimebwFloor + elevator.Stops.Count * AvgStopTime;
                }
                else if (elevator.Status == ElevatorStatus.UP && Destination.FloorNum > elevator.CurrentFloor)
                {
                    return  (Destination.FloorNum - elevator.CurrentFloor) * TravelTimebwFloor + elevator.Stops.Count * AvgStopTime;
                }
                else if (elevator.Status == ElevatorStatus.UP && Destination.FloorNum < elevator.CurrentFloor)
                {
                    return (elevator.Stops.Max() - elevator.CurrentFloor) * TravelTimebwFloor + (elevator.Stops.Max()-Destination.FloorNum) * TravelTimebwFloor + elevator.Stops.Count * AvgStopTime;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
            

      }

    }
}
