using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class AdministratorRepoADO(
        ILogger<AdministratorRepoADO> logger,
        SqlConnectionFactory connectionFactory
    )
    : IRepository<AdministratorEntityADO>
    {
        #region CREATE_ROUTES
        public async Task<int> InsertAsync(AdministratorEntityADO entity)
        {
            throw new NotImplementedException();
        }
        #endregion CREATE_ROUTES

        #region READ_ROUTES
        public async Task<IEnumerable<AdministratorEntityADO>> GetAllAsync()
        {
            try
            {
                //Connection Setup & Execution
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(connectionFactory, "SELECT * FROM Administrators");

                var admins = new List<AdministratorEntityADO>();

                while (await  reader.ReadAsync())
                {
                    var admin = new AdministratorEntityADO()
                    {
                        AdminId = reader.GetInt32(reader.GetOrdinal("AdminID")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName"))
                    };
                    admins.Add(admin);
                }

                return admins;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving Administrator list");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<AdministratorEntityADO?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        #endregion READ_ROUTES

        #region UPDATE_ROUTES
        public async Task<int> UpdateAsync(int id, AdministratorEntityADO entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DELETE_ROUTES
        public async Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
