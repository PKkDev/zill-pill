using Microsoft.EntityFrameworkCore;
using ZillPillService.Domain.DTO.User;
using ZillPillService.Domain.Exceptions;
using ZillPillService.Domain.Query.User;
using ZillPillService.Infrastructure.Context;
using ZillPillService.Infrastructure.Entities;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDataBaseContext _context;
        private readonly IJwtGenerator _jwtTokenService;
        private readonly IHttpClientFactory _clientFactory;

        public UserService(
            AppDataBaseContext context, IJwtGenerator jwtGenerator, IHttpClientFactory clientFactory)
        {
            _context = context;
            _jwtTokenService = jwtGenerator;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// send sms with code to user
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task SendAccesTokenToSmsAsync(string phone, CancellationToken ct)
        {
            var user = await _context.User
               .FirstOrDefaultAsync(x => x.Phone.Equals(phone), ct);

            if (user == null)
            {
                User newUser = new()
                {
                    Phone = phone,
                    Profile = new UserProfile()
                    {
                        FirstName = null,
                        Email = null
                    }
                };
                await _context.User.AddAsync(newUser, ct);
                await _context.SaveChangesAsync(ct);

                user = await _context.User
                    .FirstOrDefaultAsync(x => x.Phone.Equals(phone), ct);
            }

            var code = GeneratePhoneNumberTokenAsync();

            user.Code = code;

            _context.User.Update(user);
            await _context.SaveChangesAsync(ct);

            //var client = _clientFactory.CreateClient("smsAreaApi");
            //var message = $"код для доступа: {code}";
            //Dictionary<string, string> queryParam = new()
            //{
            //    {"number", $"{phone}"},
            //    {"text", $"{message}"},
            //    {"sign", "SMS Aero"}
            //};
            //var uri = QueryHelpers.AddQueryString(client.BaseAddress.AbsoluteUri, queryParam);
            //var request = new HttpRequestMessage(HttpMethod.Get, uri);
            //var response = await client.SendAsync(request, ct);
            //var responseMessage = await response.Content.ReadAsStringAsync();
            //if (!response.IsSuccessStatusCode)
            //    throw new ApiException("ошибка при отправке sms");
        }

        /// <summary>
        /// check code from sms and user
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<LoginResponseDto> CheckPhoneAccessTokenAsync(
            string phone, string code, CancellationToken ct)
        {
            var user = await _context.User
               .FirstOrDefaultAsync(x => x.Phone.Equals(phone), ct);

            if (user == null)
                throw new ApiException("пользователей не найден");

            //if (code != user.Code)
            //    throw new ApiException("код не совпадает");

            return await Authorize(user);
        }


        /// <summary>
        /// authentication user on login/pass
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<LoginResponseDto> Authorize(
            User user)
        {
            string token = _jwtTokenService.CreateToken(user);
            var result = new LoginResponseDto(token);
            return result;
        }

        /// <summary>
        /// generate 4-digit code
        /// </summary>
        /// <returns></returns>
        private string GeneratePhoneNumberTokenAsync()
        {
            var rnd = new Random();
            var start = rnd.Next(9, 99);
            var end = DateTime.Now.Second;
            return start.ToString() + end.ToString();
        }

        /// <summary>
        /// get user detail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public async Task<UserDetailDto> GetUserDetailAsync(int userId, CancellationToken ct)
        {
            var user = await _context.User
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new ApiException("user not found");

            UserDetailDto result = new()
            {
                Phone = user.Phone,
                Email = user.Profile.Email,
                FirstName = user.Profile.FirstName
            };
            return result;
        }

        /// <summary>
        /// update user detail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public async Task UpdateUserDetailAsync(int userId, UpdateUserDetailQuery query, CancellationToken ct)
        {
            var user = await _context.User
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new ApiException("user not found");

            user.Profile.Email = query.Email;
            user.Profile.FirstName = query.FirstName;

            _context.User.Update(user);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public async Task DeleteUserAsync(int userId, CancellationToken ct)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new ApiException("user not found");

            _context.User.Remove(user);
            await _context.SaveChangesAsync(ct);
        }
    }
}
