using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicStore.Business.Dto;
public class OrderListDto
{
    public int Index { get; set; }
    public string CusName { get; set; }
    public string PaymentStatus { get; set; }
    public string OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
}