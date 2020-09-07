namespace DailyHealthReportZju.Helpers
{
    public class ChangeDetector<T>
    {
        T _prev;
        public bool IsChanged(T newElement)
        {
            if (newElement.Equals(_prev))
            {
                return false;
            }
            _prev = newElement;
            return true;
        }
    }
}
