using System;
using System.Collections.Generic;
using System.Text;
using MultiStoryMultiElivator.Models;
namespace MultiStoryMultiElivator.Services
{
    interface IElevatorService
    {
        float? TimetoReachDestinationFloor(Elevator elevator, Floor Destination);
    }
}
