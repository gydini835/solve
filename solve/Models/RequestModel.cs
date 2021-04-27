using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace solve.Models
{
    [Table("request")]
    public class RequestModel
    {
      
        public int id { get; set; }      
        public DateTime date_r { get; set; }
        public string method_r {get;set;}
        public string status_r { get; set; }
        public string value_r { get; set; }
        public List<file_path> files { get; set; }
        public List<ip_adress> ip_adreses { get; set; }
    }
    [Table("ip_adresses")]
    public class ip_adress
    {
        
        public int ipInfoKey { get; set; }
        [ForeignKey("ipInfoKey")]
        public RequestModel request { get; set; }

        public int id { get; set; } 
        public string ip { get; set; }
        public string company_name { get; set; }
    }
    [Table("file_pathes")]
    public class file_path
    {
        
        public int fileInfoKey { get; set; }
        [ForeignKey("fileInfoKey")]
        public RequestModel request { get; set; }

        public int id { get; set; }
        public string f_path { get; set; }
        public string tag { get; set; }
    }
}
