using WorkdayCalculatorLibrary.Interfaces;

namespace WorkdayCalculatorLibrary
{
    internal class WorkdayStartAndStop
    {
        public TimeOnly StartTime { get; }
        public TimeOnly StopTime { get; }
        public TimeSpan WorkdayLength => StopTime - StartTime;
        public WorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            if (startHours < 0 || startMinutes < 0 || stopHours < 0 || stopMinutes < 0
                || startHours > 23 || startMinutes > 59 || stopHours > 23 || stopMinutes > 59)
            {
                throw new InvalidDataException($"Hours should be between 0 and 23 and minutes should be between 0 and 59!");
            }

            if (startHours > stopHours || (startHours == stopHours && startMinutes > stopMinutes))
            {
                throw new InvalidDataException($"Starting hours should be after stop hours!");
            }

            StartTime = new TimeOnly(startHours, startMinutes);
            StopTime = new TimeOnly(stopHours, stopMinutes);
        }
    }

    public class WorkdayCalendar : IWorkdayCalendar
    {
        HashSet<DateTime> Holidays { get; } = new HashSet<DateTime>();
        HashSet<Tuple<int, int>> RecurringHolidays { get; } = new HashSet<Tuple<int, int>>(); // month, day pair
        WorkdayStartAndStop WorkdayStartStop { get; set; } = new WorkdayStartAndStop(9, 0, 17, 0);
        public bool IsHoliday(DateTime date)
        {
            return Holidays.Contains(date.Date) 
                || RecurringHolidays.Contains(new Tuple<int, int>(date.Month, date.Day)) 
                || date.DayOfWeek.Equals(DayOfWeek.Saturday) 
                || date.DayOfWeek.Equals(DayOfWeek.Sunday);
        }        

        public bool IsWorkday(DateTime date)
        {
            return !IsHoliday(date) ;
        }

        private DateTime IncrementDay(DateTime currentDay, decimal increment)
        {
            return increment > 0 ? currentDay.AddDays(1) : currentDay.AddDays(-1);
        }

        public void SetHoliday(DateTime dateTime)
        {
            Holidays.Add(dateTime.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            if (month < 1 || day < 1 || month > 12 || day > 31)
            {
                throw new InvalidDataException($"Month should be between 1 and 12 and day should be between 1 and 31!");
            }

            RecurringHolidays.Add(new Tuple<int, int>(month, day));
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            WorkdayStartStop = new WorkdayStartAndStop(startHours, startMinutes, stopHours, stopMinutes);
        }

        public DateTime AddWorkdayFraction(DateTime date, decimal fraction)
        {
            if (fraction > 1 || fraction < -1)
                throw new ArgumentOutOfRangeException($"Argument should be a fraction!");

            var workdayFraction = WorkdayStartStop.WorkdayLength * (double)fraction;

            if (fraction > 0)
            {
                if (date.TimeOfDay > WorkdayStartStop.StopTime.ToTimeSpan())
                    date = IncrementDay(date, fraction); // add a day since we wrap around

                date = date.Date + WorkdayStartStop.StartTime.ToTimeSpan(); // reset start of last work day to start of day
            }
            else // fraction is negative
            {
                if (date.TimeOfDay.Add(workdayFraction) < WorkdayStartStop.StartTime.ToTimeSpan())
                    date = IncrementDay(date, fraction); // add a day since we wrap around

                date = date.Date + WorkdayStartStop.StopTime.ToTimeSpan(); // reset start of last work day to start of day
            }

            date = date.Add(workdayFraction); //add the fraction of day

            return date;
        }

        public DateTime GetWorkdayIncrement(DateTime startDate, decimal increment)
        {

            var wholedays = Math.Abs(decimal.Truncate(increment));
            var workayfraction = increment >= 0 ? increment - wholedays : increment + wholedays;

            // DateTime can only handle dates between 1.1.0001 г. 0:00:00 31.12.9999 г. 23:59:59 which means that
            // we can only add 2,913,271.07:41:54.2447492 or subtract days 738,787.16:18:05.7455014 days
            // and we don't want to rewrite the whole DateTime functionality from scratch we can only accomodate values in that range
            // this means no loss of magnitude or overflow can occur when converting decimal to double for the whole day part
            if (increment > 738787M || increment < -2913271M)
            {
                throw new ArgumentOutOfRangeException($"We cannot handle increments smaller than -738,787 and larger than 2,913,271 due to overflow!");
            }

            var currentDay = startDate;
            while( wholedays > 0 )
            {
                var nextDay = IncrementDay(currentDay, increment);

                if (IsWorkday(nextDay))
                { 
                    wholedays--;
                }

                currentDay = nextDay;
            }
            
            if(workayfraction != 0)
            {            
                currentDay = AddWorkdayFraction(currentDay, workayfraction);

                while (IsHoliday(currentDay)) 
                {
                    currentDay = IncrementDay(currentDay, increment);
                };
            }

            return currentDay;

        }
    }
}