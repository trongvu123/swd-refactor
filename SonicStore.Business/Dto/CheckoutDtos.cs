namespace SonicStore.Business.Dto;
public class CheckoutRequestDto
{
    public string PaymentMethod { get; set; }
    public List<int> CartIds { get; set; }
}

public class CheckoutResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string PaymentUrl { get; set; }
    public decimal TotalPrice { get; set; }
}

public class BuyNowRequestDto
{
    public int StorageId { get; set; }
    public int ReceiveTypeId { get; set; }
    public string Xa { get; set; }
    public string Huyen { get; set; }
    public string Tinh { get; set; }
}