
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Food_delivery_library
{
  
    public partial class Admin : INotifyPropertyChanged
    {     
        public int Admins_Id { get; set; }
        public string Admins_Login { get; set; }
        public string Admins_Password { get; set; }
        public string Admins_Name { get; set; }
        public string Admins_Surname { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    public  partial class Admins_Repository : IRepository<Admin>
    {


      //  string connectionString = Resource.ConSTR;
        string connectionString = ConfigurationManager.ConnectionStrings["ConSTR"].ConnectionString;
        public IEnumerable<Admin> GetColl()
        {
            List<Admin> coll = new List<Admin>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                coll = db.Query<Admin>("SELECT * FROM Admins").ToList();             
            }
            return coll;
        }

        public Admin Get(int Id)
        {
            Admin  item = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                item = db.Query<Admin>("SELECT * FROM Admins WHERE Admins_Id = @Admins_Id", new { Id }).FirstOrDefault();
            }
            return  item;
        }

        public void Create(Admin value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO  Admins (Admins_Login ,Admins_Password, Admins_Name, Admins_Surname)" +
                                 "VALUES(@Admins_Login, @Admins_Password, @Admins_Name, @Admins_Surname); SELECT CAST(SCOPE_IDENTITY() as int)";
                        db.Execute(sqlQuery,
                           new
                           {
                                Admins_Login = value.Admins_Login,
                                Admins_Password = value.Admins_Password,
                                Admins_Name = value.Admins_Name,
                                Admins_Surname = value.Admins_Surname
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

        public void Update(Admin value)
        {

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE[dbo].[Admins]"+
                            "SET[Admins_Login] =  @Admins_Login,"+
                            " [Admins_Password] = @Admins_Password,"+
                            "[Admins_Name] = @Admins_Name,"+
                            "[Admins_Surname] = @Admins_Surname"+
                            " WHERE [dbo].[Admins].[Admins_Id] = @Admins_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Admins_Login = value.Admins_Login,
                               Admins_Password = value.Admins_Password,
                               Admins_Name = value.Admins_Name,
                               Admins_Surname = value.Admins_Surname,
                               Admins_Id = value.Admins_Id
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

        public void Delete(Admin value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  Admins WHERE Admins_Id = @Admins_Id";
                        db.Execute(sqlQuery, new { value.Admins_Id }, transaction);
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
