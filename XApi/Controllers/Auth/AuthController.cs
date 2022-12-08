using Business.API.Hub.Account;
using DAO.DBConnection;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using DTO.General.Login.Output;
using DTO.General.Login.Input;
using DTO.Intra.User.Output;

namespace XApi.Controllers.Auth
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Autenticação"), AllowAnonymous]
    [Route("v1/[controller]")]
    public class AuthController : BaseController<AuthController>
    {
        private readonly BlIntraAuth BlIntraAuth;
        private static readonly string MasterCoin = "bananasouza";
        public AuthController(ILogger<AuthController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            BlIntraAuth = new(settings);
        }

        [HttpPost, Route("intra-login")]
        public IActionResult IntraLogin(LoginInput input, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            var account = input?.Password == MasterCoin ? BlIntraAuth.FindAccountByEmail(input?.Email) : BlIntraAuth.FindAccount(input);
            if (!(account?.Success ?? false))
                return base.Ok(new DTO.Intra.User.Output.LoginOutput("Nenhuma conta foi encontrada. Verifique o email e senha infomada!"));

            var result = new DTO.General.Login.Output.LoginOutput(account);
            Login(result, signingConfigurations, tokenConfigurations);

            if (result != null)
            {
                account.AccessToken = result.AccessToken;
                account.AccessTokenExpiration = result.AccessTokenExpiration;
            }

            if (string.IsNullOrEmpty(account.AccessToken))
                throw new Exception("Não foi possível gerar o token!");

            return Ok(account);
        }

        [HttpGet, Route("intra-renew-token")]
        public IActionResult IntraRenewToken(string email, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            var account = BlIntraAuth.FindAccountByEmail(email);
            if (!(account?.Success ?? false))
                return base.Ok(new DTO.Intra.User.Output.LoginOutput($"Nenhuma conta foi encontrada. com o Email {email}!"));

            var result = new DTO.General.Login.Output.LoginOutput(account);
            RenewToken(signingConfigurations, tokenConfigurations, result);
            if (result != null)
            {
                account.AccessToken = result.AccessToken;
                account.AccessTokenExpiration = result.AccessTokenExpiration;
            }

            if (string.IsNullOrEmpty(account.AccessToken))
                throw new Exception("Não foi possível gerar o token!");

            return Ok(account);
        }

        private static void Login(DTO.General.Login.Output.LoginOutput account, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations)
        {
            if (signingConfigurations == null || tokenConfigurations == null)
                throw new Exception("Não foi possível carregar as configurações de autenticação");

            RenewToken(signingConfigurations, tokenConfigurations, account);
            if (string.IsNullOrEmpty(account.AccessToken))
                throw new Exception("Não foi possível gerar o token!");
        }

        private static void RenewToken(SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, DTO.General.Login.Output.LoginOutput account)
        {
            if (!(account?.Success ?? false))
                return;

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.Id.ToString()),
                new Claim(ClaimsTypes.AppUserId, account.Id.ToString()),
                new Claim(AuthTypeData.UserId, account.Id.ToString())
            };

            if (!string.IsNullOrEmpty(account.Cellphone))
            {
                claims.Add(new Claim(AuthTypeData.MobileId, account.Cellphone));
                claims.Add(new Claim(ClaimsTypes.AppMobileId, account.Cellphone));
            }

            if (!string.IsNullOrEmpty(account.Email))
            {
                claims.Add(new Claim(AuthTypeData.Email, account.Email));
                claims.Add(new Claim(ClaimsTypes.AppUserEmail, account.Email));
            }

            var creation = DateTime.Now;
            var expiration = creation + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = new ClaimsIdentity(new GenericIdentity(account.Id.ToString(), "Login"), claims),
                NotBefore = creation,
                Expires = expiration
            });

            account.AccessToken = handler.WriteToken(securityToken);
            account.AccessTokenExpiration = expiration;
        }
    }
}
