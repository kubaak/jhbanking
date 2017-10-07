using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;

namespace JH_Banking.Models
{
    public class JH_DBContext : DbContext
    {
        public JH_DBContext()
        {
            Database.SetInitializer(new DBInitializer());
        }
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Template>    Template { get; set; }

        public void spCreateBankAccount(float balance,int userId)
        {
            this.Database.ExecuteSqlCommand("spCreateBankAccount @Balance,@UserId",
                new SqlParameter("@Balance", balance),
                new SqlParameter("@UserId", userId)
                );
        }

        public void spUpdateBankAccount(BankAccount ba)
        {
            this.Database.ExecuteSqlCommand("spUpdateBankAccount @AccId,@Balance,@CardNum",
                new SqlParameter("@Balance", ba.Balance),
                new SqlParameter("@CardNum", ba.CardNum),
                new SqlParameter("@AccId", ba.AccId)
                );
        }

        public string spCreateUser(string firstName, string lastName, string pidNum, string placeOfBirth, string citizenship, string street, string numberOfDescriptive, string town, string zip, string email, string phone, string mailStreet, string mailNumberOfDescriptive, string mailTown, string mailZip, string password, bool admin)
        {
            var generatedUsername = new SqlParameter();
            generatedUsername.ParameterName = "@generatedUsername";
            generatedUsername.Direction = System.Data.ParameterDirection.Output;
            generatedUsername.SqlDbType = System.Data.SqlDbType.BigInt;

            this.Database.ExecuteSqlCommand("spCreateUser @firstName,@lastName,@pidNum,@placeOfBirth,@citizenship,@street,@numberOfDescriptive,@town,@zip,@email,@phone,@mailStreet,@mailNumberOfDescriptive,@mailTown,@mailZip,@password,@admin,@generatedUsername OUT",
                new SqlParameter("@firstName", firstName),
                new SqlParameter("@lastName", lastName),
                new SqlParameter("@pidNum", pidNum),
                new SqlParameter("@placeOfBirth", placeOfBirth),
                new SqlParameter("@citizenship", citizenship),
                new SqlParameter("@street", street),
                new SqlParameter("@numberOfDescriptive", numberOfDescriptive), 
                new SqlParameter("@town", town),
                new SqlParameter("@zip", zip),
                new SqlParameter("@email", email),
                new SqlParameter("@phone", phone),
                new SqlParameter("@mailStreet", mailStreet == null ? "": mailStreet),
                new SqlParameter("@mailNumberOfDescriptive", mailNumberOfDescriptive == null ? "" : mailNumberOfDescriptive),
                new SqlParameter("@mailTown", mailTown == null ? "" : mailTown),
                new SqlParameter("@mailZip", mailZip == null ? "" : mailZip),
                new SqlParameter("@password", password),
                new SqlParameter("@admin", admin ),
                generatedUsername);
            return generatedUsername.Value.ToString();
        }

        public void spUpdateUser(UserAccount ua)
        {
            this.Database.ExecuteSqlCommand("spUpdateUser @userId,@userName,@firstName,@lastName,@pidNum,@placeOfBirth,@citizenship,@street,@numberOfDescriptive,@town,@zip,@email,@phone,@mailStreet,@mailNumberOfDescriptive,@mailTown,@mailZip,@password,@admin",
                new SqlParameter("@userId", ua.UserId),
                new SqlParameter("@userName", ua.UserName),
                new SqlParameter("@firstName", ua.FirstName),
                new SqlParameter("@lastName", ua.LastName),
                new SqlParameter("@pidNum", ua.PidNum),
                new SqlParameter("@placeOfBirth", ua.PlaceOfBirth),
                new SqlParameter("@citizenship", ua.Citizenship),
                new SqlParameter("@street", ua.Street),
                new SqlParameter("@numberOfDescriptive", ua.NumberOfDescriptive),
                new SqlParameter("@town", ua.Town),
                new SqlParameter("@zip", ua.Zip),
                new SqlParameter("@email", ua.Email),
                new SqlParameter("@phone", ua.Phone),
                new SqlParameter("@mailStreet", ua.MailStreet == null ? "" : ua.MailStreet),
                new SqlParameter("@mailNumberOfDescriptive",ua.MailNumberOfDescriptive == null ? "" : ua.MailNumberOfDescriptive),
                new SqlParameter("@mailTown", ua.MailTown == null ? "" : ua.MailTown),
                new SqlParameter("@mailZip", ua.MailZip == null ? "" : ua.MailZip),
                new SqlParameter("@password", ua.Password),
                new SqlParameter("@admin", ua.Admin)
                );
        }
    }

