using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Operations_Console.Models;
using Npgsql;

namespace Operations_Console.DBHelper
{
    public class DbHelper
    {
        private static readonly string _dBConn = ConfigurationManager.AppSettings["WRITE_CON_STR"];
        public static DbHelper Instance = new DbHelper();

        public List<Server> GetServers()
        {
            var con = new NpgsqlConnection(_dBConn);
            List<Server> servers = new List<Server>() { };

            var cmd = new NpgsqlCommand("\"get_servers\"", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                servers.Add(new Server
                {
                    Id = reader.GetFieldValue<int>(0),
                    IpAddress = reader.GetFieldValue<string>(1),
                    ServerName = reader.GetFieldValue<string>(2),
                    ServerType = reader.GetFieldValue<string>(3),
                    Apps = reader.GetFieldValue<List<string>>(4)
                });
            }
            con.Close();
            con.Dispose();

            return servers;

        }

        public ServerStat GetServerStats(int serverid)
        {
            var con = new NpgsqlConnection(_dBConn);
            ServerStat serverstats = new ServerStat() { };

            var cmd = new NpgsqlCommand("\"get_serverstats\"", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new NpgsqlParameter("serverid", NpgsqlTypes.NpgsqlDbType.Integer));
            cmd.Parameters[0].Value = serverid;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                serverstats = (new ServerStat()
                {
                    Time = reader.GetFieldValue<DateTime>(0),
                    ServerId = reader.GetFieldValue<int>(1),
                    IpAddress = reader.GetFieldValue<string>(2),
                    DiskUsage = reader.GetFieldValue<double>(3),
                    DiskUsagePercent = reader.GetFieldValue<double>(4),
                    CpuUsage = reader.GetFieldValue<double>(5),
                    CpuUsagePercent = reader.GetFieldValue<double>(6),
                    NetworkUsage = reader.GetFieldValue<double>(7),
                    NetworkUsagePercent = reader.GetFieldValue<double>(8),
                    MemoryUsage = reader.GetFieldValue<double>(9),
                    MemoryUsagePercent = reader.GetFieldValue<double>(10),
                    DiskData = reader.GetFieldValue<List<string>>(11)

                });
            }
            con.Close();
            con.Dispose();

            return serverstats;
        }


        public List<ServerReport> GetServerReport(int serverid)
        {
            var con = new NpgsqlConnection(_dBConn);
            List<ServerReport> report = new List<ServerReport>() { };

            var cmd = new NpgsqlCommand("\"get_server_report\"", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new NpgsqlParameter("serverid", NpgsqlTypes.NpgsqlDbType.Integer));
            cmd.Parameters[0].Value = serverid;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                report.Add(new ServerReport()
                {
                    Time = reader.GetFieldValue<DateTime>(0),
                    ServerName = reader.GetFieldValue<string>(1),
                    IpAddress = reader.GetFieldValue<string>(2),
                    DiskUsage = reader.GetFieldValue<double>(3),
                    CpuUsage = reader.GetFieldValue<double>(4),
                    NetworkUsage = reader.GetFieldValue<double>(5),
                    MemoryUsage = reader.GetFieldValue<double>(6),
                    DiskData = reader.GetFieldValue<List<string>>(7)
                });
            }
            con.Close();
            con.Dispose();

            return report;
        }

        public List<Apps> GetServerApps()
        {
            var con = new NpgsqlConnection(_dBConn);
            List<Apps> serverApps = new List<Apps>();

            var cmd = new NpgsqlCommand("\"getapps\"", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                serverApps.Add(new Apps()
                {
                    AppId = reader.GetFieldValue<int>(0),
                    Name = reader.GetFieldValue<string>(1),
                });
            }
            con.Close();
            con.Dispose();


            return serverApps;
        }

        public List<AppNodes> GetAppServers(int appid)
        {
            var con = new NpgsqlConnection(_dBConn);
            List<AppNodes> servers = new List<AppNodes>();

            var cmd = new NpgsqlCommand("\"get-app-servers\"", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new NpgsqlParameter("appid", NpgsqlTypes.NpgsqlDbType.Bigint));
            cmd.Parameters[0].Value = appid;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                servers.Add(new AppNodes()
                {
                    AppNodeId = reader.GetFieldValue<int>(0),
                    AppId = reader.GetFieldValue<int>(1),
                    ServerId = reader.GetFieldValue<int>(2),
                    ServerType = reader.GetFieldValue<string>(3)
                });
            }
            con.Close();
            con.Dispose();


            return servers;
        }

        public Server GetServersInfo(int serverid)
        {
            var con = new NpgsqlConnection(_dBConn);
            List<Server> server = new List<Server>() { };

            var cmd = new NpgsqlCommand("\"get_server_info\"", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new NpgsqlParameter("currentserverid", NpgsqlTypes.NpgsqlDbType.Integer));
            cmd.Parameters[0].Value = serverid;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                server.Add(new Server
                {
                    IpAddress = reader.GetFieldValue<string>(0),
                    ServerName = reader.GetFieldValue<string>(1),
                    Id = reader.GetFieldValue<int>(2),
                    ServerType = reader.GetFieldValue<string>(3),
                    Apps = reader.GetFieldValue<List<string>>(4)
                });
            }
            con.Close();
            con.Dispose();

            return server.SingleOrDefault();

        }

        public List<CellApps> GetCellApps()
        {
            var con = new NpgsqlConnection(_dBConn);

            List<CellApps> apps = new List<CellApps>() { };

            var cmd = new NpgsqlCommand("\"get_cell_apps\"", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                apps.Add(new CellApps
                {
                    ShortCode = reader.GetFieldValue<string>(1),
                    ApplicationType = reader.GetFieldValue<string>(2),
                });
            }

            con.Close();
            con.Dispose();

            return apps;

        }

        public CellApps GetCellAppStatus(String shortcode){
            var con = new NpgsqlConnection(_dBConn);
            List<CellApps> apps = new List<CellApps>() { };

            var cmd = new NpgsqlCommand("\"get_cell_app_status\"", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new NpgsqlParameter("shortcode", NpgsqlTypes.NpgsqlDbType.Text));
            cmd.Parameters[0].Value = shortcode;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                apps.Add(new CellApps
                {
                    ShortCode = reader.GetFieldValue<string>(0),
                    ApplicationType = reader.GetFieldValue<string>(1),
                    ExecutionTime = reader.GetFieldValue<double>(2),
                    IsSuccessful = reader.GetFieldValue<bool>(3),
                    IpAddress = reader.GetFieldValue<string>(4),
                    FailureReason = reader.GetFieldValue<string>(5),
                    ServiceName = reader.GetFieldValue<string>(6),
                    CellAppsName = reader.GetFieldValue<string>(7),
                });
            }

            con.Close();
            con.Dispose();

            return apps.SingleOrDefault();
        }

    }

}