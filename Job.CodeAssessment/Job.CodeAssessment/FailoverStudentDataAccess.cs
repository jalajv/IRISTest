namespace Job.CodeAssessment
{
    public interface IFailoverStudentDataAccess
    {
        StudentResponse GetStudentById(int id);
    }
    public class FailoverStudentDataAccess
    {
        public StudentResponse GetStudentById(int id)
        {
            // retrieve student from database
            return new StudentResponse();
        }
    }
}