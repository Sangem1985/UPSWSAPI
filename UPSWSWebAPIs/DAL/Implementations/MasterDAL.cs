using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Npgsql;
using System.Data;
using System.Text.Json;
using UPSWSWebAPIs.DAL.Interfaces;
using static UPSWSWebAPIs.Models.DBConstants;
using static UPSWSWebAPIs.Models.MasterModel;

namespace UPSWSWebAPIs.DAL.Implementations
{
    public class MasterDAL:IMasterDAL
    {
        public readonly IConfiguration _Configuration;
        public readonly string _connString;
        public readonly ILogger<MasterDAL> _Logger;

        public MasterDAL(IConfiguration Configuration, ILogger<MasterDAL> Logger)
        {
            _Configuration=Configuration;
            _connString = Configuration.GetConnectionString("NIVESHMITRA");
            _Logger = Logger;
        }
        public async Task<string> GetDistricts()
        {   
            var connection = new NpgsqlConnection(_connString);
            List<District> districts = new List<District>();

            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {   
                    await conn.OpenAsync();
                    var result = await connection.QueryAsync<District>(MasterConstants.GetDistritcs, commandType: CommandType.Text);
                    districts = result?.ToList() ?? new List<District>(); 
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Error while fetching districts");
            }

            return JsonSerializer.Serialize(districts);
        }

    }
}
