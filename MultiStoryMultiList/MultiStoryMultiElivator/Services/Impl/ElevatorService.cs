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
     int AvgStopTime = 6;

     public  int? TimetoReachDestinationFloor(Elevator elevator,Floor CurrentFloor, Floor Destination)
     {
            ElevatorStatus userStatus = CurrentFloor.FloorNum > Destination.FloorNum ? ElevatorStatus.DOWN : ElevatorStatus.UP;

            if (elevator.Status != ElevatorStatus.LONGSTOP)
            {

                if(elevator.Status == userStatus && ((elevator.Status == ElevatorStatus.UP && elevator.CurrentFloor.FloorNum < CurrentFloor.FloorNum) || (elevator.Status == ElevatorStatus.DOWN && elevator.CurrentFloor.FloorNum > CurrentFloor.FloorNum)))
                {
                    return 0;
                }

                if (elevator.Status == ElevatorStatus.DOWN && Destination.FloorNum < elevator.CurrentFloor.FloorNum)
                {
                    return Math.Abs(Destination.FloorNum - elevator.CurrentFloor.FloorNum) * TravelTimebwFloor + (elevator.DownStops.Where(d => d < elevator.CurrentFloor.FloorNum && d > Destination.FloorNum)).Count() * AvgStopTime;
                }
                else if (elevator.Status == ElevatorStatus.DOWN && Destination.FloorNum > elevator.CurrentFloor.FloorNum)
                {
                    return (elevator.CurrentFloor.FloorNum - (elevator.DownStops.Count > 0 ? elevator.DownStops.Min() : elevator.CurrentFloor.FloorNum)) * TravelTimebwFloor + (Destination.FloorNum - (elevator.DownStops.Count() > 0 ? elevator.DownStops.Min() : elevator.CurrentFloor.FloorNum)) * TravelTimebwFloor
                             + elevator.DownStops.Count * AvgStopTime + elevator.UpStops.Where(u => u < Destination.FloorNum).Count() * AvgStopTime;
                }
                else if (elevator.Status == ElevatorStatus.UP && Destination.FloorNum > elevator.CurrentFloor.FloorNum)
                {
                    return Math.Abs(Destination.FloorNum - elevator.CurrentFloor.FloorNum) * TravelTimebwFloor + (elevator.UpStops.Where(d => d > elevator.CurrentFloor.FloorNum && d < Destination.FloorNum)).Count() * AvgStopTime;
                }
                else if (elevator.Status == ElevatorStatus.UP && Destination.FloorNum < elevator.CurrentFloor.FloorNum)
                {
                    return (elevator.UpStops != null ? elevator.UpStops.Max() : elevator.CurrentFloor.FloorNum - elevator.CurrentFloor.FloorNum) * TravelTimebwFloor + (elevator.UpStops.Max() - Destination.FloorNum) * TravelTimebwFloor + 
                        + elevator.UpStops.Count * AvgStopTime + elevator.DownStops.Where(u => u > Destination.FloorNum).Count() * AvgStopTime;
                }
                else if(elevator.Status == ElevatorStatus.STOP)
                {
                    return (Math.Abs(elevator.CurrentFloor.FloorNum - CurrentFloor.FloorNum) + Math.Abs(CurrentFloor.FloorNum - Destination.FloorNum)) * TravelTimebwFloor;
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
