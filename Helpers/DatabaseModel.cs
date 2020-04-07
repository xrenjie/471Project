using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Web;
using ProjectTemp.Controllers;
using MySql.Data.MySqlClient;

namespace ProjectTemp.Helpers
{
    public class DatabaseModel
    {

        #region Query Methods

        public MySqlConnection GetSQLConnection(string connectionstring)
        {
            if (connectionstring == null)
                return null;
            return new MySqlConnection(connectionstring);
        }

        public string Get_PuBConnectionString()
        {
            try
            {
                return "Data Source=127.0.0.1,3306; Initial Catalog = hospital; User ID = hospital; Password = hospital";
            }
            catch { return null; }
        }

        public MySqlConnection GetSQLConnection()
        {
            if (Get_PuBConnectionString() == null)
                return null;
            return new MySqlConnection(Get_PuBConnectionString());
        }

        /// <summary>
        /// This method is responisble to to execute a query in your RDBMS and return for you an output value. 
        /// For instance, in some cases when you insert a new records you need to return the id of that record to do other actions
        /// </summary>
        /// <returns></returns>

        public int Execute_Non_Query_Store_Procedure(string procedureName, MySqlParameter[] parameters, string returnValue)
        {
            if (GetSQLConnection() == null)
                return -2;

            int successfulQuery = -2;
            MySqlCommand sqlCommand = new MySqlCommand(procedureName, GetSQLConnection());
            //sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.AddRange(parameters);
                sqlCommand.Connection.Open();
                successfulQuery = sqlCommand.ExecuteNonQuery();
                successfulQuery = (int)sqlCommand.Parameters["@" + returnValue].Value;

            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            if (sqlCommand.Connection != null && sqlCommand.Connection.State == ConnectionState.Open)
                sqlCommand.Connection.Close();

            return successfulQuery;
        }


        /// <summary>
        /// This method is responisble to to execute a query in your RDBMS and return for you if it was successult executed. Minay used for insert,update, and delete
        /// </summary>
        /// <returns></returns>
        public int Execute_Non_Query_Store_Procedure(string procedureName, MySqlParameter[] parameters)
        {
            if (GetSQLConnection() == null)
                return -1;

            int successfulQuery = 1;
            MySqlCommand sqlCommand = new MySqlCommand(procedureName, GetSQLConnection());
            //sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.AddRange(parameters);
                sqlCommand.Connection.Open();
                successfulQuery = sqlCommand.ExecuteNonQuery();
                //successfulQuery = 1;

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                successfulQuery = -2;
            }

            if (sqlCommand.Connection != null && sqlCommand.Connection.State == ConnectionState.Open)
                sqlCommand.Connection.Close();

            return successfulQuery;
        }


        /// <summary>
        /// This method is responisble to to execute to rertieve data from your RDBSM by executing a stored procedure. Mainly used when using one select statment
        /// </summary>
        /// <returns></returns>
        public DataTable Execute_Data_Query_Store_Procedure(string procedureName, MySqlParameter[] parameters)
        {
            if (GetSQLConnection() == null)
                return null;

            DataTable dataTable = new DataTable();
            
            MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(procedureName, GetSQLConnection());
            //sqlAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            try {
                sqlAdapter.SelectCommand.Parameters.AddRange(parameters);
                sqlAdapter.SelectCommand.Connection.Open();
                sqlAdapter.Fill(dataTable);
            }
            catch (Exception er)
            {
                string ee = er.ToString();
                dataTable = null;
            } 

            if (sqlAdapter.SelectCommand.Connection != null && sqlAdapter.SelectCommand.Connection.State == ConnectionState.Open)
                sqlAdapter.SelectCommand.Connection.Close();

