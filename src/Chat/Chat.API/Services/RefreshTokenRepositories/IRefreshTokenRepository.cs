using Chat.API.Models;

namespace Chat.API.Services.RefreshTokenRepositories
{
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Получить детальные данные по токену
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<RefreshToken> GetByToken(string token);

        /// <summary>
        /// Создать токен в хранилище
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task Create(RefreshToken refreshToken);

        /// <summary>
        /// Удалить токен по ключу Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Очистить токены с истёкшим сроком действия в хранилище
        /// </summary>
        /// <returns></returns>
        Task ClearCache();
    }
}
