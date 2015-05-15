using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectScheduler.Models
{

    public interface IRange<T>
    {
        T Start { get; }
        T End { get; }
        bool Includes(T value);
        bool Includes(IRange<T> range);
    }

    public class DateRange : IRange<DateTime>
    {
        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public bool Includes(DateTime value)
        {
            return (Start <= value) && (value <= End);
        }

        public bool Includes(IRange<DateTime> range)
        {
            return (Start <= range.Start) && (range.End <= End);
        }
        //combine for linq call

        public static bool DateInRange(DateTime start, DateTime end, DateTime query)
        {
            DateRange range = new DateRange(start, end);
            return range.Includes(query);
        }
    }

}