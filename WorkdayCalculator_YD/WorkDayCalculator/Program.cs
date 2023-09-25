// See https://aka.ms/new-console-template for more information
using WorkdayCalculatorLibrary;
using WorkdayCalculatorLibrary.Interfaces;

Console.WriteLine("Hello, Calculists!");

IWorkdayCalendar calendar = new WorkdayCalendar();

calendar.SetWorkdayStartAndStop(8, 0, 16, 0);
calendar.SetRecurringHoliday(5, 17);
calendar.SetHoliday(new DateTime(2004, 5, 27));

string format = "dd-MM-yyyy HH:mm";

var start = new DateTime(2004, 5, 24, 18, 5, 0);
decimal increment = -5.5m;

var incrementedDate = calendar.GetWorkdayIncrement(start, increment);

Console.WriteLine(start.ToString(format) + " with an addition of " + increment + " work days is " + incrementedDate.ToString(format));
// should output: 
// 24-05-2004 18:05 with an addition of -5.5 work days is 14-05-2004 12:00 
