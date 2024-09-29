using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace directoryList.Models;
public class PersonModel
{
    public int Id { get; set; }
    
    [Required (ErrorMessage = "First name is required!")]
    public string Name { get; set; }
    
    [Required (ErrorMessage = "Please enter your last name!")]
    public string Surname { get; set; }
    
    public string Number { get; set; }
    
    public List<NoteModel> Notes { get; set; } = new List<NoteModel>();
    
    public List<CallingDetailModel> Calls { get; set; } = new List<CallingDetailModel>();
}
