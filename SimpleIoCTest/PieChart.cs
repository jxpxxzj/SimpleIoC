using System;

namespace SimpleIoCTest
{
    public class PieChart : IChart
    {
        public void Draw()
        {
            Console.WriteLine("PieChart draw called.");
        }
    }
}
