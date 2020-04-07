using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ProjectTemp.Helpers;

namespace ProjectTemp.Controllers
{
    [Route("api/ValuesController")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        DatabaseModel dbm = new DatabaseModel();

        [HttpGet]
        [Route("GetBillingInfo")]
        public ActionResult<IEnumerable<string>> GetBillingInfo(int patientID)
        {
            DataTable billing = dbm.getBillingInformation(patientID);
            string[] result = new string[5];
            result[0] = "patientID: " + billing.Rows[0][0].ToString();
            result[1] = "insuranceNumber: " + billing.Rows[0][1].ToString();
            result[2] = "insuranceCompany: " + (string)billing.Rows[0][2];
            result[3] = "insurancePhoneNumber: " + (string)billing.Rows[0][3];
            result[4] = "insuranceAddress: " + (string)billing.Rows[0][4];

            return result;
        }

        [HttpGet]
        [Route("GetEmployeeID")]
        public string GetEmployeeID(string firstName, string lastName, string address, int SSN)
        {
            return "employeeID: " + dbm.getEmployeeID(firstName, lastName, address, SSN).Rows[0][0].ToString();
        }

        [HttpGet]
        [Route("GetHospitalStaff")]
        public string[,] GetHospitalStaff()
        {


            DataTable staff = dbm.getHospitalStaff();
            int length = staff.Rows.Count;
            string[,] result = new string[staff.Rows.Count,11];
            int count = 0;
            foreach (DataRow row in staff.Rows)
            {
                result[count,0] = "employeeID: " + staff.Rows[count][0].ToString();
                result[count,1] = "sex: " + (string)staff.Rows[count][1];
                result[count,2] = "firstName: " + (string)staff.Rows[count][2];
                result[count,3] = "lastName: " + (string)staff.Rows[count][3];
                result[count,4] = "birthdate: " + staff.Rows[count][4].ToString();
                result[count,5] = "address: " + (string)staff.Rows[count][5];
                result[count,6] = "SSN: " + staff.Rows[count][6].ToString();
                result[count,7] = "clockIn: " + staff.Rows[count][7].ToString();
                result[count,8] = "clockOut: " + staff.Rows[count][8].ToString();
                result[count,9] = "daysPerWeek: " + (string)staff.Rows[count][9];
                result[count,10] = "employeeType: " + (string)staff.Rows[count++][10];
            }
            return result;
        }

        [HttpGet]
        [Route("GetLabResults")]
        public ActionResult<IEnumerable<string>> GetLabResults(int patientID, int requestID)
        {
            DataTable results = dbm.getLabResults(patientID, requestID);
            string[] result = new string[3];
            result[0] = "prognosis: " + (string)results.Rows[0][0];
            result[1] = "data: " + (string)results.Rows[0][1];
            result[2] = "notes: " + (string)results.Rows[0][2];

            return result;
        }

        [HttpGet]
        [Route("GetMedicalHistory")]
        public ActionResult<IEnumerable<string>> GetMedicalHistory(int patientID)
        {
            DataTable results = dbm.getMedicalHistory(patientID);
            string[] result = new string[1];
            result[0] = "medicalHistory: " + (string)results.Rows[0][0];

            return result;
        }

        [HttpGet]
        [Route("GetNurseComments")]
        public ActionResult<IEnumerable<string>> GetNurseComments(int patientID)
        {
            DataTable results = dbm.getNurseComments(patientID);
            string[] result = new string[results.Rows.Count];
            int count = 0;
            foreach (DataRow row in results.Rows)
            {
                result[count] = "comment: " + (string)results.Rows[count++][0];
            }
            return result;
        }

        [HttpGet]
        [Route("GetPatientID")]
        public string GetPatientID(string firstName, string lastName, string address, string phoneNumber)
        {
            return "patientID: " + dbm.getPatientID(firstName, lastName, address, phoneNumber).Rows[0][0].ToString();
        }

        [HttpGet]
        [Route("GetPatientInformation")]
        public ActionResult<IEnumerable<string>> GetPatientInformation(int patientID)
        {
            DataTable results = dbm.getPatientInformation(patientID);
            string[] result = new string[13];
            result[0] = "patientID: " + results.Rows[0][0].ToString();
            result[1] = "sex: " + (string)results.Rows[0][1];
            result[2] = "firstName: " + (string)results.Rows[0][2];
            result[3] = "lastName: " + (string)results.Rows[0][3];
            result[4] = "address: " + (string)results.Rows[0][4];
            result[5] = "SSN: " + results.Rows[0][0].ToString();
            result[6] = "height: " + results.Rows[0][1].ToString();
            result[7] = "weight: " + results.Rows[0][2].ToString();
            result[8] = "bloodType: " + (string)results.Rows[0][3];
            result[9] = "birthDate: " + results.Rows[0][4].ToString();
            result[10] = "phoneNumber: " + (string)results.Rows[0][0].ToString();
            result[11] = "nurseLicenseNumber: " + results.Rows[0][1].ToString();
            result[12] = "physicianLicenseNumber: " + results.Rows[0][2].ToString();

            return result;
        }

        [HttpGet]
        [Route("GetPhysicianComments")]
        public ActionResult<IEnumerable<string>> GetPhysicianComments(int patientID)
        {
            DataTable results = dbm.getPhysicianComments(patientID);
            string[] result = new string[results.Rows.Count];
            int count = 0;
            foreach (DataRow row in results.Rows)
            {
                result[count] = "comment: " + (string)results.Rows[count++][0];
            }
            return result;
        }

        [HttpPost]
        [Route("InsertNewEmployee")]
        public int InsertNewEmployee(string firstName, string lastName, string sex, int SSN, string birthDate, string address, string clockIn, string clockOut, string daysPerWeek, string employeeType, int licenseNumber, string degree)
        {
            return dbm.insertNewEmployee(firstName, lastName, sex, SSN, birthDate, address, clockIn, clockOut, daysPerWeek, employeeType, licenseNumber, degree);
        }

        [HttpPost]
        [Route("InsertNewLabRequest")]
        public int InsertNewLabRequest(int licenseNumber, string reason, int patientID)
        {
            return dbm.insertNewLabRequest(licenseNumber, reason, patientID);
        }

        [HttpPost]
        [Route("InsertNewLabResult")]
        public int InsertNewLabResult(int labNumber, int requestID, string prognosis, string data, string notes, int patientID)
        {
            return dbm.insertNewLabResult(labNumber, requestID, prognosis, data, notes, patientID);
        }

        [HttpPost]
        [Route("InsertNewLabTech")]
        public int InsertNewLabTech(int employeeID, int labNumber, string degree)
        {
            return dbm.insertNewLabTech(employeeID, labNumber, degree);
        }

        [HttpPost]
        [Route("InsertNewMedicalHistory")]
        public int InsertNewMedicalHistory(int patientID, string medicalHistory)
        {
            return dbm.insertNewMedicalHistory(patientID, medicalHistory);
        }

        [HttpPost]
        [Route("InsertNewMedication")]
        public int InsertNewMedication(int patientID, string medicationName, string dosage, string dateLastPrescribed)
        {
            return dbm.insertNewMedication(patientID, medicationName, dosage, dateLastPrescribed);
        }

        [HttpPost]
        [Route("InsertNewNurseComment")]
        public int InsertNewNurseComment(int licenseNumber, int patientID, string comment)
        {
            return dbm.insertNewNurseComment(licenseNumber, patientID, comment);
        }

        [HttpPost]
        [Route("InsertNewPatient")]
        public int InsertNewPatient(string firstName, string lastName, string sex, string address, int SSN, int height, int weight, string bloodType, string birthDate, string phoneNumber)
        {
            return dbm.insertNewPatient(firstName, lastName, sex, address, SSN, height, weight, bloodType, birthDate, phoneNumber);
        }

        [HttpPost]
        [Route("InsertNewPhysicianComment")]
        public int InsertNewPhysicianComment(int licenseNumber, int patientID, string comment)
        {
            return dbm.insertNewPhysicianComment(licenseNumber, patientID, comment);
        }

        [HttpPost]
        [Route("InsertNewTreatment")]
        public int InsertNewTreatment(int patientID, string treatment)
        {
            return dbm.insertNewTreatment(patientID, treatment);
        }

        [HttpPut]
        [Route("UpdateBillingInfo")]
        public int UpdateBillingInfo(int patientID, string insurancePhoneNumber, int insuranceNumber, string insurer, string insuranceAddress)
        {
            return dbm.updateBillingInfo(patientID, insurancePhoneNumber, insuranceNumber, insurer, insuranceAddress);
        }

        [HttpPut]
        [Route("UpdateMedicalHistory")]
        public int UpdateMedicalHistory(int patientID, string medicalHistory)
        {
            return dbm.updateMedicalHistory(patientID, medicalHistory);
        }

        [HttpPut]
        [Route("UpdatePatientInfo")]
        public int UpdatePatientInfo(int patientID, string firstName, string lastName, string sex, string address, int SSN, int height, int weight, string bloodType)
        {
            return UpdatePatientInfo(patientID, firstName, lastName, sex, address, SSN, height, weight, bloodType);
        }
    }
}
