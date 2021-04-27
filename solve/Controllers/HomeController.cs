using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using solve.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using solve.ViewModel;
using HtmlAgilityPack;

namespace solve.Controllers
{

    public class HomeController : Controller
    {
        private ApplicationContext db;
        private readonly ILogger<HomeController> _logger;
        ApplicationContext _context;
        IWebHostEnvironment _appEnvironment;
      
        public FileModel file { get; set; }
        /* public void Launch(string name, string path)
         {
             file = file.getInstance(name, path);
         }*/

        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {

            if (uploadedFile != null)
            {

                string path = "/Files/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            
                FileModel f1 = FileModel.getInstance( 
                    uploadedFile.FileName, 
                    path );
                

            }

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            FileModel f2 = FileModel.getInstance(null, null);
            string tpath = f2.Path;
             using (StreamReader line = new StreamReader(tpath, System.Text.Encoding.Default))
            {
                string sr;
                string p_ip_adress = @"(\d+[^a-zA-Z0-9_: ]+\d+[^a-zA-Z0-9_: ]+\d+[^a-zA-Z0-9_: ]+\d+)";
                string p_date = @"([0-9]+\W+[a-zA-Z]+\W+\d+[^a-zA-Z0-9_. ]+\d+[^a-zA-Z0-9_. ]+\d+[^a-zA-Z0-9_. ]+\d+)";
                string p_metod = @"\b(POST)\b|\b(GET)\b|\b(DELETE)\b|\b(HEAD)\b";
                string p_file = @"([^ ]+html)|([^ ]+gif)|([^ ]+png)|([^ ]+svg) ";
                string p_status_value = @"([ ]+[0-9]{1,})";
                while ((sr = line.ReadLine()) != null)
                {
                  
                        Regex r_p_ip_adress = new Regex(p_ip_adress);
                        Match _r_p_ip_adress = r_p_ip_adress.Match(sr);
                        string company = Get_Name_Company(_r_p_ip_adress.Groups[1].Value);
                        Regex r_p_date = new Regex(p_date);
                        Match _r_p_date = r_p_date.Match(sr);
                        Regex r_p_metod = new Regex(p_metod);
                        Match _r_p_metod = r_p_metod.Match(sr);
                        Regex r_p_file = new Regex(p_file);
                        Match _r_p_file = r_p_file.Match(sr);
                        string tag_file = Get_Tag_Html(_r_p_file.Groups[1].Value);
                        Regex r_p_status_value = new Regex(p_status_value);
                        Match _r_p_status_value = r_p_status_value.Match(sr);
                        RequestModel r = new RequestModel
                        {
                            
                            date_r = DateTime.ParseExact(_r_p_date.Groups[1].Value, "dd/MMM/yyyy:HH:mm:ss",
                                           System.Globalization.CultureInfo.InvariantCulture),
                            method_r = reg_method(_r_p_metod),
                           
                            status_r = _r_p_status_value.Groups[1].Value,
                            value_r = _r_p_status_value.Groups[1].Value
                        };
                        db.request.Add(r);
                        db.file_pathes.Add(new file_path { f_path = file_method(_r_p_file), tag = Get_Tag_Html(_r_p_file.Groups[1].Value), request = r });
                        db.ip_adresses.Add(new ip_adress { ip = _r_p_ip_adress.Groups[1].Value, company_name = Get_Name_Company(_r_p_ip_adress.Groups[1].Value), request = r });
                        await db.SaveChangesAsync();
                        GC.Collect();
                    }
        
            }
            return RedirectToAction("Index");
        }

        public string reg_method(Match t)
        {
            for(int i = 0; i < 4; i++)
            {
                if (t.Groups[i].Value != null)
                {
                   return t.Groups[i].Value;
                }
            }
            return null;
        }
        public string file_method (Match t)
        {
            for (int i = 0; i < 4; i++)
            {
                if (t.Groups[i].Value != null)
                {
                    return t.Groups[i].Value;
                }
            }
            return null;
        }




        public HomeController(ApplicationContext context, ILogger<HomeController> logger, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
            _logger = logger;
            
           
        }
        public IActionResult Index(SortState sortOrder = SortState.StatusAsc)
        {
            /* IQueryable<RequestModel> requests = db.request.Include
             ViewData["StatusSort"] = sortOrder == SortState.StatusAsc ? SortState.StatusDesc : SortState.StatusAsc;*/
            var f = db.file_pathes.Include(f => f.request).ToList();
            var i = db.ip_adresses.Include(p => p.request).ToList();
            var r = db.request.ToList();
            var result = (from cust in f
                          join check in r on cust.id equals check.id
                          select new
                          {
                              id = cust.id, data_r = check.date_r, method_r = check.method_r, status_r =check.status_r, value = check.value_r, tag= cust.tag, f_path = cust.f_path  

                          });
            var results = (from cust in result
                           join check in i on cust.id equals check.id
                           select new
                           {
                               id = cust.id,
                               ip = check.ip,
                               company_name = check.company_name

                           });
           
            IndexViewModel v = new IndexViewModel { file_paths = db.file_pathes.Include(f => f.request).ToList(), ip_adresses = db.ip_adresses.Include(p => p.request).ToList(), requestmodels = db.request.ToList() };
            return View(v);
        }
     
      
        public string Get_Tag_Html(string path)
        {
            
            var html = @"http://tariscope.com/en/products" + path;
            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            return htmlDoc.DocumentNode.SelectSingleNode("//head/base").Attributes["href"].Value;
        }

        public string Get_Name_Company(string ip)
        {
            var objClient = new System.Net.WebClient();

            var strFile = objClient.DownloadString("http://ip-api.com/json/" + ip + "?fields=org");
            var x = JsonConvert.SerializeObject(strFile, Formatting.Indented);
           return  x.Substring(12);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
