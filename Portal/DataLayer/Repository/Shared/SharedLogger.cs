using System;
using System.Web;
using System.Web.Script.Serialization;
using DataLayer.Entitties;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace DataLayer.Repository.Shared
{
    public class SharedLogger
    {
        
        public static void LogError(Exception ex, params object[] arrVal)
        {
            try
            {
                dynamic username = GetUser();
                dynamic modelExcep = new LogException();

                modelExcep.MsgException = ex.Message;
                modelExcep.LogExceptionUid = Guid.NewGuid();
                modelExcep.InnerException = GetInnerExceptions(ex);
                modelExcep.ParamsValues = GetSerializedValues(arrVal);
                modelExcep.StackTrace = ex.StackTrace;
                modelExcep.Timestamp = DateTime.Now;
                modelExcep.Username = username;
                SaveLogToDb(modelExcep);
            }
            catch (Exception)
            {
                return;
            }
        }


        public static void SaveLogToDb(LogException modelExcep)
        {
            try
            {
                using (var dbConn = new LoggerEntities())
                {
                    dbConn.LogException.Add(modelExcep);
                    dbConn.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                SaveLogToFile(modelExcep);
            }
        }

        private static void SaveLogToFile(LogException logException)
        {
            try
            {
                LogManager.GetLogger("ERROR_LOGGER").Fatal(GetSerializedValue(logException));
            }
            catch (Exception)
            {
                return;
            }
        }


        private static string GetSerializedValue(LogException logException)
        {
            try
            {
                return new JavaScriptSerializer().Serialize(logException);
            }
            catch (Exception)
            {
                return string.Format("Recursión - {0}", "GetSerializedValue");
            }
        }


        private static string GetSerializedValues(object[] arrVal)
        {
            try
            {
                dynamic serializer = new JavaScriptSerializer();
                return serializer.Serialize(arrVal);
            }
            catch (Exception)
            {
                return string.Format("Recursión - {0}", "GetSerializedValues");
            }
        }

        private static string GetInnerExceptions(Exception exception)
        {
            try
            {
                if (exception.InnerException == null)
                {
                    return "|EOE|";
                }
                return string.Format("|{0}|{1}", exception.InnerException.Message, GetInnerExceptions(exception.InnerException));
            }
            catch (Exception)
            {
                return string.Format("Recursión - {0}", "GetInnerExceptions");
            }
        }

        private static string GetUser()
        {
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.User == null)
                {
                    return string.Empty;
                }

                return HttpContext.Current.User.Identity.Name;

            }
            catch (Exception)
            {
                return "No usuario ";
            }
        }
    }
}


