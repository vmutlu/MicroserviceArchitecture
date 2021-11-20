namespace DotnetMicroserviceArchitecture.CatalogAPI.Constants
{
    public class Route
    {
        #region Categories Routes Contant

        public const string HTTPGET_CATEGORIES = "Categories";
        public const string HTTPGET_CATEGORIESBYID = "Categories/{id}";

        #endregion

        #region Courses Routes Contant

        public const string HTTPGET_COURSES = "Courses";
        public const string HTTPGET_COURSESBYID = "Courses/{id}";
        public const string HTTPDELETE_COURSESBYID = "Courses/{courseId}";
        public const string HTTPGET_COURSESBYUSERID = "GetAllByUserId/{userId}";

        #endregion
    }
}
