namespace Food_delivery_library
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class Current_Ñheck
    {

        public int Current_Ñhecks_Id { get; set; }

        public int Current_Checks_Admin_Id { get; set; }

        public int Current_Ñhecks_User_Id { get; set; }

        public DateTime Current_Ñhecks_Date { get; set; }

        public virtual Admin admin { get; set; }

        public virtual User user { get; set; }



    }

    public partial class Current_Ñhecks_Repository : IRepository<Current_Ñheck>
    {
        string connectionString = Resource.ConSTR;

        public void Create(Current_Ñheck value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO [dbo].[Current_Ñhecks]" +
           "([Current_Checks_Admin_Id], [Current_Ñhecks_User_Id], [Current_Ñhecks_Date])" +
     "VALUES (@Current_Checks_Admin_Id,  @Current_Ñhecks_User_Id,  @Current_Ñhecks_Date)";

                        db.Execute(sqlQuery,
                       new
                       {
                           Current_Checks_Admin_Id = value.Current_Checks_Admin_Id,
                           Current_Ñhecks_User_Id = value.Current_Ñhecks_User_Id,
                           Current_Ñhecks_Date = value.Current_Ñhecks_Date,
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

        public void Delete(Current_Ñheck value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  [dbo].[Current_Ñhecks] WHERE Current_Ñhecks_Id = @Current_Ñhecks_Id";
                        db.Execute(sqlQuery, new { value.Current_Ñhecks_Id }, transaction);
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

        public Current_Ñheck Get(int Id)
        {
            Current_Ñheck item = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                item = db.Query<Current_Ñheck>("SELECT * FROM [dbo].[Current_Ñhecks] WHERE Current_Ñhecks_Id = @Current_Ñhecks_Id", new { Id }).FirstOrDefault();
            }
            return item;
        }

        public IEnumerable<Current_Ñheck> GetColl()
        {
            List<Current_Ñheck> coll = new List<Current_Ñheck>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                coll = db.Query<Current_Ñheck>("SELECT * FROM [dbo].[Current_Ñhecks]").ToList();
            }
            return coll;
        }

        public void Update(Current_Ñheck value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  [dbo].[Current_Ñhecks] SET Current_Checks_Admin_Id = @Current_Checks_Admin_Id ,Current_Ñhecks_User_Id =  @Current_Ñhecks_User_Id," +
                            " Current_Ñhecks_Date =  @Current_Ñhecks_Date)" +
                            " WHERE Current_Ñhecks_Id = @Current_Ñhecks_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Current_Ñhecks_Id = value.Current_Ñhecks_Id,
                               Current_Checks_Admin_Id = value.Current_Checks_Admin_Id,
                               Current_Ñhecks_User_Id = value.Current_Ñhecks_User_Id,
                               Current_Ñhecks_Date = value.Current_Ñhecks_Date,
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
