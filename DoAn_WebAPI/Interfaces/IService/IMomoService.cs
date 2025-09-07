using DoAn_WebAPI.Models.Momo;
public interface IMomoService
{
    Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
    Task<MomoExecuteResponseModel> PaymentExecuteAsync(IQueryCollection collection);
    bool VerifySignature(MomoNotifyModel notify);
}