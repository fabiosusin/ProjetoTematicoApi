using DAO.DBConnection;
using DAO.Intra.UserDAO;
using DTO.General.Base.Api.Output;
using DTO.General.Email.Input;
using DTO.General.Login.Input;
using DTO.Intra.User.Database;
using DTO.Intra.User.Input;
using DTO.Intra.User.Output;
using Services.Mobile.Email;
using System;
using System.Linq;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Hub.Account
{
    public class BlIntraAuth
    {
        private readonly AppUserDAO UserDAO;

        public BlIntraAuth(XDataDatabaseSettings settings)
        {
            UserDAO = new(settings);
        }

        public LoginOutput FindAccount(LoginInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Email))
                return new("Email não informado!");

            if (string.IsNullOrEmpty(input.Password))
                return new("Senha não informada!");

            var account = UserDAO.FindOne(x => x.Email == input.Email && x.Password == input.Password);
            if (account == null)
                account = UserDAO.FindOne(x => x.Email == input.Email && x.TempPassword == input.TempPassword);

            if (!string.IsNullOrEmpty(account?.TempPassword) && account.Password == input.Password)
            {
                account.TempPassword = null;
                _ = UserDAO.Update(account);
            }

            return account == null ? new("Não foi possível encontrar o usuário!") : new(account);
        }

        public LoginOutput FindAccountByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new("Email não informado!");

            if (!email.IsValidEmail())
                return new("Email inválido informado!");

            var account = UserDAO.FindOne(x => x.Email == email);
            return account == null ? new("Não foi possível encontrar o usuário!") : new(account);
        }

        public BaseApiOutput UpsertUser(AddUserInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Nome não informado!");

            if (string.IsNullOrEmpty(input.Email))
                return new("Email não informado!");

            if (string.IsNullOrEmpty(input.Username))
                return new("Nome de Usuário não informado!");

            if (input.UserId == 0)
            {
                if (string.IsNullOrEmpty(input.Password))
                    return new("Senha não informada!");

                if (FindAccountByEmail(input.Email).Success)
                    return new("Usuário já cadastrado com este Email!");

                if (UserDAO.FindOne(x => x.Username == input.Username) != null)
                    return new("Usuário já cadastrado com este nome!");

            }

            if (!string.IsNullOrEmpty(input.Password) && input.Password != input.PasswordValidation)
                return new("As senhas não coincidem!");

            var permissions = input.Permissions;

            if (input.IsMasterAdmin)
            {
                var masterUser = UserDAO.FindOne(x => x.IsMasterAdmin == true);
                if (masterUser?.Id != 0 && masterUser.Id != input.UserId)
                    return new("Usuário admin já cadastrado!");
            }

            var result = input.UserId == 0 ? UserDAO.Insert(new AppUser(input)) : UserDAO.Update(new AppUser(input));
            if (result == null)
                return new("Não foi possível salvar o usuário!");

            return new(true);
        }

        public async Task<BaseApiOutput> SendTempPassword(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                return new("Email de usuário não informado!");

            var account = UserDAO.FindOne(x => x.Email == userEmail);
            if (account == null)
                return new("Usuário não encontrado!");

            account.TempPassword = StringExtension.RandomString(6, true);
            var msg = "Sua senha temporária de acesso ao App é: " + account.TempPassword;
            var sendEmail = await new EmailService().SendEmail(new EmailRequestInput(account.Email, "Senha Temporária", msg)).ConfigureAwait(false);
            if (sendEmail)
            {
                _ = UserDAO.Update(account);
                return new BaseApiOutput(true, "Senha temporária enviada no Email: " + account.Email);
            }

            return new BaseApiOutput(false, "Não foi possível enviar a senha temporária para o Email: " + account.Email);
        }

        public BaseApiOutput DeleteUser(int id)
        {
            if (id == 0)
                return new("Requisição mal formada!");

            var user = UserDAO.FindById(id);
            if (user == null)
                return new("Usuário não encontrado!");

            UserDAO.Remove(user);
            return new(true);
        }

        public UserListOutput List(UserListInput input)
        {
            var result = UserDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Usuário encontrado!");

            return new(result);
        }
    }
}
