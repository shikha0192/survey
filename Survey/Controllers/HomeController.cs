using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using Survey.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.Versioning;


namespace HomeController.Controllers
{
    public class HomeController : Controller
    {
        //Survey obj = new Survey();
        public ActionResult Index()
        {
            BOSurvey obj = new BOSurvey();


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SurveyConnection"].ToString());
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_tbl_City", con);
            DataTable dt = new DataTable();
            sqlDA.Fill(dt);
            List<BOCity> cityList = new List<BOCity>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BOCity city = new BOCity();
                city.CityId = Convert.ToInt32(dt.Rows[i]["CityId"]);
                city.CityName = dt.Rows[i]["CityName"].ToString();
                cityList.Add(city);
                obj.BOCityList = cityList;
            }

            return View(obj);
        }
        public JsonResult SaveSurvey(string Name, string Age, string Gender, string Email, string Education, string UploadResumePath,int CityId)
        {
            string Message = "";
            int result = 1;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SurveyConnection"].ToString());
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_Survey", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Age", Age);
                cmd.Parameters.AddWithValue("@Gender", Gender);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Education", Education);
                cmd.Parameters.AddWithValue("@UploadResumePath", UploadResumePath);
                cmd.Parameters.AddWithValue("@CityId", CityId);



                cmd.ExecuteNonQuery();
                con.Close();


            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return Json(new { flag = result, msg = "Survey has been saved successfully." });
        }


        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    string fname = "";
                    string filePath;
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];



                        // fname = file.FileName;
                        fname = DateTime.Now.ToString("yyyyMMddHHmmssffff");

                        // Get the complete folder path and store the file inside it.  
                        filePath = Path.Combine(Server.MapPath("~/UploadedResume/"), fname);
                        file.SaveAs(filePath);
                        //filename = fname;
                    }
                    // Returns message that successfully uploaded  
                    //return Json("File Uploaded Successfully!");
                    return Json(new { filename = fname, msg = "File Uploaded Successfully!" });
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }


       // GET: Survey/Details/
       [HttpGet]
        public ActionResult Details()
        {
            BOSurvey obj = new BOSurvey();


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SurveyConnection"].ToString());
            SqlDataAdapter sqlDA = new SqlDataAdapter("sp_GetSurvey", con);
            DataTable dt = new DataTable();
            sqlDA.Fill(dt);
            List<BOSurvey> surveyList = new List<BOSurvey>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BOSurvey survey = new BOSurvey();
                survey.Name = dt.Rows[i]["Name"].ToString();
                survey.Age = dt.Rows[i]["Age"].ToString();
                survey.Gender = dt.Rows[i]["Gender"].ToString();
                survey.Email = dt.Rows[i]["Email"].ToString();
                survey.CityName = dt.Rows[i]["CityName"].ToString();
                survey.Education = dt.Rows[i]["Education"].ToString();
                survey.UploadResume = dt.Rows[i]["UploadResumePath"].ToString();
               
                surveyList.Add(survey);
                obj.BOSurveyList = surveyList;
            }
            return View(obj.BOSurveyList);
        }
    }
}
