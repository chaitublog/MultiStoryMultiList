using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

using MultiStoryMultiElivator.Models;
using MultiStoryMultiElivator.Services;

namespace MultiStoryMultiElivator
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()                
                .AddSingleton<IElevatorService, ElevatorService>()               
                .BuildServiceProvider();


            ElevatorManager elevatorFactory = new ElevatorManager(serviceProvider.GetService<IElevatorService>());
            int i = 10;
            while (i == 10)
            {

                #region scenarion1
                //////invalid test case
                Console.WriteLine("15, -2");
                Elevator selecedElevator = elevatorFactory.GetElevator(15, -2);



                Console.WriteLine("User Selected 0,8");
                elevatorFactory.GetElevator(0, 8);


                Thread.Sleep(6000);


                Console.WriteLine("User Selected 0,-2");
                elevatorFactory.GetElevator(0, -2);


                Console.WriteLine("User Selected 10,5");
                elevatorFactory.GetElevator(10, 5);

                Thread.Sleep(30000);
                elevatorFactory.SetLongStop(1);

                Console.WriteLine("User Selected 9,4");
                elevatorFactory.GetElevator(9, 4);

                #endregion scenarion1



                #region Scenario2
                elevatorFactory.Reset();

                 Console.WriteLine("Scenario 2 .. Starts");

                 Console.WriteLine("User Selected 0,7");
                 elevatorFactory.GetElevator(0, 7);
                 ////   DisplaySelectedElevator(elevatorFactory.GetElevator(0, 7));

                 Thread.Sleep(18000);

                 Console.WriteLine("User Selected 4,0");
                 elevatorFactory.GetElevator(4, 0);
                 //// DisplaySelectedElevator(elevatorFactory.GetElevator(4, 0));

                 Thread.Sleep(3000);

                 Console.WriteLine("User Selected 0,-2");
                 elevatorFactory.GetElevator(0, -2);
                ////  DisplaySelectedElevator(elevatorFactory.GetElevator(0, -2));

                Thread.Sleep(15000);
                elevatorFactory.SetLongStop(1);

                 //Console.WriteLine("User Selected 3,0. Since nearest Elevator stopped.. calculate best one.");

                 elevatorFactory.GetElevator(3, 0);
                 //// DisplaySelectedElevator(elevatorFactory.GetElevator(3, 0));


                 #endregion
                 

                string strInput = Console.ReadLine();
                
            }
        }
        static void DisplaySelectedElevator(Elevator selecedElevator)
        {
            if (selecedElevator != null)
            {
                Console.WriteLine("Choosen Elevator {0}", selecedElevator.ID.ToString());
            }
            else
            {
                Console.WriteLine("No Elevator Choosen.");
            }
        }
    }
}
