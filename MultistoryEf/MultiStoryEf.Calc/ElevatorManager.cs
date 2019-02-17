using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MultiStoryEf.Calc.Models;
using MultiStoryEf.Calc.Service;

namespace MultiStoryEf.Calc
{
    public sealed class ElevatorManager
    {
        Building building = new Building();
        private int NumOfElevators = 3;
        Elevator[] elevators = new Elevator[3];
        
        IElevatorService _Elevatorservice;

        private static readonly Lazy<ElevatorManager> _elevatorManager =
        new Lazy<ElevatorManager>(() => new ElevatorManager());

        public static ElevatorManager getEleManager { get { return _elevatorManager.Value; } }

        private ElevatorManager()
        {
            this._Elevatorservice = new ElevatorService(building);

            for (int nele = 0; nele < NumOfElevators; nele++)
            {
                Elevator elevator = new Elevator();
                elevator.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(elevaor_PropertyChanged);
                elevator.ID = nele + 1; elevator.Status = ElevatorStatus.STOP;
                elevators[nele] = elevator;
                building.floors[2].elevators.Add(elevator);

            }

        }


        public void traveltoDestFloor(int userFloorNum, int destFloorNum, Elevator onBoardElevator)
        {
            _Elevatorservice.traveltoDestFloor(userFloorNum, destFloorNum, onBoardElevator);
        }

        public Elevator getElevatorToUserFloor(int userFloor, ElevatorStatus elevatorStatus)
        {
            return _Elevatorservice.getElevatorToUserFloor(userFloor, elevatorStatus);
        }

        public void SetElevatorToLongStop(int EleId)
        {
            _Elevatorservice.SetLongStop(EleId);
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
                building.floors[2].elevators.Add(element);
            }
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

            building.NotUsedElevators.ToList<Elevator>().ForEach(un => Console.WriteLine("Elevator {0} Stopped/ not in use",un.ID));
        }


    
}

    

    class ElevatorNumFloor
    {
        public int FloorNum { get; set; }
        public int FloorDiff { get; set; }
    }
}
