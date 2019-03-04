using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;


namespace Commercial_Controller
{
    class Program
    {
        public static void Main(string[] args)
        {
            Controller controller = new Controller(85, 4, 20);
            controller.battery.ColumnList[1].ElevatorList[0].CurrentFloor = 2;  // going up to 24
            controller.battery.ColumnList[1].ElevatorList[0].Status = "MOVING";
            controller.battery.ColumnList[1].ElevatorList[0].Direction = "UP";
            controller.battery.ColumnList[1].ElevatorList[0].FloorList.Add(24);

            controller.battery.ColumnList[1].ElevatorList[1].CurrentFloor = 23; // going up to 28
            controller.battery.ColumnList[1].ElevatorList[1].Status = "MOVING";
            controller.battery.ColumnList[1].ElevatorList[1].Direction = "UP";
            controller.battery.ColumnList[1].ElevatorList[1].FloorList.Add(28);

            controller.battery.ColumnList[1].ElevatorList[2].CurrentFloor = 33; // going down to 1
            controller.battery.ColumnList[1].ElevatorList[2].Status = "MOVING";
            controller.battery.ColumnList[1].ElevatorList[2].Direction = "DOWN";
            controller.battery.ColumnList[1].ElevatorList[2].FloorList.Add(1);

            controller.battery.ColumnList[1].ElevatorList[3].CurrentFloor = 40; // going down to 24 then 1
            controller.battery.ColumnList[1].ElevatorList[3].Status = "MOVING";
            controller.battery.ColumnList[1].ElevatorList[3].Direction = "DOWN";
            controller.battery.ColumnList[1].ElevatorList[3].FloorList.Add(24);
            controller.battery.ColumnList[1].ElevatorList[3].FloorList.Add(1);


            controller.battery.ColumnList[1].ElevatorList[4].CurrentFloor = 42; // going down to 1
            controller.battery.ColumnList[1].ElevatorList[4].Status = "MOVING";
            controller.battery.ColumnList[1].ElevatorList[4].Direction = "DOWN";
            controller.battery.ColumnList[1].ElevatorList[4].FloorList.Add(1);
            //controller.RequestElevator(12, 1);
            controller.AssignElevator(36);

        }
    }

    // CLASS CONTROLLER ---------------------------------------------------------
    // Controller is the class that manages Columns, Elevators and the requests

    public class Controller
    {
        public int NumberOfFloors;
        public int NumberOfColumns;
        public int NumberOfElevators;
        public Battery battery;

        public Controller(int NumberOfFloors, int NumberOfColumns, int NumberOfElevators)
        {
            NumberOfColumns = NumberOfColumns;
            NumberOfElevators = NumberOfElevators;
            NumberOfFloors = NumberOfFloors;
            battery = new Battery(NumberOfColumns, NumberOfElevators, NumberOfFloors);

        }

        public Elevator RequestElevator(int FloorNumber, int RequestedFloor)
        {
            Console.WriteLine("Request elevator to floor : " + FloorNumber);
            Console.WriteLine("");
            Column column = battery.FindColumn(FloorNumber);
            Elevator elevator = column.FindElevator(FloorNumber);
            Console.WriteLine("Column #:" + column.ColumnNumber);
            elevator.SendRequest(FloorNumber);
            Console.WriteLine("CurrentFloor");
            Console.WriteLine(elevator.CurrentFloor);
            elevator.SendRequest(RequestedFloor);
            return elevator;
        }

        public Elevator AssignElevator(int RequestedFloor)
        {
            Console.WriteLine("Requested floor : " + RequestedFloor);
            Console.WriteLine("---------------------------------------------------");
            var column = battery.FindColumn(RequestedFloor);
            var elevator = column.FindElevator(RequestedFloor);
            elevator.SendRequest(RequestedFloor);
            return elevator;
        }
    }

    // CLASS BATTERY -----------------------------------------------------------


    public class Battery
    {
        public int NumberOfColumns;
        public List<Column> ColumnList;
        public int NumberOfElevators;
        public char ColumnNumber;
        public Battery(int NumberOfColumns, int NumberOfElevators, int NumberOfFloors)
        {
            NumberOfColumns = NumberOfColumns;
            ColumnList = new List<Column>();
            char letter = 'A';

            for (int i = 0; i < NumberOfColumns; i++, letter++)
            {
                Column column = new Column(letter, 5, 85);
                column.ColumnNumber = letter;
                ColumnList.Add(column);
            }
        }

        public Column FindColumn(int RequestedFloor)
        {
            Column selected = null;
            foreach (Column column in ColumnList)
            {
                if (RequestedFloor >= 2 || RequestedFloor <= 22 || RequestedFloor == 1)
                {
                    selected = ColumnList[0];
                }
                else if (RequestedFloor >= 23 && RequestedFloor <= 43 || RequestedFloor == 1)
                {
                    selected = ColumnList[1];
                }
                else if (RequestedFloor >= 44 && RequestedFloor <= 64 || RequestedFloor == 1)
                {
                    selected = ColumnList[2];
                }
                else if (RequestedFloor >= 65 && RequestedFloor <= 85 || RequestedFloor == 1)
                {
                    selected = ColumnList[3];
                }
            }
            return selected;
        }
    }


    // CLASS COLUMN -------------------------------------------------------

