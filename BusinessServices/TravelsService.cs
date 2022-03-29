using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SKTravelsApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SKTravelsApp.BusinessServices
{
    public class TravelsService
    {
        private readonly IConfiguration _config;
        public TravelsService(IConfiguration config)
        {
            _config = config;
        }

        public Dictionary<string, object> SaveTravelDays(TravelDaysDto travelDays)
        {
            Dictionary<string, object> Output = new Dictionary<string, object>();
            DataTable dtResult = new DataTable();
            try
            {
                var connectionString = _config.GetConnectionString("SKTravelsCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SaveTravelDays", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserID", travelDays.UserID);
                cmd.Parameters.AddWithValue("@NumberOfDays", travelDays.NumberOfDays);
                cmd.Parameters.AddWithValue("@MainCityID", travelDays.MainCityID);

                SqlParameter ErrorParam = new SqlParameter("@ErrorParam", SqlDbType.VarChar);
                ErrorParam.Size = 100;
                ErrorParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ErrorParam);

                SqlParameter AdditionalParam1 = new SqlParameter("@AdditionalParam1", SqlDbType.VarChar);
                AdditionalParam1.Size = 10;
                AdditionalParam1.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(AdditionalParam1);

                SqlParameter AdditionalParam2 = new SqlParameter("@AdditionalParam2", SqlDbType.VarChar);
                AdditionalParam2.Size = 50;
                AdditionalParam2.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(AdditionalParam2);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dtResult.Load(reader);
                }

                con.Close();

                var paramsList = cmd.Parameters;
                string error = "", additionalparam1 = "", additionalparam2 = "";

                if (paramsList.Contains("@AdditionalParam1"))
                {
                    additionalparam1 = Convert.ToString(cmd.Parameters["@AdditionalParam1"].Value);
                }

                if (paramsList.Contains("@AdditionalParam2"))
                {
                    additionalparam2 = Convert.ToString(cmd.Parameters["@AdditionalParam2"].Value);
                }

                if (paramsList.Contains("@ErrorParam"))
                {
                    error = Convert.ToString(cmd.Parameters["@ErrorParam"].Value);
                }

                Output.Add("Data", dtResult);
                Output.Add("Error", error);
                Output.Add("AdditionalParam1", additionalparam1);
                Output.Add("AdditionalParam2", additionalparam2);

                return Output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, object> GetTravelDays(TravelDaysDto travelDays)
        {
            Dictionary<string, object> Output = new Dictionary<string, object>();
            DataTable dtResult = new DataTable();
            try
            {
                var connectionString = _config.GetConnectionString("SKTravelsCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("GetTravelDays", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserID", travelDays.UserID);
                cmd.Parameters.AddWithValue("@MainCityID", travelDays.MainCityID);

                SqlParameter ErrorParam = new SqlParameter("@ErrorParam", SqlDbType.VarChar);
                ErrorParam.Size = 100;
                ErrorParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ErrorParam);

                SqlParameter AdditionalParam1 = new SqlParameter("@AdditionalParam1", SqlDbType.VarChar);
                AdditionalParam1.Size = 10;
                AdditionalParam1.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(AdditionalParam1);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dtResult.Load(reader);
                }

                con.Close();

                var paramsList = cmd.Parameters;
                string error = "", additionalparam1 = "";

                if (paramsList.Contains("@AdditionalParam1"))
                {
                    additionalparam1 = Convert.ToString(cmd.Parameters["@AdditionalParam1"].Value);
                }

                if (paramsList.Contains("@ErrorParam"))
                {
                    error = Convert.ToString(cmd.Parameters["@ErrorParam"].Value);
                }

                Output.Add("Data", dtResult);
                Output.Add("Error", error);
                Output.Add("AdditionalParam1", additionalparam1);

                return Output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, object> SaveTravelCities(VisitingCitiesDto travelCities)
        {
            Dictionary<string, object> Output = new Dictionary<string, object>();
            DataTable dtResult = new DataTable();
            try
            {
                string VisitingCities = string.Empty;
                if (travelCities.VisitingcityIDs != null)
                {
                    VisitingCities = string.Join(",", travelCities.VisitingcityIDs);
                }

                var connectionString = _config.GetConnectionString("SKTravelsCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SaveTravelCities", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserID", travelCities.UserID);
                cmd.Parameters.AddWithValue("@MainCityID", travelCities.MainCityID);
                cmd.Parameters.AddWithValue("@VisitingCities", VisitingCities);

                SqlParameter ErrorParam = new SqlParameter("@ErrorParam", SqlDbType.VarChar);
                ErrorParam.Size = 100;
                ErrorParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ErrorParam);

                SqlParameter AdditionalParam1 = new SqlParameter("@AdditionalParam1", SqlDbType.VarChar);
                AdditionalParam1.Size = 10;
                AdditionalParam1.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(AdditionalParam1);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dtResult.Load(reader);
                }

                con.Close();

                var paramsList = cmd.Parameters;
                string error = "", additionalparam1 = "";

                if (paramsList.Contains("@AdditionalParam1"))
                {
                    additionalparam1 = Convert.ToString(cmd.Parameters["@AdditionalParam1"].Value);
                }

                if (paramsList.Contains("@ErrorParam"))
                {
                    error = Convert.ToString(cmd.Parameters["@ErrorParam"].Value);
                }

                Output.Add("Data", dtResult);
                Output.Add("Error", error);
                Output.Add("AdditionalParam1", additionalparam1);

                return Output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
