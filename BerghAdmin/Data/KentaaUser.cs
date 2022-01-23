using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public class KentaaUser
{
    public KentaaUser()
    { }

    public KentaaUser(Donation donation)
    {
        Update(donation);
    }
    public void Update(Donation donation)
    {
    }



}
