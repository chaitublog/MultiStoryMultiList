using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiStoryMultiElivator.Models;
using MultiStoryMultiElivator.Services;
using System.Threading;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MultiStoryMultiElivator
{
   public class ElevatorManager
    {
        static  Dictionary<int, Elevator> elevators = new Dictionary<int, Elevator>();
        Building building = new Building();

        private readonly IElevatorService elevatorService;
        private int NumOfElevators = 3;
        public ElevatorManager(IElevatorService elevatorService)
        {
             this.elevatorService = elevatorService;

            for(int nele=0;nele<NumOfElevators;nele++)
            {
                Elevator elevator = new Elevator();
                elevator.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(elevaor_PropertyChanged);
                elevator.ID = nele+1; elevator.CurrentFloor = new Floor(0); elevator.Status = ElevatorStatus.STOP; elevator.UpStops = new List<int>(); elevator.DownStops = new List<int>();
                elevators.Add(nele, elevator);
            }

         }

        public void Reset()
        {
            foreach (var element in elevators)
            {
                Elevator currElevator = element.Value as Elevator;
                currElevator.CurrentFloor.FloorNum = 0;
                currElevator.Status = ElevatorStatus.STOP;
                currElevator.UpStops = new List<int>();
                currElevator.DownStops = new List<int>();
            }
        }

        public void SetLongStop(int EleID)
        {
            elevators[EleID].Status = ElevatorStatus.LONGSTOP;
        }

        static void elevaor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {            
         //   Console.WriteLine("Value of property {0} was changed! New value is {1}");
        }

        public Elevator GetElevator(int StartFloorNum, int DestFloorNum)
        {
            //User input floor validation
            List<ValidationResult> results = ValidateFloor(new Floor(StartFloorNum));
            List<ValidationResult> destRes = ValidateFloor(new Floor(DestFloorNum));
            if (results.Count>0 || destRes.Count>0)
            {
                results.ForEach(r => Console.WriteLine(r.ErrorMessage));
                destRes.ForEach(r => Console.WriteLine(r.ErrorMessage));
                return null;
            }

            //Calc Whic lift serve quickly for user request
            List <Tuple<int,Elevator>> timecalc = new List<Tuple<int, Elevator>>();
            Elevator selecedElevator = null;
            foreach (var element in elevators)
            {               
               Elevator currElevator= element.Value as Elevator;
               int? calcTime= elevatorService.TimetoReachDestinationFloor(currElevator,new Floor(StartFloorNum), new Floor(DestFloorNum));

                if (calcTime.HasValue)
                {
                    if (calcTime.Value == 0)
                    {
                        //  selecedElevator.Stops.Add(DestFloorNum);
                        //selecedElevator = 
                            return currElevator;
                        
                    }
                    timecalc.Add(new Tuple<int,Elevator>(calcTime.Value, currElevator));
                }

            }

            Tuple<int, Elevator> mincalc= timecalc.OrderBy(e => e.Item1).First();
            selecedElevator= mincalc.Item2 as Elevator;

            Console.WriteLine("Choosen Elevator {0}", selecedElevator.ID.ToString());

            

            selecedElevator.Status = GetMoveDirection(selecedElevator, StartFloorNum, DestFloorNum);
            
            
            //if (StartFloorNum != selecedElevator.CurrentFloor.FloorNum) { selecedElevator.UpStops.Add(StartFloorNum); }

            //if (DestFloorNum != selecedElevator.CurrentFloor.FloorNum) { selecedElevator.DownStops.Add(DestFloorNum); }


            //DisplayElevatorPositions();


            return selecedElevator;



        }

        private static Timer timer = new Timer(OnEleMovement, null, 3000, 3000);

        private static void OnEleMovement(object state)
        {
            foreach (var element in elevators)
            {
                Elevator currElevator = element.Value as Elevator;
               
                if (currElevator.Status== ElevatorStatus.UP)
                {
                    currElevator.CurrentFloor=new Floor(currElevator.CurrentFloor.FloorNum +1);

                    var st = currElevator.UpStops.Where(s => s == currElevator.CurrentFloor.FloorNum);
                    if(st.Count()!=0)
                    {
                        currElevator.Status = ElevatorStatus.STOP;
                        currElevator.UpStops.Remove(currElevator.CurrentFloor.FloorNum);

                        if (currElevator.UpStops.Count == 0) {
                            currElevator.Status = ElevatorStatus.STOP;
                                DisplayElevatorPositions();
                        }
                    }

                }
                else if (currElevator.Status == ElevatorStatus.DOWN)
                {
                    currElevator.CurrentFloor =new Floor(currElevator.CurrentFloor.FloorNum - 1);
                    var st = currElevator.DownStops.Where(s => s == currElevator.CurrentFloor.FloorNum);
                    if (st.Count() != 0)
                    {
                        currElevator.Status = ElevatorStatus.STOP;
                        currElevator.DownStops.Remove(currElevator.CurrentFloor.FloorNum);
                        if (currElevator.DownStops.Count == 0) {
                            currElevator.Status = ElevatorStatus.STOP;
                            DisplayElevatorPositions();
                        }
                    }

                }
                else if (currElevator.Status == ElevatorStatus.STOP)
                {
                    
                    if (currElevator.UpStops.Count > 0 && currElevator.UpStops.FirstOrDefault() > currElevator.CurrentFloor.FloorNum)
                    {
                        currElevator.Status = ElevatorStatus.UP;
                    }
                    else if(currElevator.DownStops.Count > 0 && currElevator.DownStops.FirstOrDefault() < currElevator.CurrentFloor.FloorNum)
                    {
                        currElevator.Status = ElevatorStatus.DOWN;
                    }
                                       
                }
            }
        }

        static void DisplayElevatorPositions()
        {
            //Display starts
            foreach (var element in elevators)
            {
                Elevator currElevator = element.Value as Elevator;
                Console.WriteLine("Elevator {0} ; Floor {1} ; Status {2}", currElevator.ID.ToString(), currElevator.CurrentFloor.FloorNum.ToString(), currElevator.Status);
            }
            //Display ends
        }

        ElevatorStatus GetMoveDirection(Elevator selecedElevator, int StartFloorNum, int DestFloorNum)
        {
            if (selecedElevator.CurrentFloor.FloorNum == StartFloorNum && DestFloorNum > StartFloorNum)
            {
                selecedElevator.UpStops.Add(DestFloorNum);
                return ElevatorStatus.UP;
            }
            else if (selecedElevator.CurrentFloor.FloorNum == StartFloorNum && DestFloorNum < StartFloorNum)
            {
                selecedElevator.DownStops.Add(DestFloorNum);
                return ElevatorStatus.DOWN;

            }
            else if (selecedElevator.CurrentFloor.FloorNum < StartFloorNum)
            {
                selecedElevator.UpStops.Add(StartFloorNum);
                if (DestFloorNum < StartFloorNum)
                {
                    selecedElevator.DownStops.Add(DestFloorNum);
                }
                else
                {
                    selecedElevator.DownStops.Add(DestFloorNum);
                }
                return ElevatorStatus.UP;
            }
            else if (selecedElevator.CurrentFloor.FloorNum > StartFloorNum)
            {
                selecedElevator.DownStops.Add(StartFloorNum);
                if (DestFloorNum < StartFloorNum)
                {
                    selecedElevator.DownStops.Add(DestFloorNum);
                }
                else
                {
                    selecedElevator.DownStops.Add(DestFloorNum);
                }
                selecedElevator.DownStops.Add(DestFloorNum);

                return ElevatorStatus.DOWN;
            }
            else
                return ElevatorStatus.STOP;
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

    }
}
