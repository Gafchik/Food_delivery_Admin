using Dapper;
using Food_delivery_library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Food_delivery_library
{
    public class Blocked_user
    {
        public int Blocked_user_Id { get; set; }
        public string Blocked_user_Name { get; set; }
        public string Blocked_user_Surname { get; set; }
        public string Blocked_user_Phone { get; set; }
        public string Blocked_user_Bank_card { get; set; }
        public string Blocked_user_Email { get; set; }
        public string Blocked_user_Temp_password { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
public partial class Blocked_users_Repository : IRepository<Blocked_user>
{
    string connectionString = Resource.ConSTR;

    public void Create(Blocked_user value)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            db.Open();
            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    var sqlQuery = "INSERT INTO  Blocked_users (Blocked_user_Name ,Blocked_user_Surname, Blocked_user_Phone, Blocked_user_Bank_card,Blocked_user_Email,Blocked_user_Temp_password)" +
                             "VALUES(@Blocked_user_Name, @Blocked_user_Surname, @Blocked_user_Phone, @Blocked_user_Bank_card ,@Blocked_user_Email,@Blocked_user_Temp_password);";
                    db.Execute(sqlQuery,
                       new
                       {
                           Blocked_user_Name = value.Blocked_user_Name,
                           Blocked_user_Surname = value.Blocked_user_Surname,
                           Blocked_user_Phone = value.Blocked_user_Phone,
                           Blocked_user_Bank_card = value.Blocked_user_Bank_card,
                           Blocked_user_Email = value.Blocked_user_Email,
                           Blocked_user_Temp_password = value.Blocked_user_Temp_password
                       },
                       transaction);
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }

    public void Delete(Blocked_user value)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            db.Open();
            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    var sqlQuery = "DELETE FROM  Blocked_users WHERE Blocked_user_Id = @Blocked_user_Id";
                    db.Execute(sqlQuery, new { value.Blocked_user_Id }, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }

    public Blocked_user Get(int Id)
    {
        Blocked_user item = null;
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            item = db.Query<Blocked_user>("SELECT * FROM Blocked_users WHERE Blocked_user_Id = @Blocked_user_Id", new { Id }).FirstOrDefault();
        }
        return item;
    }

    public IEnumerable<Blocked_user> GetColl()
    {
        List<Blocked_user> coll = new List<Blocked_user>();
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            coll = db.Query<Blocked_user>("SELECT * FROM Blocked_users").ToList();
        }
        return coll;
    }

    public void Update(Blocked_user value)
    {
        using (IDbConnection db = new SqlConnection(connectionString))
        {
            db.Open();
            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    var sqlQuery = "UPDATE  Blocked_users SET Blocked_user_Name = @Blocked_user_Name, Blocked_user_Surname = @Blocked_user_Surname," +
                        " Blocked_user_Phone =  @Blocked_user_Phone, Blocked_user_Bank_card = @Blocked_user_Bank_card," +
                        " Blocked_user_Email =  @Blocked_user_Email, Blocked_user_Temp_password = @Blocked_user_Temp_password" +
                        " WHERE Blocked_user_Id = @Blocked_user_Id";
                    db.Execute(sqlQuery,
                       new
                       {
                           Blocked_user_Name = value.Blocked_user_Name,
                           Blocked_user_Surname = value.Blocked_user_Surname,
                           Blocked_user_Phone = value.Blocked_user_Phone,
                           Blocked_user_Bank_card = value.Blocked_user_Bank_card,
                           Blocked_user_Email = value.Blocked_user_Email,
                           Blocked_user_Temp_password = value.Blocked_user_Temp_password,
                           Blocked_user_Id = value.Blocked_user_Id
                       },
                       transaction);
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}

  
