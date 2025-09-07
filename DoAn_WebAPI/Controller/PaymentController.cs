using DoAn_WebAPI.Interfaces.IService;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;
using DoAn_WebAPI.Models.Momo;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IMomoService _momoService;
    private readonly IOrderService _orderService;

    public PaymentController(IMomoService momoService, IOrderService orderService)
    {
        _momoService = momoService;
        _orderService = orderService;
    }

    [HttpPost("momo")]
    public async Task<IActionResult> CreateMomoPayment([FromBody] OrderInfoModel model)
    {
        var response = await _momoService.CreatePaymentAsync(model);
        if (response == null || string.IsNullOrEmpty(response.PayUrl))
            return BadRequest("Không tạo được thanh toán MoMo");

        return Ok(new { payUrl = response.PayUrl, orderId = response.OrderId });
    }

    [HttpGet("momo-return")]
    public async Task<IActionResult> MomoReturn()
    {
        var result = await _momoService.PaymentExecuteAsync(Request.Query);
        return Ok(result);
    }
    [HttpPost("momo-notify")]
    public async Task<IActionResult> MomoNotify([FromBody] MomoNotifyModel notify)
    {
        // if (!_momoService.VerifySignature(notify))
        //     return BadRequest(new { message = "Invalid signature" });

        if (notify.ErrorCode == "0")
        {
            await _orderService.MarkAsPaidAsync(notify.OrderId);
            return Ok(new { message = "Payment success", orderId = notify.OrderId });
        }
        else
        {
            await _orderService.MarkAsFailedAsync(notify.OrderId);
            return Ok(new { message = "Payment failed", orderId = notify.OrderId });
        }
    }

}
