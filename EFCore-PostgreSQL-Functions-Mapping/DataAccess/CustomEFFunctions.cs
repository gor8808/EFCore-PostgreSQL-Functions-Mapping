using Microsoft.EntityFrameworkCore.Query;

namespace EFCore_PostgreSQL_Functions_Mapping.DataAccess
{
    /// <summary>
    /// This claass contains EF core functions that are translated into SQL query
    /// </summary>
    public class CustomEFFunctions
    {
        /// <summary>
        /// Returns the first JSON item returned by the JSON path for the specified JSON value. Returns NULL if there are no results.
        /// </summary>
        /// <param name="json">json selector</param>
        /// <param name="path">json path</param>
        /// <returns>Result</returns>
        /// <exception cref="NotSupportedException"></exception>
        public static string JsonbPathQueryFirst(object json, [NotParameterized] string path)
        {
            throw new NotSupportedException($"{nameof(JsonbPathQueryFirst)} cannot be called client side");
        }
    }
}