            return dataTable;
        }

        /// <summary>
        /// This method is responisble to to execute to rertieve data from your RDBSM by executing a stored procedure. Mainly used when more than one table is being returned.
        /// </summary>
        /// <returns></returns>
        /// 

        public DataSet Execute_Data_Dataset_Store_Procedure(string procedureName, MySqlParameter[] parameters)
        {
            if (GetSQLConnection() == null)
                return null;

            DataSet dataset = new DataSet();
            MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(procedureName, GetSQLConnection());
            //sqlAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlAdapter.SelectCommand.Parameters.AddRange(parameters);
                sqlAdapter.SelectCommand.Connection.Open();
                sqlAdapter.Fill(dataset);
            }
            catch (Exception er)
            {
                string ee = er.ToString();
                dataset = null;
            }

            if (sqlAdapter.SelectCommand.Connection != null && sqlAdapter.SelectCommand.Connection.State == ConnectionState.Open)
                sqlAdapter.SelectCommand.Connection.Close();

            return dataset;
        }

        /// <summary>
        /// This method check if the connection string is valid or not
        /// </summary>
        /// <returns></returns>

        public bool CheckDatabaseConnectionString(string ConnectionString)
        {
            try
            {

                MySqlConnection con = new MySqlConnection(ConnectionString);
                con.Open();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }


        }
        #endregion


        
        

        public DataTable getBillingInformation( int patientID)
        {
            
             MySqlParameter[] Parameters = new MySqlParameter[1];
             Parameters[0] = new MySqlParameter("@patientID", patientID);
             return Execute_Data_Query_Store_Procedure("CALL getBillingInformation(@patientID)", Parameters);
            
        }

        public DataTable getEmployeeID(string firstName, string lastName, string address, int SSN)
        {
            MySqlParameter[] Parameters = new MySqlParameter[4];
            Parameters[0] = new MySqlParameter("@firstName", firstName);
            Parameters[1] = new MySqlParameter("@lastName", lastName);
            Parameters[2] = new MySqlParameter("@address", address);
            Parameters[3] = new MySqlParameter("@SSN", SSN);
            return Execute_Data_Query_Store_Procedure("CALL getEmployeeID(@firstName, @lastName, @address, @SSN)", Parameters);
        }

        public DataTable getHospitalStaff()
        {
            MySqlParameter[] Parameters = new MySqlParameter[0];
            return Execute_Data_Query_Store_Procedure("CALL getHospitalStaff()", Parameters);
        }

        public DataTable getLabResults(int patientID, int requestID)
        {
            MySqlParameter[] Parameters = new MySqlParameter[2];
            Parameters[0] = new MySqlParameter("@patientID", patientID);
            Parameters[1] = new MySqlParameter("@requestID", requestID);
            return Execute_Data_Query_Store_Procedure("CALL getLabResults(@patientID, @requestID)", Parameters);
        }

        public DataTable getMedicalHistory(int patientID)
        {
            MySqlParameter[] Parameters = new MySqlParameter[1];
            Parameters[0] = new MySqlParameter("@patientID", patientID);
            return Execute_Data_Query_Store_Procedure("CALL getMedicalHistory(@patientID)", Parameters);
        }

        public DataTable getNurseComments(int patientID)
        {
            MySqlParameter[] Params = new MySqlParameter[1];
            Params[0] = new MySqlParameter("@patientID", patientID);
            return Execute_Data_Query_Store_Procedure("CALL getNurseComments(@patientID)", Params);
        }

        public DataTable getPatientID(string firstName, string lastName, string address, string phoneNumber)
        {
            MySqlParameter[] Parameters = new MySqlParameter[4];
            Parameters[0] = new MySqlParameter("@firstName", firstName);
            Parameters[1] = new MySqlParameter("@lastName", lastName);
            Parameters[2] = new MySqlParameter("@address", address);
            Parameters[3] = new MySqlParameter("@phoneNumber", phoneNumber);
            return Execute_Data_Query_Store_Procedure("CALL getPatientID(@firstName, @lastName, @address, @phoneNumber)", Parameters);
        }

        public DataTable getPatientInformation(int patientID)
        {
            MySqlParameter[] Params = new MySqlParameter[1];
            Params[0] = new MySqlParameter("@patientID", patientID);
            return Execute_Data_Query_Store_Procedure("CALL getPatientInformation(@patientID)", Params);
        }

        public DataTable getPhysicianComments(int patientID)
        {
            MySqlParameter[] Params = new MySqlParameter[1];
            Params[0] = new MySqlParameter("@patientID", patientID);
            return Execute_Data_Query_Store_Procedure("CALL getPhysicianComments(@patientID)", Params);
        }

        public int insertNewEmployee(string firstName, string lastName, string sex, int SSN, string birthDate, string address, string clockIn, string clockOut, string daysPerWeek, string employeeType, int licenseNumber, string degree)
        {
            MySqlParameter[] Params = new MySqlParameter[12];
            Params[0] = new MySqlParameter("@firstName", firstName);
            Params[1] = new MySqlParameter("@lastName", lastName);
            Params[2] = new MySqlParameter("@sex", sex);
            Params[3] = new MySqlParameter("@SSN", SSN);
            Params[4] = new MySqlParameter("@birthDate", birthDate);
            Params[5] = new MySqlParameter("@address", address);
            Params[6] = new MySqlParameter("@clockIn", clockIn);
            Params[7] = new MySqlParameter("@clockOut", clockOut);
            Params[8] = new MySqlParameter("@daysPerWeek", daysPerWeek);
            Params[9] = new MySqlParameter("@employeeType", employeeType);
            Params[10] = new MySqlParameter("@licenseNumber", licenseNumber);
            Params[11] = new MySqlParameter("@degree", degree);
            return Execute_Non_Query_Store_Procedure("CALL newEmployee(@firstName, @lastName, @sex, @SSN, @birthDate, @address, @clockIn, @clockOut, @daysPerWeek, @employeeType, @licenseNumber, @degree)", Params);
        }

        public int insertNewLabRequest(int licenseNumber, string reason, int patientID)
        {
            MySqlParameter[] Params = new MySqlParameter[3];
            Params[0] = new MySqlParameter("@licenseNumber", licenseNumber);
            Params[1] = new MySqlParameter("@reason", reason);
            Params[2] = new MySqlParameter("@patientID", patientID);
            return Execute_Non_Query_Store_Procedure("CALL newLabRequest(@licenseNumber, @reason, @patientID)", Params);
        }

        public int insertNewLabResult(int labNumber, int requestID, string prognosis, string data, string notes, int patientID)
        {
            MySqlParameter[] Params = new MySqlParameter[6];
            Params[0] = new MySqlParameter("@labNumber", labNumber);
            Params[1] = new MySqlParameter("@requestID", requestID);
            Params[2] = new MySqlParameter("@prognosis", prognosis);
            Params[3] = new MySqlParameter("@data", data);
            Params[4] = new MySqlParameter("@notes", notes);
            Params[5] = new MySqlParameter("@patientID", patientID);
            return Execute_Non_Query_Store_Procedure("CALL newLabResult(@labNumber, @requestID, @prognosis, @data, @notes, @patientID)", Params);
        }

        public int insertNewLabTech(int employeeID, int labNumber, string degree)
        {
            MySqlParameter[] Params = new MySqlParameter[3];
            Params[0] = new MySqlParameter("@employeeID", employeeID);
            Params[1] = new MySqlParameter("@labNumber", labNumber);
            Params[2] = new MySqlParameter("@degree", degree);
            return Execute_Non_Query_Store_Procedure("CALL newLabTech(@employeeID, @labNumber, @degree)", Params);
        }

        public int insertNewMedicalHistory(int patientID, string medicalHistory)
        {
            MySqlParameter[] Params = new MySqlParameter[2];
            Params[0] = new MySqlParameter("@patientID", patientID);
            Params[1] = new MySqlParameter("@medicalHistory", medicalHistory);
            return Execute_Non_Query_Store_Procedure("CALL newMedicalHistory(@patientID, @medicalHistory)", Params);
        }

        public int insertNewMedication(int patientID, string medicationName, string dosage, string dateLastPrescribed)
        {
            MySqlParameter[] Params = new MySqlParameter[4];
            Params[0] = new MySqlParameter("@patientID", patientID);
            Params[1] = new MySqlParameter("@medicationName", medicationName);
            Params[2] = new MySqlParameter("@dosage", dosage);
            Params[3] = new MySqlParameter("@dateLastPrescribed", dateLastPrescribed);
            return Execute_Non_Query_Store_Procedure("CALL newMedication(@patientID, @medicationName, @dosage, @dateLastPrescribed)", Params);
        }

        public int insertNewNurseComment(int licenseNumber, int patientID, string comment)
        {
            MySqlParameter[] Params = new MySqlParameter[3];
            Params[0] = new MySqlParameter("@licenseNumber", licenseNumber);
            Params[1] = new MySqlParameter("@patientID", patientID);
            Params[2] = new MySqlParameter("@comment", comment);
            return Execute_Non_Query_Store_Procedure("CALL newNurseComment(@licenseNumber, @patientID, @comment)", Params);
        }

        public int insertNewPatient(string firstName, string lastName, string sex, string address, int SSN, int height, int weight, string bloodType, string birthDate, string phoneNumber)
        {
            MySqlParameter[] Params = new MySqlParameter[10];
            Params[0] = new MySqlParameter("@firstName", firstName);
            Params[1] = new MySqlParameter("@lastName", lastName);
            Params[2] = new MySqlParameter("@sex", sex);
            Params[3] = new MySqlParameter("@address", address);
            Params[4] = new MySqlParameter("@SSN", SSN);
            Params[5] = new MySqlParameter("@height", height);
            Params[6] = new MySqlParameter("@weight", weight);
            Params[7] = new MySqlParameter("@bloodType", bloodType);
            Params[8] = new MySqlParameter("@birthDate", birthDate);
            Params[9] = new MySqlParameter("@phoneNumber", phoneNumber);
            return Execute_Non_Query_Store_Procedure("CALL newPatient(@firstName, @lastName, @sex, @address, @SSN, @height, @weight, @bloodType, @birthDate, @phoneNumber)", Params);
        }

        public int insertNewPhysicianComment(int licenseNumber, int patientID, string comment)
        {
            MySqlParameter[] Params = new MySqlParameter[3];
            Params[0] = new MySqlParameter("@licenseNumber", licenseNumber);
            Params[1] = new MySqlParameter("@patientID", patientID);
            Params[2] = new MySqlParameter("@comment", comment);
            return Execute_Non_Query_Store_Procedure("CALL newPhysicianComment(@licenseNumber, @patientID, @comment)", Params);
        }

        public int insertNewTreatment(int patientID, string treatmentIssued)
        {
            MySqlParameter[] Parameters = new MySqlParameter[2];
            Parameters[0] = new MySqlParameter("@patientID", patientID);
            Parameters[1] = new MySqlParameter("@treatmentIssued", treatmentIssued);
            return Execute_Non_Query_Store_Procedure("CALL newTreatment(@patientID, @treatmentIssued)", Parameters);
        }

        public int updateBillingInfo(int patientID, string insurancePhoneNumber, int insuranceNumber, string insurer, string insuranceAddress)
        {
            MySqlParameter[] Params = new MySqlParameter[5];
            Params[0] = new MySqlParameter("@patientID", patientID);
            Params[1] = new MySqlParameter("@insurancePhoneNumber", insurancePhoneNumber);
            Params[2] = new MySqlParameter("@insuranceNumber", insuranceNumber);
            Params[3] = new MySqlParameter("@insurer", insurer);
            Params[4] = new MySqlParameter("@insuranceAddress", insuranceAddress);
            return Execute_Non_Query_Store_Procedure("CALL updateBillingInfo(@patientID, @insurancePhoneNumber, @insuranceNumber, @insurer, @insuranceAddress)", Params);
        }

        public int updateMedicalHistory(int patientID, string medicalHistory)
        {
            MySqlParameter[] Params = new MySqlParameter[2];
            Params[0] = new MySqlParameter("@patientID", patientID);
            Params[1] = new MySqlParameter("@medicalHistory", medicalHistory);
            return Execute_Non_Query_Store_Procedure("CALL updateMedicalHistory(@patientID, @medicalHistory)", Params);
        }

        public int updatePatientInfo(int patientID, string firstName, string lastName, string sex, string address, int SSN, int height, int weight, string bloodType)
        {
            MySqlParameter[] Params = new MySqlParameter[9];
            Params[0] = new MySqlParameter("@patientID", patientID);
            Params[1] = new MySqlParameter("@firstName", firstName);
            Params[2] = new MySqlParameter("@lastName", lastName);
            Params[3] = new MySqlParameter("@sex", sex);
            Params[4] = new MySqlParameter("@address", address);
            Params[5] = new MySqlParameter("@SSN", SSN);
            Params[6] = new MySqlParameter("@height", height);
            Params[7] = new MySqlParameter("@weight", weight);
            Params[8] = new MySqlParameter("@bloodType", bloodType);
            return Execute_Non_Query_Store_Procedure("CALL updatePatientInfo(@patientID, @firstName, @lastName, @sex, @address, @SSN, @height, @weight, @bloodType)", Params);
        }
       
        
    }
}
