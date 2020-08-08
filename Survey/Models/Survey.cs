using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc.Razor;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Survey.Models
{
    public class BOSurvey
    {
        public int Id { get; set; }
        
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [StringLength(100, ErrorMessage = "Should be in numeric and not greater than zero", MinimumLength = 3)]
        //[DataType(DataType.Age)]
        [Display(Name = "Age")]
        public string Age { get; set; }
        
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]

        public string Email { get; set; }
       
        public int CityId { get; set; }

        
        [Display(Name = "Upload Resume")]
        
        public string UploadResume { get; set; }
   
        
        [Display(Name = "Education")]
        public string Education { get; set; }

        
        [Display(Name = "CityName")]
        public string CityName { get; set; }

        public List<BOCity> BOCityList { get; set; }
        public List<BOSurvey> BOSurveyList { get; set; }
        

    }
    public class BOCity {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}