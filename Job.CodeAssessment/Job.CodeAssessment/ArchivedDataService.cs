namespace Job.CodeAssessment
{
    public interface IArchivedDataService
    {
        Student GetArchivedStudent(int studentId);
    }
    public class ArchivedDataService: IArchivedDataService
    {
        public Student GetArchivedStudent(int studentId)
        {
            // retrieve student from archive data service
            return new Student();
        }
    }
}