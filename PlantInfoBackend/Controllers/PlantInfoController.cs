using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlantInfoBackend.Services;
using System.Data;
using System.Data.SqlClient;

// controller for all database and front end communication

namespace PlantInfoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantInfoController : ControllerBase
    {
        private IConfiguration _configuration;
        public PlantInfoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetPlantData")]
        public JsonResult GetPlantData()
        {
            string query = "SELECT Plant.PlantName, Plant.IdealMoisture, Plant.LowMoisture, UserName.Email FROM dbo.UserName JOIN dbo.Plant ON UserName.PlantName = Plant.PlantName WHERE plant.Plantname = UserName.Plantname;";
            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("PlantInfoDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
               myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);

        }

        [HttpPost]
        [Route("SetUserValues")]
        public JsonResult SetUserValues([FromForm]string userEmail, [FromForm]string plant)
        {
            string query = "update dbo.UserName set UserName.Email = @userEmail, Username.PlantName = @plant where dbo.UserName.RecordNumber = 1";
            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("PlantInfoDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                   
                    myCommand.Parameters.AddWithValue("@userEmail", userEmail);
                    myCommand.Parameters.AddWithValue("@plant", plant);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added!");

        }

        // deletes an email from the database
        [HttpDelete]
        [Route("DeleteEmail")]
        public JsonResult DeleteEmail([FromForm] string userEmail)
        {
            string query = "delete from dbo.UserName where email=@userEmail";
            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("PlantInfoDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@userEmail", userEmail);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted!");
        }

        // Gets the information from the database 
        [HttpGet]
        [Route("CheckLevel")]
        public JsonResult CheckLevel()
        {
            string query = "select Plant.LowMoisture, UserName.PlantName, UserName.email, Username.Name from Plant join UserName On UserName.PlantName = Plant.PlantName";
            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("PlantInfoDBCon");
            SqlDataReader myReader;
            PlantData plantData = new PlantData();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}

