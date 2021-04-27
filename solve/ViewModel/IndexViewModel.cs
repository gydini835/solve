using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using solve.Models;
namespace solve.ViewModel
{
    public class IndexViewModel

    {
   
        public IEnumerable<RequestModel> requestmodels;
        public IEnumerable<file_path> file_paths;
        public IEnumerable<ip_adress> ip_adresses;
    }
}
