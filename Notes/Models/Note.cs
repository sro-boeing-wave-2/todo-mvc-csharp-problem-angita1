using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class Note
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string plainText { get; set; }
        public List<CheckList> checklist { get; set; }
        public List<Labels> label { get; set; }
        public bool IsPinned { get; set; }
    }

    public class CheckList
    {
        
        public int ID { get; set; }
        public string value { get; set; }
        public Boolean IsChecked { get; set; }
    }
    public class Labels
    {
       
        public int ID { get; set; }
        public string value { get; set; }
    }
}
