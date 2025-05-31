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
            //var conn = new NpgsqlConnection(_connString);
            //List<District> DistrictsList = new List<District>();

            //try
            //{
            //    await conn.OpenAsync();

            //    var res = await conn.QueryAsync<District>(
            //        MasterConstants.GetDistritcs,          
            //        commandType: CommandType.StoredProcedure,
            //        commandTimeout: 1200
            //    );

            //    DistrictsList = res?.ToList() ?? new List<District>();
            //}
            //catch (Exception ex)
            //{
            //    _Logger.LogError(ex, "Error fetching districts");
            //    throw;
            //}
            //finally
            //{
            //    await conn.CloseAsync();
            //}

            //return JsonSerializer.Serialize(DistrictsList);
            List<District> districts = new List<District>();

            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    string query = MasterConstants.GetDistritcs; // For example: "SELECT DistrictCode, DistrictName FROM master_district WHERE status = 'Y'";

                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            districts.Add(new District
                            {
                                DistrictId = reader["districtcode"].ToString(),
                                DistrictName = reader["districtname"].ToString()
                            });
                        }
                    }
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
