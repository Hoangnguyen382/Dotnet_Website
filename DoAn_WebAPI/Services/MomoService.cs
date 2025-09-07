using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.Momo;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

public class MomoService : IMomoService
{
    private readonly IOptions<MomoOptionModel> _options;

    public MomoService(IOptions<MomoOptionModel> options)
    {
        _options = options;
    }

    public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model)
    {
        model.OrderInfo = "Khách hàng: " + model.FullName + ". Nội dung: " + model.OrderInfo;

        var rawData =
            $"partnerCode={_options.Value.PartnerCode}" +
            $"&accessKey={_options.Value.AccessKey}" +
            $"&requestId={model.OrderId}" +
            $"&amount={model.Amount}" +
            $"&orderId={model.OrderId}" +
            $"&orderInfo={model.OrderInfo}" +
            $"&returnUrl={_options.Value.ReturnUrl}" +
            $"&notifyUrl={_options.Value.NotifyUrl}" +
            $"&extraData=";

        var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

        var client = new RestClient(_options.Value.MomoApiUrl);
        var request = new RestRequest() { Method = Method.Post };
        request.AddHeader("Content-Type", "application/json; charset=UTF-8");

        var requestData = new
        {
            accessKey = _options.Value.AccessKey,
            partnerCode = _options.Value.PartnerCode,
            requestType = _options.Value.RequestType,
            notifyUrl = _options.Value.NotifyUrl,
            returnUrl = _options.Value.ReturnUrl,
            orderId = model.OrderId,
            amount = model.Amount.ToString(),
            orderInfo = model.OrderInfo,
            requestId = model.OrderId,
            extraData = "",
            signature = signature
        };
        request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);
        var momoResponse = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
        return momoResponse;
    }

    public async Task<MomoExecuteResponseModel> PaymentExecuteAsync(IQueryCollection collection)
    {
        var amount = collection.First(s => s.Key == "amount").Value;
        var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
        var orderId = collection.First(s => s.Key == "orderId").Value;
        var fullname = collection.First(s => s.Key == "fullname").Value;

        return new MomoExecuteResponseModel()
        {
            FullName = fullname,
            Amount = amount,
            OrderId = orderId,
            OrderInfo = orderInfo
        };
    }
    // Xác thực signature từ NotifyUrl
    public bool VerifySignature(MomoNotifyModel notify)
    {
        var rawData = $"partnerCode={notify.PartnerCode}&accessKey={notify.AccessKey}&requestId={notify.RequestId}&amount={notify.Amount}&orderId={notify.OrderId}&orderInfo={notify.OrderInfo}&orderType={notify.OrderType}&transId={notify.TransId}&message={notify.Message}&localMessage={notify.LocalMessage}&responseTime={notify.ResponseTime}&errorCode={notify.ErrorCode}&payType={notify.PayType}&extraData={notify.ExtraData}";

        var expectedSignature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

        return notify.Signature == expectedSignature;
    }

    private string ComputeHmacSha256(string message, string secretKey)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(messageBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}
