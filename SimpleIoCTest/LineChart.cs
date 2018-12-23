using System;

namespace SimpleIoCTest
{
    public class LineChart : IChart
    {
        public void Draw()
        {
            Console.WriteLine("LineChart draw called.");
        }
    }
}
