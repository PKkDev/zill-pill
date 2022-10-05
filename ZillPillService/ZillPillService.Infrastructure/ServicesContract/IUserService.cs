using ZillPillService.Domain.DTO.User;
using ZillPillService.Domain.Query.User;
using ZillPillService.Infrastructure.Entities;

namespace ZillPillService.Infrastructure.ServicesContract
{
    public interface IUserService
    {
        /// <summary>
        /// send sms with code to user
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task SendAccesTokenToSmsAsync(string phone, CancellationToken ct);

        /// <summary>
        /// check code from sms and user
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<LoginResponseDto> CheckPhoneAccessTokenAsync(string phone, string code, CancellationToken ct);

        /// <summary>
        /// authentication user on login/pass
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<LoginResponseDto> Authorize(User user);

        /// <summary>
        /// get user detail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<UserDetailDto> GetUserDetailAsync(int userId, CancellationToken ct);

        /// <summary>
        /// update user detail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task UpdateUserDetailAsync(int userId, UpdateUserDetailQuery query, CancellationToken ct);

        /// <summary>
        /// delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task DeleteUserAsync(int userId, CancellationToken ct);
    }
}
