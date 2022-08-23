namespace Chat.API.Models.Responses
{
    public class AuthenticatedUserResponce
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Срок действия токена доступа в минутах
        /// </summary>
        public double AccessTokenExpirationMinutes { get; set; }

        /// <summary>
        /// Токен для обновления AccessToken 
        /// 
        /// (если AccessToken потерялся или срок его действия закончился)
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Срок действия токена для обновления
        /// </summary>
        public double RefreshTokenExpirationMinutes { get; set; }

        public AuthenticatedUserResponce(string accessToken, double accessTokenExpirationMinutes, string refreshToken, double refreshTokenExpirationMinutes)
        {
            AccessToken = accessToken;
            AccessTokenExpirationMinutes = accessTokenExpirationMinutes;
            RefreshToken = refreshToken;
            RefreshTokenExpirationMinutes = refreshTokenExpirationMinutes;
        }
    }
}
