using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MultiStoryEf.Calc.Models;

namespace MultiStoryEf.Calc
{
    public class ElevatorManager
    {
        Building building = new Building();
        private int NumOfElevators = 3;
        Elevator[] elevators = new Elevator[3];


        public ElevatorManager()
        {
            for (int nele = 0; nele < NumOfElevators; nele++)
            {
                Elevator elevator = new Elevator();
                elevator.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(elevaor_PropertyChanged);
                elevator.ID = nele + 1; elevator.Status = ElevatorStatus.STOP;
                elevators[nele] = elevator;
                building.floors[2].elevators.Add(elevator);

            }

        }


        static void elevaor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //   Console.WriteLine("Value of property {0} was changed! New value is {1}");
        }

        public void Reset()
        {
            building.floors.ForEach(f => f.elevators.Clear());

            foreach (var element in elevators)
            {
                element.Status = ElevatorStatus.STOP;
                building.floors[0].elevators.Add(element);
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
                    }
                }

            });


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

        public Elevator getElevatorToUserFloor(int userFloor, ElevatorStatus elevatorStatus)
        {
            //User input floor validation           
            List<ValidationResult> destRes = ValidateFloor(new Floor(userFloor));
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

        ElevatorStatus GetMoveDirection(Elevator selecedElevator, int startFloorNum, int endFloorNum)
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

        static List<ValidationResult> ValidateFloor(Floor floor)
        {
            bool validateAllProperties = false;

            var results = new List<ValidationResult>();
            var destRes = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(floor,
                                                        new ValidationContext(floor, null, null),
                                                        results,
                                                        validateAllProperties);


            return results;

        }

        public void Display()
        {
            building.floors.ForEach(f =>
            {
                if (f.elevators.Count > 0)
                {
                    List<Elevator> display = f.elevators.ToList<Elevator>();
                    display.ForEach(d => Console.WriteLine("Elevator {0} @ floor {1} Status {2}", d.ID , f.FloorNum,d.Status));

                }

            });
        }
    
       

    }

    class ElevatorNumFloor
    {
        public int FloorNum { get; set; }
        public int FloorDiff { get; set; }
    }
}
