using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using System.Threading;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MultiStoryEf.Calc.Models;

namespace MultiStoryEf.Calc.Service
{
    public class ElevatorService : IElevatorService
    {
       Building building = null;
       public ElevatorService(Building building )
       {
            this.building = building;
       }

       public void traveltoDestFloor(int userFloorNum, int destFloorNum, Elevator onBoardElevator)
       {
            Floor presentFloor = building.floors.Find(x => x.FloorNum == userFloorNum);
            Floor destFloor = building.floors.Find(x => x.FloorNum == destFloorNum);
            onBoardElevator.Status = GetMoveDirection(onBoardElevator, userFloorNum, destFloorNum);
            if (presentFloor.elevators.Contains(onBoardElevator))
            {
                presentFloor.elevators.Remove(onBoardElevator);
                Thread.Sleep(Math.Abs(userFloorNum - destFloorNum) * 1000);
                destFloor.elevators.Add(onBoardElevator);
                onBoardElevator.Status = ElevatorStatus.STOP;
            }
       }

        public void SetLongStop(int EleID)
        {
            building.floors.ForEach(f =>
            {
                if (f.elevators.Count > 0)
                {
                    List<Elevator> display = f.elevators.ToList<Elevator>();
                    Elevator e = display.Find(d => d.ID == EleID);
                    if (e != null)
                    {
                        e.Status = ElevatorStatus.LONGSTOP;
                        f.elevators.Remove(e);
                        building.NotUsedElevators.Add(e);
                    }
                }

            });
        }

        public Elevator getElevatorToUserFloor(int userFloor, ElevatorStatus elevatorStatus)
        {
            //User input floor validation           
            List<ValidationResult> destRes = new Floor(userFloor).ValidateFloor().ToList<ValidationResult>();
            if (destRes.Count > 0)
            {
                destRes.ForEach(r => Console.WriteLine(r.ErrorMessage));
                return null;
            }
            Floor presentFloor = building.floors.Find(x => x.FloorNum == userFloor);

            //Calc Which lift serve quickly for user request
            Floor nearestEleFloor = ChooseElevator(presentFloor, elevatorStatus);
            Elevator ele = nearestEleFloor.elevators.FirstOrDefault();

            nearestEleFloor.elevators.Remove(ele);

            ele.Status = GetMoveDirection(ele, nearestEleFloor.FloorNum, userFloor);

            Thread.Sleep(Math.Abs(nearestEleFloor.FloorNum - userFloor) * 1000);

            presentFloor.elevators.Add(ele);

            return ele;
        }

        private Floor ChooseElevator(Floor start, ElevatorStatus elevatorStatus)
        {
            List<ElevatorNumFloor> diffFloors = new List<ElevatorNumFloor>();

            building.floors.ForEach(x =>
            {
                if (x.elevators.Count > 0)
                {
                    foreach (Elevator elevator in x.elevators)
                    {

                        if (elevator.Status != ElevatorStatus.LONGSTOP && (elevatorStatus == elevator.Status || elevator.Status == ElevatorStatus.STOP))
                        {
                            diffFloors.Add(new ElevatorNumFloor()
                            { FloorNum = x.FloorNum, FloorDiff = Math.Abs(x.FloorNum - start.FloorNum) });
                        }

                    }
                }
            }
            );

            var minDiff = diffFloors?.Min(x => x.FloorDiff);

            return building.floors.Where(f => f.FloorNum == diffFloors.First(x => x.FloorDiff == minDiff).FloorNum).FirstOrDefault();
        }

        private ElevatorStatus GetMoveDirection(Elevator selecedElevator, int startFloorNum, int endFloorNum)
        {
            if (endFloorNum > startFloorNum)
            {
                return ElevatorStatus.UP;
            }
            else if (endFloorNum < startFloorNum)
            {
                return ElevatorStatus.DOWN;
            }
            else
            {
                return ElevatorStatus.STOP;
            }
        }


    }

}
