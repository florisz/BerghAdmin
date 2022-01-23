using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public class KentaaProject
{
    public KentaaProject()
    { }

    public KentaaProject(Donation donation)
    {
        Update(donation);
    }
 
    public void Update(Donation donation)
    {
    }

}