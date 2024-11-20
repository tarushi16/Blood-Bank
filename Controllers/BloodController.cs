using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using BloodBank.Models;

namespace BloodBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class bloodController : ControllerBase
    {
        //refer to the blood models
        public static List<blood> bloods = new List<blood>
        {
            new blood { Id = 1, age = 21, BloodType = "O+",ContactInfo="1234567890",DonorName="kucchu",CollectionDate=  DateTime.Now, Quantity=3, ExpiratoinDate = DateTime.Now.AddDays(40), Status = "Available" },
            new blood { Id = 2, age = 22, BloodType = "AB-",ContactInfo="1234567890",DonorName="harshit",CollectionDate=  DateTime.Now, Quantity=3, ExpiratoinDate = DateTime.Now.AddDays(40), Status = "Available" },
            new blood { Id = 3, age = 22, BloodType = "B+",ContactInfo="1234567890",DonorName="chiku",CollectionDate=  DateTime.Now, Quantity=3, ExpiratoinDate = DateTime.Now.AddDays(40), Status = "Available" }
        };


        //I have defined functions here for validation of each field to check for data validation 
        private bool Statuscheck(string status)
        {
            if (status == "Available" || status == "Requested" || status == "Expired") return true;
            return false;
        }

        private bool BloodTypecheck(string bloodtype)
        {
            if (bloodtype == "A+" || bloodtype == "B-" || bloodtype == "B+" || bloodtype == "A-" || bloodtype == "AB+" || bloodtype == "AB-" || bloodtype == "O+" || bloodtype == "O-")
            {
                return true;
            }
            return false;
        }
        private bool MobnoCheck(string mobno)
        {
            string pattern = @"^\d{10}$";
            if (Regex.IsMatch(mobno, pattern)) return true;
            return false;
        }


        //post endpoint
        [HttpPost]
        public ActionResult<IEnumerable<blood>> AddBlood(string DonornName, int age, string bloodType, string mobNo, int quantity, DateTime CollectionDate, DateTime ExpDate, string status)
        {
        
            if (!Statuscheck(status))
            {
                return BadRequest("Status can only be - Availabe, Requested or Expired");
            }
            if (!BloodTypecheck(bloodType))
            {
                return BadRequest("Enter a valid blood type in capital alphabet with sign after it ");
            }
            if (!MobnoCheck(mobNo))
            {
                return BadRequest("Enter a valid Mobile Number");
            }
            if (age <= 0 || quantity <= 0 || string.IsNullOrEmpty(DonornName))
            {
                return BadRequest("Check if age or quantity is negative or zero or else Donor Name not provided");
            }

            //unique id 
            int id = 1;
            if (bloods != null && bloods.Count > 0)
            {
                id = bloods.Max(i => i.Id) + 1;
            }

            bloods.Add(new blood { Id = id, age = age, DonorName = DonornName, BloodType = bloodType, ContactInfo = mobNo, Quantity = quantity, CollectionDate = CollectionDate, ExpiratoinDate = ExpDate, Status = status });
            return Ok(bloods);
        }

        //Data Fetch
        [HttpGet]
        public ActionResult<IEnumerable<blood>> GetBloodData()
        {
            //not found error evaluation
            if (bloods == null) return NotFound("Empty no Data Entered");
            return Ok(bloods);
        }



        //same id error configuration
        [HttpGet("id")]
        public ActionResult<blood> GetBloodById(int id)
        {
            if (bloods.Count <= 0) return NotFound("Empty no Data Entered");
            var blood = bloods.Find(i => i.Id == id);
            if (blood == null) return NotFound("No data with id found");
            return blood;
        }



        //logic for update
        [HttpPut("id")]
        public ActionResult<blood> updateBloodById(int id, string? newStatus, string? BloodType, int? age, string? mobNo, string? DonorName)
        {

   
            if (newStatus != null && !Statuscheck(newStatus))
            {
                return BadRequest("Status can only be - Availabe, Requested or Expired");
            }
            if (BloodType != null && !BloodTypecheck(BloodType))
            {
                return BadRequest("Enter a valid blood type in capital alphabet with sign after it ");
            }
            if (mobNo != null && !MobnoCheck(mobNo))
            {
                return BadRequest("Enter a valid Mobile Number");
            }
            if (age != null && age <= 0)
            {
                return BadRequest("Check if age or quantity is negative or zero or else Donor Name not provided");
            }


            //no data
            if (bloods.Count <= 0) return NotFound("Empty no Data Entered");


            //update data
            var blood = bloods.Find(i => i.Id == id);
            if (blood == null) return NotFound("No data with id found");
            if (newStatus != null) blood.Status = newStatus;
            if (mobNo != null) blood.ContactInfo = mobNo;
            if (DonorName != null) blood.DonorName = DonorName;
            if (age != null) blood.age = (int)age;
            if (BloodType != null) blood.BloodType = BloodType;

            return blood;
        }


       
        [HttpDelete]
        public ActionResult<IEnumerable<blood>> DeleteBloodById(int id)
        {

            if (bloods.Count <= 0) return NotFound("Empty no Data Entered");
            var blood = bloods.Find(i => i.Id == id);
            if (blood == null) return NotFound("No data with id found");

            //remove
            bloods.Remove(blood);
            return bloods;
        }


        //pagination
        [HttpGet("getPageData")]
        public ActionResult<IEnumerable<blood>> getPageData(int page = 1, int size = 10)
        {
            var res = bloods.Skip((page - 1) * size).Take(size).ToList();
            return res;
        }



        [HttpGet("bloodtype")]
        public ActionResult<IEnumerable<blood>> SearchByBlood(string bloodtype)
        {
            if (!BloodTypecheck(bloodtype))
            {
                return BadRequest("Enter a valid blood type in capital alphabet with sign after it ");
            }
            var bloodRes = bloods.Where(i => i.BloodType == bloodtype).ToList();
            if (bloodRes == null) return NotFound();
            return bloodRes;
        }

        [HttpGet("status")]
        public ActionResult<IEnumerable<blood>> SearchByStatus(string status)
        {
            if (!Statuscheck(status))
            {
                return BadRequest("Status can only be - Availabe, Requested or Expired");
            }
            var bloodRes = bloods.Where(i => i.Status == status).ToList();
            if (bloodRes == null) return NotFound();
            return bloodRes;
        }
        [HttpGet("name")]
        public ActionResult<IEnumerable<blood>> SearchByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name can't be empty");
            }
            var bloodRes = bloods.Where(i => i.DonorName == name).ToList();
            if (bloodRes == null) return NotFound();
            return bloodRes;
        }


        //Extra functionality

        //if bloodlist has data sort by blood type otherwise sort by date.
        [HttpGet("sortByDate")]
        public ActionResult<IEnumerable<blood>> SortByDate()
        {
            if (bloods == null || bloods.Count == 0) return NotFound("No data available.");

            var sortedBloods = bloods.AsQueryable();

            sortedBloods = sortedBloods.OrderBy(i => i.CollectionDate);

            return Ok(sortedBloods);
        }
        [HttpGet("sortByBlood")]
        public ActionResult<IEnumerable<blood>> SortByBlood()
        {
            if (bloods == null || bloods.Count == 0) return NotFound("No data available.");

            var sortedBloods = bloods.AsQueryable();

            sortedBloods = sortedBloods.OrderBy(i => i.BloodType);

            return Ok(sortedBloods);
        }



        [HttpGet("GetFilterdData")]
        public ActionResult<IEnumerable<blood>> GetBloodFilteredData(string? BloodType = null, string? status = null, string? donorName = null)
        {

         
            if (status != null && !Statuscheck(status))
            {
                return BadRequest("Status can only be - Availabe, Requested or Expired");
            }
            if (BloodType != null && !BloodTypecheck(BloodType))
            {
                return BadRequest("Enter a valid blood type in capital alphabet with sign after it ");
            }

           
            if (bloods == null || bloods.Count == 0) return NotFound("No data available.");

            var filteredBloods = bloods.AsQueryable();


          
            if (!string.IsNullOrEmpty(BloodType))
            {
                filteredBloods = filteredBloods.Where(b => b.BloodType.Equals(BloodType, StringComparison.OrdinalIgnoreCase));
            }


            if (!string.IsNullOrEmpty(status))
            {
                filteredBloods = filteredBloods.Where(b => b.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(donorName))
            {
                filteredBloods = filteredBloods.Where(b => b.DonorName.Contains(donorName, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(filteredBloods.ToList());
        }
    }
}