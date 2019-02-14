using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MultiStoryMultiElivator.Models;
using MultiStoryMultiElivator.Services;

namespace MultiStoryMultiElivator
{
    class ElevatorFactory
    {
        Dictionary<int, Elevator> elevators = new Dictionary<int, Elevator>();
        // Constructor
        private readonly IElevatorService elevatorService;

        public ElevatorFactory(IElevatorService elevatorService)
        {
            this.elevatorService = elevatorService;

            elevators.Add(1, new Elevator() { ID=1, CurrentFloor=0, Status=ElevatorStatus.STOP });
            elevators.Add(2, new Elevator() { ID = 2, CurrentFloor = 0, Status = ElevatorStatus.STOP });
            elevators.Add(3, new Elevator() { ID = 3, CurrentFloor = 0, Status = ElevatorStatus.STOP });
        }



    }
}
