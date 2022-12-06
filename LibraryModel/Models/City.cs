using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace Pascu_Bianca_Lab2.Models
{
    public class City
    {
        public int ID { get; set; }
        public string CityName { get; set; }
      //  public ICollection<Customer>? Customers { get; set; }

    }
}
