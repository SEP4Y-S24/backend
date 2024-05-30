using Models;

namespace Shared.Helpers;

public class DateComparer:IComparer<Measurement>
{
   
        public int Compare(Measurement a, Measurement b)  
        {
            return  (a == null && b == null) ? 0
                : (a == null) ? -1
                : (b == null) ? 1
                : a.TimeOfReading.Ticks.CompareTo(b.TimeOfReading.Ticks);
        }
    
}