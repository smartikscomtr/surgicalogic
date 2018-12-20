using System;

namespace Surgicalogic.Planning.Simulation
{
    class Program
    {
        Random rnd = new Random();

        static void Main(string[] args)
        {
            Random rnd = new Random();
            var operations = new int[] { 1, 2, 3, 4, 5 };
            var start = new int[] { 1, 4, 6, 7, 11 };
            var durations = new int[operations.Length];
            var resources = new int[] { 1, 2, 3 };
            var inUse = new int[resources.Length];
            var eventList = new int[operations.Length, 3];

            for (int i = 0; i < resources.Length; i++)
            {
                inUse[i] = 0;
            }

            for (int i = 0; i < operations.Length; i++)
            {
                eventList[i, 0] = operations[i];
                eventList[i, 1] = 1;
                eventList[i, 2] = start[i];
            }

            for (int i = 0; i < 10000; i++)
            {
                for (int t = 0; t < operations.Length; t++)
                {
                    durations[t] = rnd.Next(1, 10);
                }

                while(eventList.Length > 0)
                {



                    ReleaseReources(ref inUse);
                }
            }
        }

        public static void ReleaseReources(ref int[] inUse)
        {
            for (int i = 0; i < inUse.Length; i++)
            {
                inUse[i] = 0;
            }
        }
    }
}
