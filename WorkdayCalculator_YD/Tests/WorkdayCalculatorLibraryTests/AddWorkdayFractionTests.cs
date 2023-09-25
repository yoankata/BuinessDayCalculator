using WorkdayCalculatorLibrary;

namespace Tests.WorkdayCalculatorLibraryTests
{

    public class AddWorkdayFractionTests
    {
        [Test]
        [TestCase(8, 0, 16, 0, "24-05-2004 07:05", 0.25, "24-05-2004 10:00")]
        [TestCase(8, 0, 16, 0, "24-05-2004 07:05", 0.75, "24-05-2004 14:00")]
        [TestCase(8, 0, 16, 0, "24-05-2004 07:05", -0.25, "23-05-2004 14:00")]
        [TestCase(8, 0, 16, 0, "24-05-2004 07:05", -0.75, "23-05-2004 10:00")]
        [TestCase(8, 0, 16, 0, "24-05-2004 18:05", 0.25, "25-05-2004 10:00")]
        [TestCase(8, 0, 16, 0, "24-05-2004 18:05", 0.75, "25-05-2004 14:00")]
        [TestCase(8, 0, 16, 0, "24-05-2004 18:05", -0.25, "24-05-2004 14:00")]
        [TestCase(8, 0, 16, 0, "24-05-2004 18:05", -0.75, "24-05-2004 10:00")]
        [TestCase(8, 0, 16, 0, "24-05-2004 12:05", 0.25, "24-05-2004 10:00")]
        [TestCase(8, 0, 16, 0, "24-05-2004 12:05", 0.75, "24-05-2004 14:00")] //
        [TestCase(8, 0, 16, 0, "24-05-2004 12:05", -0.25, "24-05-2004 14:00")]
        [TestCase(8, 0, 16, 0, "24-05-2004 12:05", -0.75, "23-05-2004 10:00")]
        [Parallelizable(ParallelScope.All)]
        public void AddWorkdayFraction_adds_fraction(int startHours, int startMinutes, int stopHours, int stopMinutes, string dateStr, double fraction, string expectedStr)
        {
            var workdayCalendar = new WorkdayCalendar();

            workdayCalendar.SetWorkdayStartAndStop(startHours, startMinutes, stopHours, stopMinutes);
            var result = workdayCalendar.AddWorkdayFraction(DateTime.Parse(dateStr), (decimal)fraction);
            Assert.That(result, Is.EqualTo(DateTime.Parse(expectedStr)).Within(1).Minutes);
        }

    }
}