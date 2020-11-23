using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace BasicLibrary
{
    public class SqlConnDataBase : IDisposable
    {
        /// <summary>
        /// Store the Connection String
        /// </summary>
        private string strConn = null;
        private SqlCommand Command = null;
        private SqlConnection Connection = null;
        private SqlDataAdapter DataAdapter = null;
        private DataTable Table { get; set; }

        /// <summary>
        /// Return a Message as result of any action
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Return the state of the connection
        /// </summary>
        public bool State { get; private set; }
        // Track whether Dispose has been called.
        private bool isDisposed = false;

        //Constructor
        /// <summary>
        /// Implement the Connection to SQL by ADO
        /// </summary>
        /// <param name="_ConnectionString">Set the name of key on the Web Config or set the complete connection string</param>
        /// <param name="FromWebConfig">If true, the connection string is gived from a WebConfig file</param>
        public SqlConnDataBase(string _ConnectionString, bool FromWebConfig = false)
        {
            if (FromWebConfig)
                strConn = ConfigurationManager.ConnectionStrings[_ConnectionString].ConnectionString;
            else
                strConn = _ConnectionString;

            //init
            try
            {
                Connection = new System.Data.SqlClient.SqlConnection(strConn);
                Connection.Open();
                State = true;
            }
            catch(Exception ex)
            {
                State = false;
                //Message = "No se ha establecido la conexión con la base de datos.";
                Message = "The connection to the database has not been established.";
            }
        }

        /// <summary>
        /// Return a table with the query result
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            return Table;
        }

        /// <summary>
        /// Execute the SqlCommand expecting something back
        /// </summary>
        /// <returns></returns>
        public bool ReadExecution()
        {
            if (State)
            {
                try
                {
                    DataSet _DataSet = new DataSet();
                    DataAdapter = new SqlDataAdapter(Command);
                    DataAdapter.Fill(_DataSet);

                    //One table was returned and stored in Table variable
                    Table = _DataSet.Tables[0];

                    return true;
                }
                catch(Exception ex)
                {
                    //Message = "Error de ejecución";
                    Message = "Execution error.";
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Execute the SqlCommand with no read result
        /// </summary>
        /// <returns></returns>
        public bool NoReadExecution()
        {
            if (State)
            {
                try
                {
                    Command.ExecuteNonQuery();

                    return true;
                }
                catch(Exception ex)
                {
                    //Message = "Error de ejecución";
                    Message = "Execution error.";
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Set the name of an StoreProcedure to be executed
        /// </summary>
        /// <param name="myStoreProcedure">Name of the SP on SQL DataBase</param>
        public void StoreProcedureName(string myStoreProcedure)
        {
            Command = new SqlCommand(myStoreProcedure, Connection);
            Command.CommandType = CommandType.StoredProcedure;
        }
        /// <summary>
        /// Set the query string to be executed
        /// </summary>
        /// <param name="myQueryString">Set the query sintax to execute on DB</param>
        public void Query(string myQueryString)
        {
            Command = new SqlCommand(myQueryString, Connection);
            Command.CommandType = CommandType.Text;
        }

        /// <summary>
        /// Used with the StoreProcedureName to set the parameters of the StoreProcedure
        /// </summary>
        /// <param name="ParamName">Set the name of the Parameter like "@ParamName". This must to be equal in the SP on DataBase</param>
        /// <param name="ParamVal">Set the value associated with the parameter</param>
        public void InputParam(string ParamName, string ParamVal)
        {
            Command.Parameters.AddWithValue(ParamName, ParamVal).Direction = ParameterDirection.Input;
        }
        public void InputParam(string ParamName, int ParamVal)
        {
            Command.Parameters.AddWithValue(ParamName, ParamVal).Direction = ParameterDirection.Input;
        }
        public void InputParam(string ParamName, bool ParamVal)
        {
            Command.Parameters.AddWithValue(ParamName, ParamVal).Direction = ParameterDirection.Input;
        }



        public void Dispose()
        {
            if(Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SqlConnDataBase()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;
            if (disposing)
            {
                // if any managed resources to be free.  
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
            isDisposed = true;
        }
    }
}
