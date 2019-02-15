using System;
using System.Collections.Generic;
using System.Text;
using MultiStoryMultiElivator.Models;
namespace MultiStoryMultiElivator.Services
{
    public interface IElevatorService
    {
        int? TimetoReachDestinationFloor(Elevator elevator, Floor CurrentFloor,Floor Destination);
    }
}
