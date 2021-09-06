using Isu.Tools;

namespace Isu.Entities
{
    public class CourseNumber
    {
        private int _course;
        public CourseNumber(int course)
        {
            Course = course;
        }

        public int Course
        {
            get
            {
                return _course;
            }
            set
            {
                if (value > 0 && value <= 6)
                {
                    _course = value;
                }
                else
                {
                    throw new IsuException("Error: incorrect number of course");
                }
            }
        }
    }
}