    public class Column
    {
        public List<Elevator> ElevatorList;
        public int ColumnNumber;

        public Column(char ColumnNumber, int NumberOfElevators, int NumberOfFloors)
        {
            ColumnNumber = ColumnNumber;
            ElevatorList = new List<Elevator>();

            for (int i = 0; i < NumberOfElevators; i++)
            {
                Elevator elevator = new Elevator(i, NumberOfFloors);
                ElevatorList.Add(elevator);
            }
        }


        public Elevator FindElevator(int RequestedFloor)
        {
            foreach (Elevator elevator in ElevatorList)
            {
                if (elevator.Status == "STOPPED" && elevator.CurrentFloor == 1)
                {
                    return elevator;
                }
                else if (elevator.Status == "IDLE" && elevator.CurrentFloor > RequestedFloor)
                {
                    return FindNearest(RequestedFloor);
                }
                else if (elevator.Status == "STOPPED" || elevator.Status == "MOVING" && elevator.CurrentFloor > RequestedFloor && elevator.Direction == "DOWN")
                {
                    return FindNearest(RequestedFloor);
                }
                else 
                {
                    return FindNearest(RequestedFloor);
                }
            }
            return null;
       }

        public Elevator FindNearest(int RequestedFloor)
        {
            var refgap = Math.Abs(ElevatorList[0].CurrentFloor - RequestedFloor);
            var refElevator = ElevatorList[0];

            foreach (Elevator elevator in ElevatorList)
            {
                if (elevator.Direction == "DOWN")
                {
                    var gap = Math.Abs(elevator.CurrentFloor - RequestedFloor);

                    if (gap <= refgap)
                    {
                        refgap = gap;
                        refElevator = elevator;

                    }

                }

            }
            return refElevator;
        }

    }

    // CLASS ELEVATOR -------------------------------------------------------

    public class Elevator
    {
        public int ElevatorNumber;
        public int CurrentFloor;
        public string Status;
        public List<int> FloorList;
        public string Direction;

        public Elevator(int ElevatorNumber, int NumberOfFloors)
        {
            ElevatorNumber = ElevatorNumber;
            Status = "IDLE";
            CurrentFloor = 1;
            FloorList = new List<int>();
            Direction = "UP";
        }

        public void SendRequest(int RequestedFloor)
        {
            Console.WriteLine("Send request");
            Console.WriteLine(RequestedFloor);
            FloorList.Add(RequestedFloor);
            if (RequestedFloor > CurrentFloor)
            {
                FloorList.Sort((a, b) => a.CompareTo(b));
            }
            else if (RequestedFloor < CurrentFloor)
            {
                FloorList.Sort((a, b) => -1 * a.CompareTo(b));
            }
            OperateElevator(RequestedFloor);
        }


        public void OperateElevator(int RequestedFloor)
        {
            while (FloorList.Count > 0)
            {
                if (RequestedFloor == CurrentFloor)
                {
                    OpenDoor();
                    FloorList.RemoveAt(0);
                    CloseDoor();
                }
                else if (RequestedFloor > CurrentFloor)
                {
                    MoveUp(RequestedFloor);
                    FloorList.RemoveAt(0);
                    CloseDoor();
                }
                else if (RequestedFloor < CurrentFloor)
                {
                    MoveDown(RequestedFloor);
                    FloorList.RemoveAt(0);
                    CloseDoor();
                }
            }
        }

        public void MoveUp(int RequestedFloor)
        {
            Console.WriteLine(" Elevator : #" + ElevatorNumber + "  Floor : " + CurrentFloor);
            while (RequestedFloor != CurrentFloor)
            {
                Status = "MOVING";
                Thread.Sleep(500);
                CurrentFloor += 1;
                Console.WriteLine(" Elevator : #" + ElevatorNumber + "  Floor : " + CurrentFloor);
            }
            Console.WriteLine("Arrived at floor ");
            Status = "STOPPED";
            OpenDoor();
        }

        public void MoveDown(int RequestedFloor)
        {
            Console.WriteLine(" Elevator : #" + ElevatorNumber + "  Floor : " + CurrentFloor);
            while (RequestedFloor != CurrentFloor)
            {
                Status = "MOVING";
                Thread.Sleep(500);
                CurrentFloor -= 1;
                Console.WriteLine(" Elevator : #" + ElevatorNumber + "  Floor : " + CurrentFloor);
            }
            Console.WriteLine("Arrived at floor ");
            Status = "STOPPED";
            OpenDoor();
        }

        public void OpenDoor()
        {
            Console.WriteLine(" Elevator : #" + ElevatorNumber + "  Floor : " + CurrentFloor + " DOOR IS OPENING");
            if (Status == "IDLE" || Status == "STOPPED")
            {
                Thread.Sleep(500);
            }
            Console.WriteLine("DOOR IS OPENED");
            Thread.Sleep(500);
        }

        public void CloseDoor()
        {
            Console.WriteLine(" Elevator : #" + ElevatorNumber + "  Floor : " + CurrentFloor + " DOOR IS CLOSING");
            if (Status == "IDLE" || Status == "STOPPED")
            {
                Thread.Sleep(500);
            }
            Console.WriteLine("DOOR IS CLOSED");
            Thread.Sleep(500);
        }
    }
}