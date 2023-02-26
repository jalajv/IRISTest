namespace Job.CodeAssessment
{
    public interface IStudentDataAccess
    {
        StudentResponse LoadStudent(int studentId);
    }
    public class StudentDataAccess:IStudentDataAccess
    {
        public StudentResponse LoadStudent(int studentId)
        {
            // retrieve student from 3rd party webservice
            return new StudentResponse();
        }
    }
}