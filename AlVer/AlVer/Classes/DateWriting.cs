namespace AlVer.Classes
{
    public class DateWriting
    {

        public static string DateWrite(DateTime? date)
        {
            return date.Value.Day.ToString() +"/"+date.Value.Month.ToString() + "/"+date.Value.Year.ToString() +"-" + (date.Value.Hour + 3).ToString() + ":" + date.Value.Minute.ToString(); 
        }
    }
}
