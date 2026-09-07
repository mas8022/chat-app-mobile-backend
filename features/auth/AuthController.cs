using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.common;
using backend.features.auth.dtos;
using backend.features.auth.dtos.sendOtp;
using Microsoft.AspNetCore.Mvc;

namespace backend.features.auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AuthService authService) : ControllerBase
    {
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(SendOtpDto dto)
        {
            return Ok(await authService.SendOtp(dto.Phone));
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        {
            var result = await authService.VerifyOtp(dto);

            return Ok(new Result
            {
                Status = result.Status,
                Message = result.Message,
                Data = new
                {
                    accessToken = result.AccessToken,
                    sessionId = result.SessionId
                }
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromHeader(Name = "access_token")] string? accessToken,
         [FromHeader(Name = "session_id")] string? sessionId)
        {
            var result = await authService.RefreshToken(accessToken, sessionId);
            return Ok(new Result
            {
                Status = result.Status,
                Message = result.Message,
                Data = new
                {
                    AccessToken = result.AccessToken,
                    IsAccess = result.IsAccess
                }
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromHeader(Name = "session_id")] string? sessionId)
        {
            return Ok(await authService.Logout(sessionId));
        }
    }
}
