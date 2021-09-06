using System;
using System.Collections.Generic;
using System.Text;

namespace Isu.Entities
{
    public class CourseNumber
    {
        public CourseNumber(int number) 
        {
            Number = number;
        }

        public int Number
        {
            get
            {
                return Number;
            }
       
            set 
            {
                if (value > 0 && value <= 6) 
                {
                    Number = value;
                }
            } 
        }
    }
}
