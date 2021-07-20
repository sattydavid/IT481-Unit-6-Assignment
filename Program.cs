using System;
using System.Diagnostics;
using System.Threading;

namespace David_Satterfield_Unit6_IT481
{

    class DressingRooms
    {

        private int roomNum; // number of rooms
        private int customNum; // number of customers
        private Thread[] customers;  // customer thread array
        private Semaphore semaphore; // semaphore : is used to lock room
        public static Random random = new Random();
        private long usingroomtime = 0;
        private int items = 0; // item number
        private long totalwaitingtime = 0; //total waiting time
        private int waitingcustomNum;       // number of waiting customer

        // constructor
        public DressingRooms()
        {
            roomNum = 3;

        }
        public DressingRooms(int roomN, int customN)
        {
            roomNum = roomN;
            customNum = customN;
            waitingcustomNum = customN;
        }
        public void setRoomNum(int num)
        {
            this.roomNum = num;
        }
        public void setCustomerNum(int num)
        {
            this.customNum = num;
        }

        // in this function, if room is empty use room and if not, wait till relese room
        public void RequestRoom()
        {

            Console.WriteLine(" {0} = waiting", Thread.CurrentThread.Name);
            semaphore.WaitOne();   //lock room
            Console.WriteLine(" {0} = dressing", Thread.CurrentThread.Name);
            int numNumberOfItems = random.Next(1, 6); // choose clothing item 
            var roomtimer = new Stopwatch(); // Timer to measure time when using room
            roomtimer.Start();
            items += numNumberOfItems;
            // dressing items
            while (numNumberOfItems > 0)
            {
                Thread.Sleep(random.Next(1000, 3000));
                numNumberOfItems--;
                //  Console.WriteLine(numNumberOfItems);
            }

            Console.WriteLine(" {0} = leave", Thread.CurrentThread.Name);
            roomtimer.Stop();
            long elapsed_time = roomtimer.ElapsedMilliseconds;
            // calculate waiting time, if current customer is bigger room number, it means that else man was waiting till release room.
            if (waitingcustomNum > roomNum)
                totalwaitingtime = totalwaitingtime + (waitingcustomNum - roomNum) * elapsed_time;
            waitingcustomNum--;
            usingroomtime += elapsed_time;
            semaphore.Release();
        }
        public void mainaction()
        {
            var totalTimer = new Stopwatch(); // to measure all time
            totalTimer.Start();
            this.semaphore = new Semaphore(roomNum, roomNum);
            this.customers = new Thread[customNum];
            //create and start customer threads
            for (int j = 0; j < this.customNum; j++)
            {
                customers[j] = new Thread(RequestRoom);
                customers[j].Name = "customer " + j;
                customers[j].Start();
            }
            int k = 0;
            //waiting until all thread end
            while (k < customNum)
            {
                customers[k].Join();
                k++;
            }
            totalTimer.Stop();
            //calculate all data
            double totaltime = totalTimer.ElapsedMilliseconds / 1000.0;
            double average_roomtime = usingroomtime / customNum / roomNum / 1000.0;
            double average_itemnumber = items / customNum;
            double average_waitingTime = totalwaitingtime / customNum / 1000.0;
            Console.WriteLine("The elapsed time is :{0} minutes ", totaltime);
            Console.WriteLine("The number of customers are :{0} ", customNum);
            Console.WriteLine("The average number of items is :{0} ", average_itemnumber);
            Console.WriteLine("average room time:{0} minutes", average_roomtime);
            Console.WriteLine("average waiting time:{0} minutes", average_waitingTime);
            Console.WriteLine(" ============================================================= ");
        }


    }

    class test
    {
        static void Main(String[] args)
        {
            DressingRooms test1 = new DressingRooms(2, 6);
            test1.mainaction();
            DressingRooms test2 = new DressingRooms(1, 2);
            test2.mainaction();
            DressingRooms test3 = new DressingRooms(3, 5);
            test3.mainaction();


        }
    }
}
