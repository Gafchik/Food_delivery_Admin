using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Food_delivery_library
{
    public partial class User
    {
        public int User_Id { get; set; }
        public string User_Name { get; set; }
        public string User_Surname { get; set; }
        public string User_Phone { get; set; }
        public string User_Bank_card { get; set; }
        public string User_Email { get; set; }
        public string User_Temp_password { get; set; }
    }
    public partial class User_Repository : IRepository<User>
    {
        string connectionString = Resource.ConSTR;

        public void Create(User value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO  Users (User_Name ,User_Surname, User_Phone, User_Bank_card,User_Email,User_Temp_password)" +
                                 "VALUES(@User_Name, @User_Surname, @User_Phone, @User_Bank_card ,@User_Email,@User_Temp_password);";
                        db.Execute(sqlQuery,
                           new
                           {
                               User_Name = value.User_Name,
                               User_Surname = value.User_Surname,
                               User_Phone = value.User_Phone,
                               User_Bank_card = value.User_Bank_card,
                               User_Email = value.User_Email,
                               User_Temp_password = value.User_Temp_password
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

        public void Delete(User value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  Users WHERE User_Id = @User_Id";
                        db.Execute(sqlQuery, new { value.User_Id }, transaction);
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

        public User Get(int Id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> GetColl()
        {
            List<User> coll = new List<User>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        coll = db.Query<User>("SELECT * FROM Users", transaction).ToList();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            return coll;
        }

        public void Update(User value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  Users SET User_Name = @User_Name ,User_Surname =  @User_Surname," +
                            " User_Phone =  @User_Phone, User_Bank_card = @User_Bank_card)" +
                            " User_Email =  @User_Email, User_Temp_password = @User_Temp_password)" +
                            " WHERE User_Id = @User_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               User_Name = value.User_Name,
                               User_Surname = value.User_Surname,
                               User_Phone = value.User_Phone,
                               User_Bank_card = value.User_Bank_card,
                               User_Email = value.User_Email,
                               User_Temp_password = value.User_Temp_password,
                               User_Id = value.User_Id
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
}

