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

        public static Month GetMonthDescription(string month)
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
    }
}
