using WorkdayCalculatorLibrary;

namespace Tests.WorkdayCalculatorLibraryTests
{
    public class GetWorkdayIncrementTests
    {
        [Test]
        [TestCase("24-05-2004 18:05", -5.5, "14-05-2004 12:00")]
        [TestCase("24-05-2004 07:05", -5.5, "13-05-2004 12:00")]
        [TestCase("24-05-2004 19:03", 44.723656, "27-07-2004 13:47")]
        [TestCase("24-05-2004 18:03", -6.7470217, "13-05-2004 10:02")]
        [TestCase("24-05-2004 08:03", 12.782709, "10-06-2004 14:16")] //14:18
        [TestCase("24-05-2004 07:03", 8.276628, "04-06-2004 10:12")]
        [Parallelizable(ParallelScope.All)]
        public void GetWorkdayIncrement(string startDateStr, double increment, string expectedStr)
        {
            var startDate = DateTime.Parse(startDateStr);
            var expected = DateTime.Parse(expectedStr);

            var workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkdayStartAndStop(8, 0, 16, 0);
            workdayCalendar.SetRecurringHoliday(5, 17);
            workdayCalendar.SetHoliday(new DateTime(2004, 5, 27));

            DateTime result = workdayCalendar.GetWorkdayIncrement(startDate, (decimal)increment);

            Assert.That(result, Is.EqualTo(expected).Within(1).Minutes);
        }

    }
}