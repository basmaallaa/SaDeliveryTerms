using System;
using System.Collections.Generic;

namespace DeliveryTermsDL.Models.SupplyChain;

public partial class SaDeliveryTerm
{
    public int Code { get; set; }

    public string SName { get; set; } = null!;

    public string BName { get; set; } = null!;

    public int? Days { get; set; }

    public bool? ActiveFlag { get; set; }

    public int EntryUser { get; set; }

    public DateTime EntryDate { get; set; }

    public int? ChangeUser { get; set; }

    public DateTime? ChangeDate { get; set; }
}
