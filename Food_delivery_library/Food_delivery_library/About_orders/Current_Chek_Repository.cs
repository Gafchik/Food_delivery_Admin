using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Food_delivery_library.About_orders
{

    public class Current_Cheсk
    {
        public int Id { get; set; }
        public int Check_Id { get; set; }
        public string Check_Admin { get; set; }
        public string Check_User_Phone { get; set; }
        public DateTime Check_Date { get; set; }
        public float Check_Final_Price { get; set; }
    }
    public class Current_Chek_Repository : IRepository<Current_Cheсk>
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConSTR"].ConnectionString;
        public void Create(Current_Cheсk value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO  Current_Cheсk (Check_Id, Check_Admin ,Check_User_Phone, Check_Date, Check_Final_Price)" +
                                 " VALUES(@Check_Id, @Check_Admin, @Check_User_Phone, @Check_Date, @Check_Final_Price)";
                        db.Execute(sqlQuery,
                           new
                           {
                               Check_Id = value.Check_Id,
                               Check_Admin = value.Check_Admin,
                               Check_User_Phone = value.Check_User_Phone,
                               Check_Date = value.Check_Date,
                               Check_Final_Price = value.Check_Final_Price

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

        public void Delete(Current_Cheсk value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM Current_Cheсk WHERE Id = @Id";
                        db.Execute(sqlQuery, new { Id = value.Id }, transaction);
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

        public Current_Cheсk Get(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Current_Cheсk> GetColl()
        {
            List<Current_Cheсk> coll = new List<Current_Cheсk>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                coll = db.Query<Current_Cheсk>("SELECT * FROM Current_Cheсk ").ToList();
            }

            return coll;
        }

        public void Update(Current_Cheсk value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  Current_Cheсk SET Check_Admin = @Check_Admin, Check_User_Phone =  @Check_User_Phone," +
                            " Check_Date =  @Check_Date, Check_Final_Price = @Check_Final_Price, Check_Id = @Check_Id" +
                            " WHERE Id = @Id ";
                        db.Execute(sqlQuery,
                           new
                           {
                               Id = value.Id,
                               Check_Id = value.Check_Id,
                               Check_Admin = value.Check_Admin,
                               Check_User_Phone = value.Check_User_Phone,
                               Check_Final_Price = value.Check_Final_Price,
                               Check_Date = value.Check_Date
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
