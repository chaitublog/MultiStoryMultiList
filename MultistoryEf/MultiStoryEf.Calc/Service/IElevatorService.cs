using System;
using System.Collections.Generic;
using System.Text;
using MultiStoryEf.Calc.Models;

namespace MultiStoryEf.Calc.Service
{
    public interface IElevatorService
    {
        void traveltoDestFloor(int userFloorNum, int destFloorNum, Elevator onBoardElevator);
        void SetLongStop(int EleID);
        Elevator getElevatorToUserFloor(int userFloor, ElevatorStatus elevatorStatus);
    }
}
