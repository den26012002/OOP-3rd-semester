using Isu.Tools;

namespace Isu.Entities
{
    public class CourseNumber
    {
        private uint _course;
        private uint _maxCource = 6;
        public CourseNumber(uint course)
        {
            Course = course;
        }

        public uint Course
        {
            get
            {
                return _course;
            }
            set
            {
                if (value > _maxCource)
                {
                    throw new IsuException("Error: incorrect number of course");
                }

                _course = value;
            }
        }
    }
}
