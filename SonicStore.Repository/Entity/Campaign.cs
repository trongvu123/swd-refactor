using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SonicStore.Repository.Entity;
public class Campaign
{
    [Key]
    [Column("id")]
    public int CampaignId { get; set; }

    [Column("title")]
    public string Title { get; set; } = default!;

    [Column("description")]
    public string Description { get; set; } = default!;

    [Column("status")]
    public string Status { get; set; } = default!;

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("approved_by")]
    public int? ApprovedBy { get; set; }

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Campaigns")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("Campaign")]
    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
