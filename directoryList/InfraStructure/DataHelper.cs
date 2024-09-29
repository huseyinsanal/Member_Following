using System.Data;
using directoryList.Models;
using Microsoft.Extensions.Localization;

namespace directoryList.InfraStructure;

public static class DataHelper
{
    public static List<PersonModel> PersonList { get; set; } = new List<PersonModel>();

    public static void AddNoteToPerson(int personId, NoteModel note)
    {
        var person = PersonList.FirstOrDefault(p => p.Id == personId);

        if (person != null)
        {
            person.Notes.Add(note);
        }
    }

    public static void AddCallToPerson(int personId, CallingDetailModel call)
    {
        var person = PersonList.FirstOrDefault(p => p.Id == personId);

        if (person != null)
        {
            person.Calls.Add(call);
        }
    }
}