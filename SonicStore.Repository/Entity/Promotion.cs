using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SonicStore.Repository.Entity;
public class Promotion
{
    [Key]
    [Column("promotion_id")]
    public int PromotionId { get; set; }
    [Column("promotion_name")]
    public string PromotionName { get; set; }
    [Column("start_date")]
    public DateTime? StartDate { get; set; }
    [Column("end_date")]
    public DateTime? EndDate { get; set; }
    [Column("minimum_purchase")]
    public decimal? MinimumPurchase { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("updated_by")]
    public int UpdatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("CreatedPromotions")]
    public virtual User? CreatedByUser { get; set; }

    [ForeignKey("UpdatedBy")]
    [InverseProperty("UpdatedPromotions")]
    public virtual User? UpdatedByUser { get; set; }
}
