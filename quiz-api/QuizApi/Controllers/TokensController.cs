using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApi.Dtos.Taker;
using QuizApi.Dtos.Token;
using QuizApi.Models;
using QuizApi.Services.Takers;
using QuizApi.Services.Tokens;

namespace QuizApi.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ILogger<TakersController> _logger;
        private static TakerUserNameDto _taker = new TakerUserNameDto();
        private readonly ITokenService _tokenService;
        private readonly ITakerService _takerService;

        public TokensController(
            ILogger<TakersController> logger,
            ITokenService tokenService,
             ITakerService takerService)
        {
            _logger = logger;
            _tokenService = tokenService;
            _takerService = takerService;
        }

        [HttpPost("acquire")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcquireToken([FromBody] TakerLoginDto takerLogin)
        {
            try
            {
                _taker = await _takerService.GetTakerByUsername(takerLogin.Username);

                string accessToken = _tokenService.CreateToken(_taker);
                var refreshToken = _tokenService.GenerateRefreshToken();
                SetRefreshToken(refreshToken);

                var tokenDto = new TokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token
                };

                return Ok(tokenDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPost("renew")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RenewToken([FromBody] RenewTokenDto request)
        {
            string tokenStatus = _tokenService.Verify(request.RefreshToken, _taker);

            if (tokenStatus == "Invalid")
            {
                return Unauthorized("Invalid refresh token");
            }
            else if (tokenStatus == "Expired")
            {
                return Unauthorized("Refresh token expired");

            }

            string accessToken = _tokenService.CreateToken(_taker);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            };

            return Ok(tokenDto);
        }

        private static void SetRefreshToken(RefreshToken newRefreshToken)
        {
            _taker.RefreshToken = newRefreshToken.Token;
            _taker.TokenCreated = newRefreshToken.Created;
            _taker.TokenExpires = newRefreshToken.Expires;
        }
    }
}