    public class DBInitializer : DropCreateDatabaseIfModelChanges<JH_DBContext> 
        //DropCreateDatabaseAlways<JH_DBContext>
    {
        protected override void Seed(JH_DBContext context)
        {
            context.UserAccount.Add(new UserAccount()
            {
                UserName = "Admin001",
                Password = JH_Banking.Models.Helpers.PasswordHelper.HashPassword("1234"),
                FirstName = "a",
                LastName = "a",
                PidNum = "990203/6443",
                Email = "a@a.com",
                PlaceOfBirth = "a",
                Citizenship = "a",
                Street = "a",
                NumberOfDescriptive = "a",
                Town = "a",
                Zip = "30100",
                Phone = "777888999",
                Admin = true
            });
            context.SaveChanges();
            context.UserAccount.Add(new UserAccount()
            {
                UserName = "User0001",
                Password = JH_Banking.Models.Helpers.PasswordHelper.HashPassword("0001"),
                FirstName = "user1",
                LastName = "user1",
                PidNum = "990206/6883",
                Email = "user1@a.com",
                PlaceOfBirth = "a",
                Citizenship = "a",
                Street = "a",
                NumberOfDescriptive = "a",
                Town = "Plzeň",
                Zip = "30100",
                Phone = "777888999",
                Admin = false
            });           

            context.UserAccount.Add(new UserAccount()
            {
                UserName = "User0002",
                Password = JH_Banking.Models.Helpers.PasswordHelper.HashPassword("0002"),
                FirstName = "user2",
                LastName = "user2",
                PidNum = "991212/6589",
                Email = "user2@mail.com",
                PlaceOfBirth = "no",
                Citizenship = "ČR",
                Street = "a",
                NumberOfDescriptive = "123",
                Town = "Plzeň",
                Zip = "30100",
                Phone = "777888999",
                Admin = false
            });

            context.UserAccount.Add(new UserAccount()
            {
                UserName = "10000001",
                Password = JH_Banking.Models.Helpers.PasswordHelper.HashPassword("0002"),
                FirstName = "a",
                LastName = "a",
                PidNum = "9910101688",
                Email = "a@a.com",
                PlaceOfBirth = "a",
                Citizenship = "a",
                Street = "a",
                NumberOfDescriptive = "a",
                Town = "Plzeň",
                Zip = "30100",
                Phone = "777888999",
                Admin = false
            });
            context.SaveChanges(); // musí tady být, jinak se neprovede insert pod
            context.BankAccount.Add(new BankAccount()
            {
                AccNum = "100000000",
                Balance = 100000,
                CardNum = "315789512549625",
                UserId = 2
            });
            context.BankAccount.Add(new BankAccount()
            {
                AccNum = "100000001",
                Balance = 20000,
                CardNum = "315789512549626",
                UserId = 3
            });
            context.SaveChanges();

            context.Transaction.Add(new Transaction()
            {
                AccNum = "100000000",
                Date = DateTime.Now,
                DestAccNum = "100000001",
                DestBankCode = "9999",
                Amount = 250,
                DueDate = DateTime.Now,
            });

            context.Transaction.Add(new Transaction()
            {
                AccNum = "100000000",
                Date = DateTime.Now,
                DestAccNum = "1761672113",
                DestBankCode = "0800",
                Amount = 250,
                DueDate = DateTime.Now,
            });

            context.Transaction.Add(new Transaction()
            {
                AccNum = "100000001",
                Date = DateTime.Now,
                DestAccNum = "100000000",
                DestBankCode = "9999",
                Amount = 250,
                DueDate = DateTime.Now,
            });

            context.Transaction.Add(new Transaction()
            {
                AccNum = "100000001",
                Date = DateTime.Now,
                DestAccNum = "1761672113",
                DestBankCode = "0800",
                Amount = 250,
                DueDate = DateTime.Now,
            });
            // executes script that creates stored procedures
            context.Database.ExecuteSqlCommand(Properties.Resources.spCreateUser);
            context.Database.ExecuteSqlCommand(Properties.Resources.spCreateBankAccount);
            context.Database.ExecuteSqlCommand(Properties.Resources.spUpdateUser);
            context.Database.ExecuteSqlCommand(Properties.Resources.spUpdateBankAccount);
            context.SaveChanges();
        }
    }
}