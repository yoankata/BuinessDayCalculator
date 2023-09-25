using WorkdayCalculatorLibrary;

namespace Tests.WorkdayCalculatorLibraryTests
{
    public class SetHolidayTests
    {
        [Test]
        public void SetHoliday_twice()
        {
            var workdayCalendar = new WorkdayCalendar();
            var holidayDate = DateTime.Today;

            workdayCalendar.SetHoliday(holidayDate);

            Assert.IsTrue(workdayCalendar.IsHoliday(holidayDate));
        }
    }
}