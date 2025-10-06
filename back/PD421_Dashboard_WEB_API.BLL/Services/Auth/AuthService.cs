﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PD421_Dashboard_WEB_API.BLL.Dtos.Auth;
using PD421_Dashboard_WEB_API.BLL.Settings;
using PD421_Dashboard_WEB_API.DAL.Entitites.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PD421_Dashboard_WEB_API.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtOptions, IMapper mapper)
        {
            _userManager = userManager;
            _jwtSettings = jwtOptions.Value;
            _mapper = mapper;
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Picture, user.Image ?? string.Empty),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty)
            };

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count > 0)
            {
                var roleClaims = roles.Select(role => new Claim("roles", role));
                claims.AddRange(roleClaims);
            }

            string secretKey = _jwtSettings.SecretKey;
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpiresInHours)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ServiceResponse> LoginAsync(LoginDto dto)
        {
            string normalizedLogin = dto.Login.ToUpper();

            var entity = await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedLogin
                || u.NormalizedUserName == normalizedLogin);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Message = "Логін вказано невірно",
                    IsSuccess = false,
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            bool passwordRes = await _userManager.CheckPasswordAsync(entity, dto.Password);

            if (!passwordRes)
            {
                return new ServiceResponse
                {
                    Message = "Пароль вказано невірно",
                    IsSuccess = false,
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var token = await GenerateJwtTokenAsync(entity);

            return new ServiceResponse
            {
                Message = "Успішний вхід",
                Data = token
            };
        }

        public async Task<ServiceResponse> RegisterAsync(RegisterDto dto)
        {
            if(!await IsUniqueEmailAsync(dto.Email))
            {
                return new ServiceResponse
                {
                    IsSuccess = false,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = $"Пошта {dto.Email} вже використовується"
                };
            }

            if (!await IsUniqueUserNameAsync(dto.UserName))
            {
                return new ServiceResponse
                {
                    IsSuccess = false,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = $"Ім'я користувача {dto.UserName} вже використовується"
                };
            }

            var entity = _mapper.Map<ApplicationUser>(dto);

            var result = await _userManager.CreateAsync(entity, dto.Password);

            if(!result.Succeeded)
            {
                return new ServiceResponse
                {
                    IsSuccess = false,
                    Message = result.Errors.First().Description,
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            await _userManager.AddToRoleAsync(entity, "user");

            return new ServiceResponse
            {
                Message = $"Користувач {dto.UserName} успішно зареєстрований"
            };
        }

        private async Task<bool> IsUniqueEmailAsync(string email)
        {
            return !await _userManager.Users.AnyAsync(u => u.NormalizedEmail == email.ToUpper());
        }

        private async Task<bool> IsUniqueUserNameAsync(string userName)
        {
            return !await _userManager.Users.AnyAsync(u => u.NormalizedUserName == userName.ToUpper());
        }
    }
}
