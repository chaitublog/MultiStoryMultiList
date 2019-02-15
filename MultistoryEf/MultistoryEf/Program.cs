using System;
using System.Threading;
using MultiStoryEf.Calc;
using MultiStoryEf.Calc.Models;

namespace MultistoryEf
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start.. ");
            ElevatorManager elevatorManager = new ElevatorManager();
            Elevator ele=  elevatorManager.getElevatorToUserFloor(0,ElevatorStatus.UP);
            Console.WriteLine("Travel from 0 to 7 floor");
            elevatorManager.traveltoDestFloor(0, 7, ele);
            Console.WriteLine(" ");


            Console.WriteLine("Travel from 0 to 6 floor");
            ele = elevatorManager.getElevatorToUserFloor(0, ElevatorStatus.UP);
            elevatorManager.traveltoDestFloor(0, 6, ele);
            Console.WriteLine(" ");


            Console.WriteLine("Travel from 6 to 4 floor");
            ele = elevatorManager.getElevatorToUserFloor(6, ElevatorStatus.DOWN);
            elevatorManager.traveltoDestFloor(6, 4, ele);
            Console.WriteLine(" ");

            elevatorManager.SetLongStop(2);

            Console.WriteLine("Travel from 0 to -2 floor");
            ele = elevatorManager.getElevatorToUserFloor(0, ElevatorStatus.DOWN);
            elevatorManager.traveltoDestFloor(0, -2, ele);
            Console.WriteLine(" ");
            elevatorManager.Display();
            Console.WriteLine(" ");

            Console.WriteLine("Travel from 3 to 0 floor");
            ele = elevatorManager.getElevatorToUserFloor(3, ElevatorStatus.DOWN);
            elevatorManager.traveltoDestFloor(3, 0, ele);

            Console.WriteLine("Final Status..");

            elevatorManager.Display();

            Console.WriteLine("Completed.. ");
            Console.ReadLine();
        }
    }
}
