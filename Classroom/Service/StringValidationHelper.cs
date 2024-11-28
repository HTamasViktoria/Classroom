namespace Classroom.Service
{
    public class StringValidationHelper
    {
        public static void IsValidId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("The given identifier cannot be null, empty or whitespace.");
            }
        }
    }
}