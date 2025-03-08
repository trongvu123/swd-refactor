using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SonicStore.Repository.Entity;
public class Budget
{
    [Key]
    [Column("id")]
    public int BudgetId { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
    public int CreatedBy { get; set; }
    public int CampaignId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [ForeignKey("CampaignId")]
    [InverseProperty("Budgets")]
    public virtual Campaign Campaign { get; set; } = null!;
}
