using QuizApi.Dtos.Taker;
using QuizApi.Dtos.Token;
using QuizApi.Models;

namespace QuizApi.Services.Tokens
{
    public interface ITokenService
    {
        /// <summary>
        /// Creates a token for a Taker to use for authentication
        /// </summary>
        /// <param name="taker"></param>
        /// <returns>Generated token</returns>
        public string CreateToken(TakerUserNameDto taker);

        /// <summary>
        ///  Creates a refresh token for a Taker to use for authentication
        /// </summary>
        /// <returns>RefreshToken</returns>
        public RefreshToken GenerateRefreshToken();

        /// <summary>
        /// Verifies if token is still valid
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns> Valid, Invalid, or Expired</returns>
        public string Verify(string refreshToken, TakerUserNameDto taker);
    }
}
