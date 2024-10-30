using HouseRepCore;
using Dapper.Contrib.Extensions;

namespace DTO;

[Table("BillType")]
public class BillType : ClassBase
{

    public BillType()
    {
    }


    #region "Properties ------------------------------------------------------------------------"

    [Dapper.Contrib.Extensions.Key]
    public int BillTypeID { get; set; }

    public string? Dataset { get; set; }

    public string? Description { get; set; }

    public string? BillNumber { get; set; }

    public string? TypeOfBill { get; set; }

    public string? SessionYear { get; set; }

    public string? SessionType { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeleteDate { get; set; }

    [Write(false)]
    public string FullDescription
    {
        get
        {
            if (BillNumber != null)
            {
                if (SessionType == null || SessionType.ToLower() == "rs")
                    return TypeOfBill + " " + BillNumber + " " + Description + " " + SessionYear;
                else
                    return TypeOfBill + " " + BillNumber + " " + Description + " " + SessionYear + " " + SessionType;
            }
            else
                return "";
        }
    }

    #endregion


}
