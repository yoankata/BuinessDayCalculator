using WorkdayCalculatorLibrary;

namespace Tests.WorkdayCalculatorLibraryTests
{
    public class SetRecurringHolidayTests
    {
        [Test]
        [TestCase(2, 1, "1-02-2004 18:05")]
        [TestCase(12, 3, "3-12-1204 18:05")]
        [TestCase(6, 4, "4-06-2004 18:09")]
        [TestCase(5, 24, "24-05-2024 17:05")]
        [Parallelizable(ParallelScope.All)]
        public void SetRecurringHoliday_sets_recurring_date(int month, int day, string dateStr)
        {
            var workdayCalendar = new WorkdayCalendar();

            workdayCalendar.SetRecurringHoliday(month, day);

            Assert.IsTrue(workdayCalendar.IsHoliday(DateTime.Parse(dateStr)));
        }

        [Test]
        [TestCase(6, 3, "2-02-2004 18:05")]
        [TestCase(11, 4, "3-11-1204 18:05")]
        [TestCase(5, 9, "5-06-2019 18:09")]
        [TestCase(2, 29, "24-05-2024 17:05")]
        [Parallelizable(ParallelScope.All)]
        public void SetRecurringHoliday_doesnt_sets_other_date(int month, int day, string dateStr)
        {
            var workdayCalendar = new WorkdayCalendar();

            workdayCalendar.SetRecurringHoliday(month, day);

            Assert.IsFalse(workdayCalendar.IsHoliday(DateTime.Parse(dateStr)));
        }

        [Test]
        [TestCase(-1, 2)]
        [TestCase(3, 40)]
        [TestCase(4, -6)]
        [TestCase(40, 3)]
        [Parallelizable(ParallelScope.All)]
        public void SetRecurringHoliday_throws_wrong_date(int month, int day)
        {
            var workdayCalendar = new WorkdayCalendar();

            Assert.Throws<InvalidDataException>(() => workdayCalendar.SetRecurringHoliday(month, day));
        }

    }
}