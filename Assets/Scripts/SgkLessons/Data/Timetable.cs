using System.Collections.Generic;

namespace SgkLessons.Data
{
    [System.Serializable]
    public struct Timetable
    {
        public string date;
        public List<Lesson> lessons;
    }
}
