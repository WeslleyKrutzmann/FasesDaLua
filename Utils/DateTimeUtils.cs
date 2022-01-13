using FasesDaLua.Domain;
using System;

namespace FasesDaLua.Utils
{
    public class DateTimeUtils
    {
        public static string GetDayOfWeekDescription(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "Domingo";

                case DayOfWeek.Monday:
                    return "Segunda-Feira";

                case DayOfWeek.Tuesday:
                    return "Terça-Feira";

                case DayOfWeek.Wednesday:
                    return "Quarta-Feira";

                case DayOfWeek.Thursday:
                    return "Quinta-Feira";

                case DayOfWeek.Friday:
                    return "Sexta-Feira";

                case DayOfWeek.Saturday:
                    return "Sábado";

                default:
                    throw new NotSupportedException($"Day of week {dayOfWeek} is not supported.");
            }
        }

        public static Month GetMonth(string month)
        {
            switch (month)
            {
                case "January":
                case "Janeiro":
                    return Month.January;

                case "February":
                case "Fevereiro":
                    return Month.February;

                case "March":
                case "Março":
                    return Month.March;

                case "April":
                case "Abril":
                    return Month.April;

                case "May":
                case "Maio":
                    return Month.May;

                case "June":
                case "Junho":
                    return Month.June;

                case "July":
                case "Julho":
                    return Month.July;

                case "August":
                case "Agosto":
                    return Month.August;

                case "September":
                case "Setembro":
                    return Month.September;

                case "Outubro":
                    return Month.October;

                case "November":
                case "Novembro":
                    return Month.November;

                case "December":
                case "Dezembro":
                    return Month.December;

                default:
                    throw new NotSupportedException($"Month {month} is not supported.");
            }
        }

        public static string GetMonthDescription(Month month)
        {
            switch (month)
            {
                case Month.January:
                    return "Janeiro";

                case Month.February:
                    return "Fevereiro";

                case Month.March:
                    return "Março";

                case Month.April:
                    return "Abril";

                case Month.May:
                    return "Maio";

                case Month.June:
                    return "Junho";

                case Month.July:
                    return "Julho";

                case Month.August:
                    return "Agosto";

                case Month.September:
                    return "Setembro";

                case Month.October:
                    return "Outubro";

                case Month.November:
                    return "Novembro";

                case Month.December:
                    return "Dezembro";

                default:
                    throw new NotSupportedException($"Month {month} is not supported.");
            }
        }
    }
}
