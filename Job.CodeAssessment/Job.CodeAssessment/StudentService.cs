using Asos.CodeTest;
using System;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Job.CodeAssessment
{
    public class StudentService
    {
        private ILogger _fileLogger;
        private IArchivedDataService _archiveddataservice;
        private IFailoverRepository _failoverrepository;
        private IStudentDataAccess _studentdataaccess;
        private IFailoverStudentDataAccess _failoverstudentdataaccess;
        public StudentService(ILogger fileLogger, IArchivedDataService archiveddataservice,IFailoverRepository failoverrepository, IStudentDataAccess studentdataacces, IFailoverStudentDataAccess failoverstudentdataccess)
        {
            _fileLogger = fileLogger;
            _archiveddataservice = archiveddataservice;
            _failoverrepository = failoverrepository;
            _studentdataaccess = studentdataacces;
            _failoverstudentdataaccess = failoverstudentdataccess;
        }
        /// <summary>
        /// GetStudent method is used to get the student record based on studentId parameter
        /// </summary>
        /// <param name="studentId">Unique student Id</param>
        /// <param name="isStudentArchived">Boolean type parameter to check whether the sudent is archived or not</param>
        /// <returns></returns>
        public Student GetStudent(int studentId, bool isStudentArchived)
        {
            Student student = null;
            try
            {
                _fileLogger.Info("Add method Start");
                Student archivedStudent = null;
              
                if (isStudentArchived)
                {
                    // var archivedDataService = new ArchivedDataService();
                    archivedStudent = _archiveddataservice.GetArchivedStudent(studentId);
                    return archivedStudent;
                }
                else
                {

                   // var failoverRespository = new FailoverRepository();
                    var failoverEntries = _failoverrepository.GetFailOverEntries();


                    var failedRequests = 0;

                    foreach (var failoverEntry in failoverEntries)
                    {
                        if (failoverEntry.DateTime > DateTime.Now.AddMinutes(-10))
                        {
                            failedRequests++;
                        }
                    }

                    StudentResponse studentResponse = null;
                    

                    if (failedRequests > 100 && (ConfigurationManager.AppSettings["IsFailoverModeEnabled"] == "true" || ConfigurationManager.AppSettings["IsFailoverModeEnabled"] == "True"))
                    {
                        // studentResponse = FailoverStudentDataAccess.GetStudentById(studentId);
                        studentResponse = _failoverstudentdataaccess.GetStudentById(studentId);
                    }
                    else
                    {
                       // var dataAccess = new StudentDataAccess();
                        studentResponse = _studentdataaccess.LoadStudent(studentId);
                    }

                    if (studentResponse.IsArchived)
                    {
                       // var archivedDataService = new ArchivedDataService(); 
                       /* Two objects of the same class was created within the same function */
                        student = _archiveddataservice.GetArchivedStudent(studentId);
                    }
                    else
                    {
                        student = studentResponse.Student;
                    }   
                }
            }
            catch (Exception ex)
            {
                _fileLogger.Error("Error Occurred Studentservice >> Getstudent()", ex);
            }
            return student;
        }
    }
}
