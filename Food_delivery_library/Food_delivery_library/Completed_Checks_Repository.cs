namespace Food_delivery_library
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class Completed_Сheck
    {
     
        public int Completed_Сhecks_Id { get; set; }

        public int Completed_Checks_Admin_Id { get; set; }

        public int Completed_Сhecks_User_Id { get; set; }
       
        public DateTime Completed_Сhecks_Date { get; set; }

        public virtual Admin admin { get; set; }

        public virtual User user { get; set; }

      
       
    }

    public partial class Completed_Сhecks_Repository : IRepository<Completed_Сheck>
    {
        string connectionString = Resource.ConSTR;
       
        public void Create(Completed_Сheck value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO [dbo].[Completed_Сhecks]" +
           "([Completed_Checks_Admin_Id], [Completed_Сhecks_User_Id], [Completed_Сhecks_Date])" +
     "VALUES (@Completed_Checks_Admin_Id,  @Completed_Сhecks_User_Id,  @Completed_Сhecks_Date)";

                            db.Execute(sqlQuery,
                           new
                           {
                               Completed_Checks_Admin_Id = value.Completed_Checks_Admin_Id,
                               Completed_Сhecks_User_Id = value.Completed_Сhecks_User_Id,
                               Completed_Сhecks_Date = value.Completed_Сhecks_Date,                              
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

        public void Delete(Completed_Сheck value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  [dbo].[Completed_Сhecks] WHERE Completed_Сhecks_Id = @Completed_Сhecks_Id";
                        db.Execute(sqlQuery, new { value.Completed_Сhecks_Id }, transaction);
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

        public Completed_Сheck Get(int Id)
        {
            Completed_Сheck item = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                item = db.Query<Completed_Сheck>("SELECT * FROM [dbo].[Completed_Сhecks] WHERE Completed_Сhecks_Id = @Completed_Сhecks_Id", new { Id }).FirstOrDefault();
            }         
            return item;
        }

        public IEnumerable<Completed_Сheck> GetColl()
        {
            List<Completed_Сheck> coll = new List<Completed_Сheck>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                coll = db.Query<Completed_Сheck>("SELECT * FROM [dbo].[Completed_Сhecks]").ToList();
            }           
            return coll;
        }

        public void Update(Completed_Сheck value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {                    
                        var sqlQuery = "UPDATE  [dbo].[Completed_Сhecks] SET Completed_Checks_Admin_Id = @Completed_Checks_Admin_Id ,Completed_Сhecks_User_Id =  @Completed_Сhecks_User_Id," +
                            " Completed_Сhecks_Date =  @Completed_Сhecks_Date)" +
                            " WHERE Completed_Сhecks_Id = @Completed_Сhecks_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Completed_Сhecks_Id = value.Completed_Сhecks_Id,
                               Completed_Checks_Admin_Id = value.Completed_Checks_Admin_Id,
                               Completed_Сhecks_User_Id = value.Completed_Сhecks_User_Id,
                               Completed_Сhecks_Date = value.Completed_Сhecks_Date,                            
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
