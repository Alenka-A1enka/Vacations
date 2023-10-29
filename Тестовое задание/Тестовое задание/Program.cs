using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticTask1
{
    class Program
    {
        static void Main(string[] args)
        {
            var VacationDictionary = new Dictionary<string, List<DateTime>>()
            {
                ["Иванов Иван Иванович"] = new List<DateTime>(),
                ["Петров Петр Петрович"] = new List<DateTime>(),
                ["Юлина Юлия Юлиановна"] = new List<DateTime>(),
                ["Сидоров Сидор Сидорович"] = new List<DateTime>(),
                ["Павлов Павел Павлович"] = new List<DateTime>(),
                ["Георгиев Георг Георгиевич"] = new List<DateTime>()
            };
            
            List<DateTime> free_dates = new List<DateTime>(); // Свободные даты для отпуска. 
            var AviableWorkingDaysOfWeekWithoutWeekends = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            
            DateTime current_date = new DateTime(DateTime.Now.Year, 1, 1);
            int days_in_year = DateTime.IsLeapYear(current_date.Year) ? 366 : 365;

            // Добавление всех дат года в свободные даты. 
            for (int i = 0; i < days_in_year; i++)
            {
                free_dates.Add(current_date.AddDays(i));

            }


            // Список отпусков сотрудников
            List<DateTime> Vacations = new List<DateTime>();
            // Хранит отпуск сотрудника. 
            List<DateTime> dateList = new List<DateTime>();
            Random random = new Random();
            foreach (var VacationList in VacationDictionary)
            {
                dateList = VacationList.Value;
                int vacationCount = 28;

                while (vacationCount > 0)
                {
                    // Получаем случайную дату только из тех, что сейчас доступны. 
                    int index = random.Next(free_dates.Count);
                    var startDate = free_dates[index];

                    //берем только те дни для начала отпуска, которые являются буднями. 
                    if (AviableWorkingDaysOfWeekWithoutWeekends.Contains(startDate.DayOfWeek.ToString()))
                    {
                        int[] vacationSteps = { 7, 14 };
                        int vacIndex;
                        var endDate = new DateTime();
                        int difference = 0;

                        // Если осталось всего 7 дней отпуска. 
                        if (vacationCount <= 7)
                        {
                            endDate = startDate.AddDays(7);
                            difference = 7;
                        }
                        else
                        {
                            // Случайный выбор - 7 или 14 дней отпуска. 
                            vacIndex = vacationSteps[random.Next(vacationSteps.Length)];
                            endDate = startDate.AddDays(vacIndex);
                            difference = vacIndex;
                        }

                        // Проверка условий по отпуску
                        bool CanCreateVacation = false;
                        bool existStart = false;
                        bool existEnd = false;
                        //проверяем, что в одну дату отпуск только у одного сотрудника 
                        if (!Vacations.Any(element => element >= startDate && element <= endDate))
                        {
                            //проверка, что разница между всеми отпусками составляет более 3 дней. 
                            if (!Vacations.Any(element => element.AddDays(3) >= startDate && element.AddDays(3) <= endDate))
                            {
                                //проверка, разница между отпусками сотрудника - месяц. 
                                existStart = dateList.Any(element => element.AddMonths(1) >= startDate && element.AddMonths(1) >= endDate);
                                existEnd = dateList.Any(element => element.AddMonths(-1) <= startDate && element.AddMonths(-1) <= endDate);
                                if (!existStart || !existEnd)
                                    CanCreateVacation = true;
                            }
                        }

                        if (CanCreateVacation)
                        {
                            for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1))
                            {
                                // Добавление дней отпуска в списки. 
                                Vacations.Add(dt);
                                dateList.Add(dt);
                                // Удаляем даты из доступных. 
                                free_dates.Remove(dt);
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                // Удаление 3 дней до отпуска, так как их все равно выбрать нельзя. 
                                free_dates.Remove(startDate.AddDays(-1));
                                free_dates.Remove(startDate.AddDays(-2));
                                free_dates.Remove(startDate.AddDays(-3));
                            }
                            vacationCount -= difference;
                        }
                    }
                }
            }


            foreach (var VacationList in VacationDictionary)
            {
                Console.WriteLine("Дни отпуска " + VacationList.Key + " : ");
                for (int i = 0; i < VacationList.Value.Count; i++) { Console.WriteLine(VacationList.Value[i]); }
            }
            Console.ReadKey();
        }
    }
}
