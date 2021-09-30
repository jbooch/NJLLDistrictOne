using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace DistrictObjects
{
    public abstract class DistrictObject
    {
        public static ArrayList States
        {
            get
            {
                ArrayList StateList = new ArrayList();
                StateList.Add("AL");
                StateList.Add("AR");
                StateList.Add("AK");
                StateList.Add("AZ");
                StateList.Add("CA");
                StateList.Add("CO");
                StateList.Add("CT");
                StateList.Add("DE");
                StateList.Add("FL");
                StateList.Add("GA");
                StateList.Add("HI");
                StateList.Add("ID");
                StateList.Add("IL");
                StateList.Add("IN");
                StateList.Add("IA");
                StateList.Add("KS");
                StateList.Add("KY");
                StateList.Add("LA");
                StateList.Add("MA");
                StateList.Add("ME");
                StateList.Add("MD");
                StateList.Add("MS");
                StateList.Add("MI");
                StateList.Add("MN");
                StateList.Add("MO");
                StateList.Add("MT");
                StateList.Add("NB");
                StateList.Add("NV");
                StateList.Add("NH");
                StateList.Add("NJ");
                StateList.Add("NM");
                StateList.Add("NY");
                StateList.Add("NC");
                StateList.Add("ND");
                StateList.Add("OH");
                StateList.Add("OK");
                StateList.Add("OR");
                StateList.Add("PA");
                StateList.Add("RI");
                StateList.Add("SC");
                StateList.Add("SD");
                StateList.Add("TN");
                StateList.Add("TX");
                StateList.Add("UT");
                StateList.Add("VT");
                StateList.Add("VA");
                StateList.Add("WA");
                StateList.Add("WV");
                StateList.Add("WI");
                StateList.Add("WY");
                StateList.Add("DC");

                StateList.Sort();
                return StateList;
            }
        }

        private static string _sqlMessages;
        private int _id;
        protected bool _updated;
        private string _lastUser;
        private DateTime _lastTmp;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        public bool updated
        {
            get { return _updated; }
        }
        public string lastUser
        {
            get { return _lastUser; }
            set { _lastUser = value; }
        }
        public DateTime LastTmp
        {
            get { return _lastTmp; }
            set { _lastTmp = value; }
        }

        public static string sqlMessages
        {
            get { return _sqlMessages; }
        }

        internal static void addToSQLMessage(string aMessage)
        {
            _sqlMessages += aMessage + ": ";
        }
        internal static void resetSQLMessage()
        {
            _sqlMessages = "";
        }
        public static string getDistrictData(string aFieldName)
        {
            return DatabaseManager.getDistrictData(aFieldName);
        }
        public static bool saveDistrictData(string aFieldName, string aValue)
        {
            return DatabaseManager.saveDistrictData(aFieldName, aValue);
        }

        internal class DatabaseManager
        {
            private static string _connectionString;

            private static string connectionString
            {
                get
                {
                    if (_connectionString == null)
                    {
                        _connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");
                    }
                    return _connectionString;
                }
            }

            internal static ArrayList getAllTeams()
            {
                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictTeam";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    Team tTeam;
                    while (aReader.Read())
                    {
                        if ((string)aReader["pool"] == "I")
                        {
                            tTeam = new InterleagueTeam();
                        }
                        else
                        {
                            tTeam = new DistrictTeam();
                            if (aReader["pool"] != DBNull.Value)
                                ((DistrictTeam)tTeam).pool = (string)aReader["pool"];
                        }

                        tTeam.id = (int)aReader["id"];
                        tTeam.league = League.find((int)aReader["league"]);
                        tTeam.division = Division.find((int)aReader["division"]);
                        if (aReader["name"] != DBNull.Value)
                            tTeam.name = (string)aReader["name"];
                        if (aReader["manager"] != DBNull.Value)
                            tTeam.manager = Person.find((int)aReader["manager"]);
                        tTeam._updated = false;

                        temp.Add(tTeam);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
            }

            internal static ArrayList getAllGames()
            {
                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictGame";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    Game tGame;
                    while (aReader.Read())
                    {
                        if (((string)aReader["type"]) == "P")
                            tGame = new PoolGame();
                        else if (((string)aReader["type"]) == "I")
                            tGame = new InterleagueGame();
                        else
                            tGame = new BracketGame();

                        tGame.id = (int)aReader["id"];
                        tGame.gameDate = (DateTime)aReader["gameDate"];
                        tGame.team1 = Team.find((int)aReader["team1"]);
                        tGame.team2 = Team.find((int)aReader["team2"]);
                        tGame.location = (string)aReader["location"];
                        if (aReader["updateComment"] != DBNull.Value)
                            tGame.updateComment = (string)aReader["updateComment"];
                        if (aReader["score1"] != DBNull.Value)
                            tGame.score1 = (int) aReader["score1"];
                        if (aReader["score2"] != DBNull.Value)
                            tGame.score2 = (int)aReader["score2"];
                        if (aReader["verifiedBy"] != DBNull.Value)
                        {
                            tGame.personVerified = Person.find((int)aReader["verifiedBy"]);
                            tGame.dateVerified = (DateTime)aReader["verifiedDateTime"];
                        }

                        if (tGame.isPool())
                        {
                            if (aReader["fieldInnings1"] != DBNull.Value)
                                tGame.fieldInnings1 = (int)aReader["fieldInnings1"];
                            if (aReader["fieldInnings2"] != DBNull.Value)
                                tGame.fieldInnings2 = (int)aReader["fieldInnings2"];
                            ((PoolGame)tGame).pool = (string)aReader["category"];
                        }
                        if (tGame.isBracket())
                        {
                            if (aReader["fieldInnings1"] != DBNull.Value)
                                tGame.fieldInnings1 = (int)aReader["fieldInnings1"];
                            ((BracketGame)tGame).gameNumber = Convert.ToInt32((string)aReader["category"]);
                        }
                        if (aReader["updateTmp"] != DBNull.Value)
                            tGame.LastTmp = (DateTime)aReader["updateTmp"];
                        tGame._updated = false;
                        temp.Add(tGame);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
            }

            internal static ArrayList getAllLagues()
            {
                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictLeague";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    League tLeague;
                    while (aReader.Read())
                    {
                        tLeague = new League();
                        tLeague.id = (int)aReader["id"];
                        tLeague.Name = (string)aReader["name"];
                        if (aReader["president"] != DBNull.Value)
                            tLeague.President = Person.find((int)aReader["president"]);
                        if (aReader["webSite"] != DBNull.Value)
                            tLeague.WebSite = (string)aReader["webSite"];
                        if (aReader["leagueId"] != DBNull.Value)
                            tLeague.leagueId = (string)aReader["leagueId"];
                        tLeague._updated = false;
                        if (aReader["charterYear"] != DBNull.Value)
                            tLeague.charterYear = (string)aReader["charterYear"];
                        if (aReader["town"] != DBNull.Value)
                            tLeague.town = (string)aReader["town"];
                        if (aReader["directions"] != DBNull.Value)
                            tLeague.directions = (string)aReader["directions"];
                        temp.Add(tLeague);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
            }

            internal static ArrayList getAllUmpires()
            {

                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictUmpires";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    Umpire tUmp;
                    while (aReader.Read())
                    {
                        tUmp = new Umpire();
                        tUmp.id = (int)aReader["id"];
                        tUmp.firstName = (string)aReader["firstName"];
                        tUmp.lastName = (string)aReader["lastName"];
                        if (aReader["homeLeague"] != DBNull.Value)
                            tUmp.homeLeague = (string)aReader["homeLeague"];
                        if (aReader["umpireSince"] != DBNull.Value)
                            tUmp.umpireSince = (string)aReader["umpireSince"];
                        if (aReader["credits"] != DBNull.Value)
                            tUmp.credits = (string)aReader["credits"];
                        if (aReader["image"] != DBNull.Value)
                            tUmp.image = (string)aReader["image"];
                        temp.Add(tUmp);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
/*
                ArrayList newList = new ArrayList();
                Umpire anUmp = new Umpire();
                anUmp.id = 1;
                anUmp.firstName = "Tom";
                anUmp.lastName = "Reynolds";
                anUmp.homeLeague = "Wharton";
                anUmp.umpireSince = "1776";
                anUmp.credits = "District Umpire In Chief, 2014 Junior Boys State Umpire";
                newList.Add(anUmp);

                anUmp = new Umpire();
                anUmp.id = 2;
                anUmp.firstName = "Dave";
                anUmp.lastName = "Deckhert";
                anUmp.homeLeague = "Par-Troy Little League East";
                anUmp.umpireSince = "600 BC";
                anUmp.credits = "Baseball Seniors World Series Umpire";
                anUmp.image = "http://www.llnjone.org/umpires/umpirephotos/Decks%20number%201.jpg";
                newList.Add(anUmp);

                anUmp = new Umpire();
                anUmp.id = 3;
                anUmp.firstName = "Eric";
                anUmp.lastName = "Hubner";
                anUmp.homeLeague = "Par-Troy Little League East";
                anUmp.umpireSince = "1812";
                anUmp.credits = "State Softball Umpire";
                newList.Add(anUmp);

                return newList;
*/
            }

            internal static ArrayList getAllDivisions()
            {
                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictDivision";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    Division tDivision;
                    while (aReader.Read())
                    {
                        tDivision = new Division();
                        tDivision.id = (int)aReader["id"];
                        tDivision.name = (string)aReader["name"];
                        tDivision.isBaseball = (bool)aReader["baseballDivision"];
//                        try
//                        {
                            tDivision.defaultInnings = (int)aReader["defaultInnings"];
//                        }
//                        catch (Exception)
//                        {
//                            tDivision.defaultInnings = 6;
//                        }
                        if (aReader["PoolAdvanceRules"] != DBNull.Value) 
                            tDivision.poolAdvanceRules = (string)aReader["PoolAdvanceRules"];
                        tDivision._updated = false;
                        temp.Add(tDivision);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
            }

            internal static ArrayList getAllCalendarEntries()
            {
                ArrayList tArray = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictCalendar";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    while (aReader.Read())
                    {
                        CalendarItem temp = new CalendarItem();
                        temp.id = (int)aReader["id"];
                        temp.date = (DateTime)aReader["date"];
                        temp.description = (string)aReader["description"];
                        temp.place = (string)aReader["place"];
                        temp.subject = (string)aReader["subject"];
                        temp._updated = false;
                        tArray.Add(temp);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return tArray;
            }

            internal static ArrayList getAllSpecialGames()
            {
                ArrayList tArray = new ArrayList();
 
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictSpecialGames";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    while (aReader.Read())
                    {
                        SpecialGame temp = new SpecialGame();
                        temp.id = (int)aReader["id"];
                        temp.ageRange = (string)aReader["ageRange"];
                        if (aReader["hostLeague"] != DBNull.Value)
                            temp.hostLeague = League.find((int)aReader["hostLeague"]);
                        temp.registrationForm = new DistrictLink((string)aReader["registrationForm"]);
                        temp.status = (string)aReader["status"];
                        temp.tournament = (string)aReader["name"];
                        temp.tournamentRules = new DistrictLink((string)aReader["tournamentRules"]);
                        temp.type = (string)aReader["type"];
                        temp.webSite = new DistrictLink((string)aReader["webSite"]);
                        temp._updated = false;
                        tArray.Add(temp);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return tArray;
            }

            internal static string getDistrictData(string aFieldName)
            {
                string answer = "";
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select value from DistrictData where (labelFieldName = @aFieldName or textFieldName = @aFieldName)";
                aCmd.CommandType = CommandType.Text;

                aCmd.Parameters.Add(new SqlParameter("@aFieldName",aFieldName));
                try
                {
                    aCmd.Connection.Open();
                    answer = (string)aCmd.ExecuteScalar();
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return answer;
            }

            internal static ArrayList getAllLocations()
            {
                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from Location";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    Location tLoc = null;
                    while (aReader.Read())
                    {
                        tLoc = new Location();
                        tLoc.id = (int)aReader["id"];
                        tLoc.hostLeague = League.find((int)aReader["hostLeague"]);
                        tLoc.name = (string)aReader["name"];
                        tLoc.street = (string)aReader["street"];
                        tLoc.city = (string)aReader["city"];
                        tLoc.state = (string)aReader["state"];
                        tLoc.zip = (string)aReader["zip"];
                        if (aReader["latitude"] != DBNull.Value)
                            tLoc.latitude = (string)aReader["latitude"];
                        if (aReader["longitude"] != DBNull.Value)
                            tLoc.longitude = (string)aReader["longitude"];
                        tLoc._updated = false;
                        temp.Add(tLoc);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
            }

            internal static ArrayList getAllMinutes()
            {
                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictMinutes";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    DistrictMinutes tMinutes;
                    while (aReader.Read())
                    {
                        tMinutes = new DistrictMinutes();
                        tMinutes.id = (int)aReader["id"];
                        tMinutes.year = (string)aReader["mnYear"];
                        tMinutes.month = (string)aReader["mnMonth"];
                        tMinutes.fileName = (string)aReader["mnFileName"];
                        tMinutes._updated = false;
                        temp.Add(tMinutes);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
            }

            internal static bool insert(DistrictMinutes temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "dbo.insertDistrictMinutes";
                aCmd.CommandType = CommandType.StoredProcedure;
                aCmd.Parameters.Add("@mnYear", SqlDbType.VarChar);
                aCmd.Parameters.Add("@mnMonth", SqlDbType.VarChar);
                aCmd.Parameters.Add("@mnFileName", SqlDbType.VarChar);

                aCmd.Parameters["@mnYear"].Value = temp.year;
                aCmd.Parameters["@mnMonth"].Value = temp.month;
                aCmd.Parameters["@mnFileName"].Value = temp.fileName;

                try
                {
                    aCmd.Connection.Open();
                    temp.id = Convert.ToInt32(aCmd.ExecuteScalar());
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    ans = false;
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }

            internal static bool saveDistrictData(string afieldName, string aValue)
            {
                bool ans = false;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "update DistrictData set value = @aValue where (labelFieldName = @aFieldName or textFieldName = @aFieldName)";
                aCmd.CommandType = CommandType.Text;
                aCmd.Parameters.AddWithValue("@aValue",aValue);
                aCmd.Parameters.AddWithValue("@aFieldName", afieldName);

                try
                {
                    aCmd.Connection.Open();
                    int rowsUpdated = aCmd.ExecuteNonQuery();

                    if (rowsUpdated == 1)
                        ans = true;

                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }

            private static void insert(Results temp)
            {
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "dbo.insertDistrictResults";
                aCmd.CommandType = CommandType.StoredProcedure;
                aCmd.Parameters.Add("@gameId", SqlDbType.Int);
                aCmd.Parameters.Add("@score1", SqlDbType.Int);
                aCmd.Parameters.Add("@score2", SqlDbType.Int);
                aCmd.Parameters.Add("@fieldInnings1", SqlDbType.Int);
                aCmd.Parameters.Add("@fieldInnings2", SqlDbType.Int);
                aCmd.Parameters.Add("@recordedBy", SqlDbType.VarChar);
                aCmd.Parameters.Add("@dateReceived", SqlDbType.DateTime);
                aCmd.Parameters.Add("@comments", SqlDbType.Text);
                aCmd.Parameters.Add("@recordedPhone", SqlDbType.VarChar);

                aCmd.Parameters["@gameId"].Value = temp.theGame.id;
                aCmd.Parameters["@score1"].Value = temp.score1;
                aCmd.Parameters["@score2"].Value = temp.score2;
                if (temp.fieldInnings1 != 0)
                    aCmd.Parameters["@fieldInnings1"].Value = temp.fieldInnings1;
                else
                    aCmd.Parameters["@fieldInnings1"].Value = DBNull.Value;
                if (temp.fieldInnings2 != 0)
                    aCmd.Parameters["@fieldInnings2"].Value = temp.fieldInnings2;
                else
                    aCmd.Parameters["@fieldInnings2"].Value = DBNull.Value;

                aCmd.Parameters["@recordedBy"].Value = temp.recordedBy;
                aCmd.Parameters["@dateReceived"].Value = temp.dateReceived;
                aCmd.Parameters["@recordedPhone"].Value = temp.phoneContact;
                if (temp.comments != null)
                    aCmd.Parameters["@comments"].Value = temp.comments;
                else
                    aCmd.Parameters["@comments"].Value = DBNull.Value;

                try
                {
                    aCmd.Connection.Open();
                    temp.id = Convert.ToInt32(aCmd.ExecuteScalar());
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
            }
            private static void update(Results temp)
            {
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "dbo.updateDistrictResults";
                aCmd.CommandType = CommandType.StoredProcedure;
                aCmd.Parameters.Add("@id", SqlDbType.Int);
                aCmd.Parameters.Add("@score1", SqlDbType.Int);
                aCmd.Parameters.Add("@score2", SqlDbType.Int);
                aCmd.Parameters.Add("@fieldInnings1", SqlDbType.Int);
                aCmd.Parameters.Add("@fieldInnings2", SqlDbType.Int);
                aCmd.Parameters.Add("@recordedBy", SqlDbType.VarChar);
                aCmd.Parameters.Add("@dateReceived", SqlDbType.DateTime);
                aCmd.Parameters.Add("@comments", SqlDbType.Text);
                aCmd.Parameters.Add("@recordedPhone", SqlDbType.VarChar);

                aCmd.Parameters["@id"].Value = temp.id;
                aCmd.Parameters["@score1"].Value = temp.score1;
                aCmd.Parameters["@score2"].Value = temp.score2;
                if (temp.fieldInnings1 != 0)
                    aCmd.Parameters["@fieldInnings1"].Value = temp.fieldInnings1;
                else
                    aCmd.Parameters["@fieldInnings1"].Value = DBNull.Value;
                if (temp.fieldInnings2 != 0)
                    aCmd.Parameters["@fieldInnings2"].Value = temp.fieldInnings2;
                else
                    aCmd.Parameters["@fieldInnings2"].Value = DBNull.Value;

                aCmd.Parameters["@recordedBy"].Value = temp.recordedBy;
                aCmd.Parameters["@dateReceived"].Value = temp.dateReceived;
                aCmd.Parameters["@recordedPhone"].Value = temp.phoneContact;
                if (temp.comments != null)
                    aCmd.Parameters["@comments"].Value = temp.comments;
                else
                    aCmd.Parameters["@comments"].Value = DBNull.Value;

                try
                {
                    aCmd.Connection.Open();
                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
            }
            private static void insert(Game temp)
            {
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "dbo.insertDistrictGame";
                aCmd.CommandType = CommandType.StoredProcedure;
                aCmd.Parameters.Add("@gameDate", SqlDbType.DateTime);
                aCmd.Parameters.Add("@team1", SqlDbType.Int);
                aCmd.Parameters.Add("@team2", SqlDbType.Int);
                aCmd.Parameters.Add("@location", SqlDbType.VarChar);
                aCmd.Parameters.Add("@type", SqlDbType.VarChar);
                aCmd.Parameters.Add("@category", SqlDbType.VarChar);
                aCmd.Parameters.Add("@updateComment",SqlDbType.VarChar);

                aCmd.Parameters["@gameDate"].Value = temp.gameDate;
                aCmd.Parameters["@team1"].Value = temp.team1.id;
                aCmd.Parameters["@team2"].Value = temp.team2.id;
                aCmd.Parameters["@location"].Value = temp.location;
                if (temp.updateComment != null)
                    aCmd.Parameters["@updateComment"].Value = temp.updateComment;
                else
                    aCmd.Parameters["@updateComment"].Value = DBNull.Value;
                if (temp.isPool())
                {
                    aCmd.Parameters["@type"].Value = "P";
                    aCmd.Parameters["@category"].Value = ((PoolGame)temp).pool;
                }
                else if (temp.isBracket())
                {
                    aCmd.Parameters["@type"].Value = "B";
                    aCmd.Parameters["@category"].Value = ((BracketGame)temp).gameNumber;
                }
                else
                {
                    aCmd.Parameters["@type"].Value = "I";
                    aCmd.Parameters["@category"].Value = "I";
                }

                try
                {
                    aCmd.Connection.Open();
                    temp.id = Convert.ToInt32(aCmd.ExecuteScalar());
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
            }
            internal static bool insert(Location temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "dbo.insertLocation";
                aCmd.CommandType = CommandType.StoredProcedure;
                aCmd.Parameters.Add("@name", SqlDbType.VarChar);
                aCmd.Parameters.Add("@street", SqlDbType.VarChar);
                aCmd.Parameters.Add("@city", SqlDbType.VarChar);
                aCmd.Parameters.Add("@state", SqlDbType.VarChar);
                aCmd.Parameters.Add("@zip", SqlDbType.VarChar);
                aCmd.Parameters.Add("@hostLeague", SqlDbType.Int);
                aCmd.Parameters.Add("@latitude", SqlDbType.VarChar);
                aCmd.Parameters.Add("@longitude", SqlDbType.VarChar);

                aCmd.Parameters["@name"].Value = temp.name;
                aCmd.Parameters["@street"].Value = temp.street;
                aCmd.Parameters["@city"].Value = temp.street;
                aCmd.Parameters["@state"].Value = temp.state;
                aCmd.Parameters["@zip"].Value = temp.zip;
                aCmd.Parameters["@hostLeague"].Value = temp.hostLeague.id;
                if (temp.latitude != null)
                    aCmd.Parameters["@latitude"].Value = temp.latitude;
                else
                    aCmd.Parameters["@latitude"].Value = DBNull.Value;
                if (temp.longitude != null)
                    aCmd.Parameters["@longitude"].Value = temp.latitude;
                else
                    aCmd.Parameters["@longitude"].Value = DBNull.Value;

                try
                {
                    aCmd.Connection.Open();
                    temp.id = Convert.ToInt32(aCmd.ExecuteScalar());
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    ans = false;
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }


            private static void update(Game temp)
            {
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "dbo.updateDistrictGame";
                aCmd.CommandType = CommandType.StoredProcedure;
                aCmd.Parameters.Add("@id", SqlDbType.Int);
                aCmd.Parameters.Add("@gameDate", SqlDbType.DateTime);
                aCmd.Parameters.Add("@team1", SqlDbType.Int);
                aCmd.Parameters.Add("@team2", SqlDbType.Int);
                aCmd.Parameters.Add("@score1", SqlDbType.Int);
                aCmd.Parameters.Add("@score2", SqlDbType.Int);
                aCmd.Parameters.Add("@fieldInnings1", SqlDbType.Int);
                aCmd.Parameters.Add("@fieldInnings2", SqlDbType.Int);
                aCmd.Parameters.Add("@location", SqlDbType.VarChar);
                aCmd.Parameters.Add("@verifiedBy", SqlDbType.VarChar);
                aCmd.Parameters.Add("@verifiedDate", SqlDbType.DateTime);
                aCmd.Parameters.Add("@type",SqlDbType.VarChar);
                aCmd.Parameters.Add("@category", SqlDbType.VarChar);
                aCmd.Parameters.Add("@updateComment", SqlDbType.VarChar);

                aCmd.Parameters["@id"].Value = temp.id;
                aCmd.Parameters["@gameDate"].Value = temp.gameDate;
                aCmd.Parameters["@team1"].Value = temp.team1.id;
                aCmd.Parameters["@team2"].Value = temp.team2.id;
                aCmd.Parameters["@score1"].Value = temp.score1;
                aCmd.Parameters["@score2"].Value = temp.score2;
                if (temp.fieldInnings1 != 0)
                    aCmd.Parameters["@fieldInnings1"].Value = temp.fieldInnings1;
                else
                    aCmd.Parameters["@fieldInnings1"].Value = DBNull.Value;
                if (temp.fieldInnings2 != 0)
                    aCmd.Parameters["@fieldInnings2"].Value = temp.fieldInnings2;
                else
                    aCmd.Parameters["@fieldInnings2"].Value = DBNull.Value;

                aCmd.Parameters["@location"].Value = temp.location;
                if (temp.personVerified != null)
                {
                    aCmd.Parameters["@verifiedBy"].Value = temp.personVerified.id;
                    aCmd.Parameters["@verifiedDate"].Value = temp.dateVerified;
                }
                else
                {
                    aCmd.Parameters["@verifiedBy"].Value = DBNull.Value;
                    aCmd.Parameters["@verifiedDate"].Value = DBNull.Value;
                }
                if (temp.isPool())
                {
                    aCmd.Parameters["@type"].Value = "P";
                    aCmd.Parameters["@category"].Value = ((PoolGame)temp).pool;
                }
                else if (temp.isBracket())
                {
                    aCmd.Parameters["@type"].Value = "B";
                    aCmd.Parameters["@category"].Value = ((BracketGame)temp).gameNumber;
                }
                else
                {
                    aCmd.Parameters["@type"].Value = "I";
//                    aCmd.Parameters["@category"].Value = ((InterleagueGame)temp).pool;
                }
                if (temp.updateComment != null)
                    aCmd.Parameters["@updateComment"].Value = temp.updateComment;
                else
                    aCmd.Parameters["@updateComment"].Value = DBNull.Value;


                try
                {
                    aCmd.Connection.Open();
                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
            }
            private static void insert(Team temp)
            {
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "dbo.insertDistrictTeam";
                aCmd.CommandType = CommandType.StoredProcedure;
                aCmd.Parameters.Add("@league", SqlDbType.Int);
                aCmd.Parameters.Add("@division", SqlDbType.Int);
                aCmd.Parameters.Add("@name", SqlDbType.VarChar);
                aCmd.Parameters.Add("@manager", SqlDbType.Int);
                aCmd.Parameters.Add("@pool", SqlDbType.Char);

                aCmd.Parameters["@league"].Value = temp.league.id;
                aCmd.Parameters["@division"].Value = temp.division.id;
                if (temp.name != null && temp.name.Trim().Length > 0)
                    aCmd.Parameters["@name"].Value = temp.name;
                else
                    aCmd.Parameters["@name"].Value = DBNull.Value;
                if (temp.manager != null)
                    aCmd.Parameters["@manager"].Value = temp.manager.id;
                else
                    aCmd.Parameters["@manager"].Value = DBNull.Value;

                if (temp.isDistrictTeam())
                    aCmd.Parameters["@pool"].Value = ((DistrictTeam)temp).pool;
                else
                    aCmd.Parameters["@pool"].Value = "I";

                try
                {
                    aCmd.Connection.Open();
                    temp.id = Convert.ToInt32(aCmd.ExecuteScalar());
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
            }
            private static void update(Team temp)
            {
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "dbo.updateDistrictTeam";
                aCmd.CommandType = CommandType.StoredProcedure;
                aCmd.Parameters.Add("@id", SqlDbType.Int);
                aCmd.Parameters.Add("@league", SqlDbType.Int);
                aCmd.Parameters.Add("@division", SqlDbType.Int);
                aCmd.Parameters.Add("@name", SqlDbType.VarChar);
                aCmd.Parameters.Add("@manager", SqlDbType.Int);
                aCmd.Parameters.Add("@pool", SqlDbType.Char);

                aCmd.Parameters["@id"].Value = temp.id;
                aCmd.Parameters["@league"].Value = temp.league.id;
                aCmd.Parameters["@division"].Value = temp.division.id;
                if (temp.isDistrictTeam())
                    aCmd.Parameters["@pool"].Value = ((DistrictTeam)temp).pool;
                else
                    aCmd.Parameters["@pool"].Value = "I";
                if (temp.name != null && temp.name.Trim().Length >0)
                    aCmd.Parameters["@name"].Value = temp.name;
                else
                    aCmd.Parameters["@name"].Value = DBNull.Value;
                if (temp.manager != null)
                    aCmd.Parameters["@manager"].Value = temp.manager;
                else
                    aCmd.Parameters["@manager"].Value = DBNull.Value;


                try
                {
                    aCmd.Connection.Open();
                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
            }
            private static void update(Division temp)
            {
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "Update DistrictDivision set PoolAdvanceRules = '" + temp.poolAdvanceRules + "' where id = " + temp.id;
                aCmd.CommandType = CommandType.Text;


                try
                {
                    aCmd.Connection.Open();
                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
            }

            internal static bool save(Staff temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "update districtStaff set position = @aPosition, personId = @personId where id = @anId";
                aCmd.CommandType = CommandType.Text;
                aCmd.Parameters.AddWithValue("@anId", temp.id);
                aCmd.Parameters.AddWithValue("@aPosition", temp.position);

                try
                {
                    aCmd.Connection.Open();

                    if (temp.person != null)
                    {
                        int tRec = savePerson(temp.person);
                        if (tRec == -1)
                        {
                            ans = false;
                        }
                        else if (temp.person.id == -1)
                        {
                            temp.person.id = tRec;
                        }
                    }
                    if (ans)
                    {
                        if (temp.person != null)
                            aCmd.Parameters.AddWithValue("@personID", temp.person.id);
                        else
                            aCmd.Parameters.AddWithValue("@personID", DBNull.Value);
                        aCmd.ExecuteNonQuery();
                    }

                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    addToSQLMessage(ex.Message);
                    ans = false;
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }

            internal static bool save(Results results)
            {
                bool ans = true;    
                if (results.id  == -1)
                    try
                    {
                        insert(results);
                    }
                    catch (Exception)
                    {
                        // record exception
                        ans = false;
                    }
                else
                    try
                    {
                        update(results);
                    }
                    catch (Exception)
                    {
                        // record exception
                        ans = false;
                    }

                return ans;
            }
            internal static bool save(Game theGame)
            {
                bool ans = true;
                if (theGame.id == -1)
                    try
                    {
                        insert(theGame);
                        Game.reset();
                    }
                    catch (Exception)
                    {
                        // record exception
                        ans = false;
                    }
                else
                    try
                    {
                        update(theGame);
                    }
                    catch (Exception)
                    {
                        // record exception
                        ans = false;
                    }

                return ans;
            }
            internal static bool save(Team aTeam)
            {
                bool ans = true;
                if (aTeam.id == -1)
                    try
                    {
                        insert(aTeam);
                        Team.reset();
                    }
                    catch (Exception)
                    {
                        // record exception
                        ans = false;
                    }
                else if (aTeam.deleted == true)
                {
                    try
                    {
                        delete(aTeam);
                        Team.reset();
                    }
                    catch (Exception)
                    {
                        // record exception
                        ans = false;
                    }
                }
                else 
                {
                    try
                    {
                        update(aTeam);
                    }
                    catch (Exception)
                    {
                        // record exception
                        ans = false;
                    }

                }
                return ans;
            }
            internal static bool save(Division aDivision)
            {
                bool ans = true;
                if (aDivision.id == -1)
                    try
                    {
//                        insert(aDivision);
                        Division.reset();
                    }
                    catch (Exception)
                    {
                        // record exception
                        ans = false;
                    }
                else
                    try
                    {
                        update(aDivision);
                    }
                    catch (Exception)
                    {
                        // record exception
                        ans = false;
                    }

                return ans;
            }
            internal static bool save(WebData temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "update DistrictPageData set text = @text where page = @page and tag = @tag";
                aCmd.CommandType = CommandType.Text;
                aCmd.Parameters.Add("@text", SqlDbType.Text);
                aCmd.Parameters.Add("@page", SqlDbType.VarChar);
                aCmd.Parameters.Add("@tag", SqlDbType.VarChar);

                aCmd.Parameters["@text"].Value = temp.text;
                aCmd.Parameters["@page"].Value = temp.page;
                aCmd.Parameters["@tag"].Value = temp.tag;

                try
                {
                    aCmd.Connection.Open();
                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                }
                catch (Exception)
                {
                    ans = false;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { ans = false; }
                }
                return ans;
            }
            internal static bool save(CalendarItem temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "update DistrictCalendar set date = @date, subject = @subject, description = @description, place = @place where id = @id";
                aCmd.CommandType = CommandType.Text;
                aCmd.Parameters.Add("@date", SqlDbType.DateTime);
                aCmd.Parameters.Add("@subject", SqlDbType.VarChar);
                aCmd.Parameters.Add("@description", SqlDbType.VarChar);
                aCmd.Parameters.Add("@place", SqlDbType.VarChar);
                aCmd.Parameters.Add("@id", SqlDbType.Int);

                aCmd.Parameters["@date"].Value = temp.date;
                aCmd.Parameters["@subject"].Value = temp.subject;
                aCmd.Parameters["@description"].Value = temp.description;
                aCmd.Parameters["@place"].Value = temp.place;
                aCmd.Parameters["@id"].Value = temp.id;

                try
                {
                    aCmd.Connection.Open();
                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                }
                catch (Exception)
                {
                    ans = false;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { ans = false; }
                }
                return ans;
            }
            internal static bool save(SpecialGame temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "update DistrictSpecialGames set name = @name, type = @type, ageRange = @ageRange, hostLeague = @hostLeague, status = @status, tournamentRules = @tournamentRules, registrationForm = @registrationForm, webSite = @webSite where id = @id";
                aCmd.CommandType = CommandType.Text;

                aCmd.Parameters.Add("@id", SqlDbType.Int);
                aCmd.Parameters.Add("@name", SqlDbType.VarChar);
                aCmd.Parameters.Add("@type", SqlDbType.VarChar);
                aCmd.Parameters.Add("@ageRange", SqlDbType.VarChar);
                aCmd.Parameters.Add("@hostLeague", SqlDbType.Int);
                aCmd.Parameters.Add("@status", SqlDbType.VarChar);
                aCmd.Parameters.Add("@tournamentRules", SqlDbType.VarChar);
                aCmd.Parameters.Add("@registrationForm", SqlDbType.VarChar);
                aCmd.Parameters.Add("@webSite", SqlDbType.VarChar);

                aCmd.Parameters["@id"].Value = temp.id;
                aCmd.Parameters["@name"].Value = temp.tournament;
                aCmd.Parameters["@type"].Value = temp.type;
                aCmd.Parameters["@ageRange"].Value = temp.ageRange;
                aCmd.Parameters["@status"].Value = temp.status;
                aCmd.Parameters["@tournamentRules"].Value = temp.tournamentRules.ToString();
                aCmd.Parameters["@registrationForm"].Value = temp.registrationForm.ToString();
                aCmd.Parameters["@webSite"].Value = temp.webSite.ToString();
                aCmd.Parameters["@hostLeague"].Value = temp.hostLeague.id;

                try
                {
                    aCmd.Connection.Open();
                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                }
                catch (Exception)
                {
                    ans = false;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { ans = false; }
                }
                return ans;
            }
            internal static int savePerson(Person aPerson)
            {
                int ans = -1;
                SqlConnection aConnection = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConnection;
                if (aPerson.id > -1)
                {
                    aCmd.CommandText = "updatePerson";
                }
                else
                {
                    aCmd.CommandText = "insertPerson";
                }
                aCmd.CommandType = CommandType.StoredProcedure;

                aCmd.Parameters.AddWithValue("@id", aPerson.id);
                aCmd.Parameters.AddWithValue("@firstName", aPerson.FirstName);
                aCmd.Parameters.AddWithValue("@lastName", aPerson.LastName);
                aCmd.Parameters.AddWithValue("@homePhone", aPerson.HomePhone);
                aCmd.Parameters.AddWithValue("@cellPhone", aPerson.CellPhone);
                aCmd.Parameters.AddWithValue("@emailAddress", aPerson.EmailAddress);

                try
                {
                    aCmd.Connection.Open();
                    object tAns = aCmd.ExecuteScalar();
                    ans = Convert.ToInt32(tAns);
                    if (aPerson.id != -1)
                        ans = aPerson.id;
                    else
                        aPerson.id = ans;
                }
                catch (Exception ex)
                {
                    addToSQLMessage(ex.Message);
                    ans = -1;
                }
                return ans;
            }
            internal static bool save(League temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                if (temp.id == -1)
                    aCmd.CommandText = "insert into DistrictLeague (name, webSite, leagueId, charterYear, town, president, directions) values (@name, @webSite, @leagueId, @charterYear, @town, @presidentId, @directions)";
                else
                    aCmd.CommandText = "update DistrictLeague set name = @name, webSite = @webSite, leagueId = @leagueId, charterYear = @charterYear, town = @town, president= @presidentId, directions=@directions where id = @id";
                aCmd.CommandType = CommandType.Text;

                aCmd.Parameters.AddWithValue("@id",temp.id);
                aCmd.Parameters.AddWithValue("@name",temp.Name);
                aCmd.Parameters.AddWithValue("@webSite", temp.WebSite);
                aCmd.Parameters.AddWithValue("@leagueId", temp.leagueId);
                aCmd.Parameters.AddWithValue("@charterYear",temp.charterYear);
                aCmd.Parameters.AddWithValue("@town", temp.town);
                if (temp.directions != null)
                    aCmd.Parameters.AddWithValue("@directions", temp.directions);
                else
                    aCmd.Parameters.AddWithValue("@directions", DBNull.Value);

                try
                {
                    aCmd.Connection.Open();
                    int personId = -1;
                    if (temp.President != null)
                    {
                        personId = savePerson(temp.President);
                        if (personId == -1)
                        {
                            ans = false;
                        }
                        else
                        {
                            aCmd.Parameters.AddWithValue("@presidentId", personId);
                        }
                    }
                    else
                    {
                        aCmd.Parameters.AddWithValue("@presidentId", DBNull.Value);
                    }
                    if (ans)
                    {
                        aCmd.ExecuteNonQuery();
                        aCmd.Connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    addToSQLMessage(ex.Message);
                    ans = false;
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) {}
                }
                return ans;
            }
            internal static bool save(Umpire temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                if (temp.id == -1)
                    aCmd.CommandText = "insertUmpire";
                else
                    aCmd.CommandText = "updateDistrictUmpire";
                aCmd.CommandType = CommandType.StoredProcedure;

                aCmd.Parameters.AddWithValue("@id", temp.id);
                aCmd.Parameters.AddWithValue("@firstname", temp.firstName);
                aCmd.Parameters.AddWithValue("@lastName", temp.lastName);
                aCmd.Parameters.AddWithValue("@homeLeague", temp.homeLeague);
                aCmd.Parameters.AddWithValue("@umpireSince", temp.umpireSince);
                aCmd.Parameters.AddWithValue("@credits", temp.credits);
                if (temp.image != null)
                    aCmd.Parameters.AddWithValue("@image", temp.image);
                else
                    aCmd.Parameters.AddWithValue("@image", DBNull.Value);

                try
                {
                    aCmd.Connection.Open();

                    int tID = Convert.ToInt32(aCmd.ExecuteScalar());
                    if (temp.id == -1)
                        temp.id = tID;
                    aCmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    addToSQLMessage(ex.Message);
                    ans = false;
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }
            internal static bool save(Location temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "updateLocation";
                aCmd.CommandType = CommandType.StoredProcedure;

                aCmd.Parameters.AddWithValue("@id", temp.id);
                aCmd.Parameters.AddWithValue("@name", temp.name);
                aCmd.Parameters.AddWithValue("@street", temp.street);
                aCmd.Parameters.AddWithValue("@hostLeague", temp.hostLeague.id);
                aCmd.Parameters.AddWithValue("@city", temp.city);
                aCmd.Parameters.AddWithValue("@state", temp.state);
                aCmd.Parameters.AddWithValue("@zip", temp.zip);
                if (temp.latitude != null)
                    aCmd.Parameters.AddWithValue("@latitude", temp.latitude);
                else
                    aCmd.Parameters.AddWithValue("@latitude", DBNull.Value);
                if (temp.longitude != null)
                    aCmd.Parameters.AddWithValue("@longitude", temp.longitude);
                else
                    aCmd.Parameters.AddWithValue("@longitude", DBNull.Value);

                try
                {
                    aCmd.Connection.Open();

                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    addToSQLMessage(ex.Message);
                    ans = false;
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }

            internal static ArrayList getResultsFor(Game game)
            {
                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictGameScore where game = " + game.id.ToString();
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    Results tResult;
                    while (aReader.Read())
                    {
                        tResult = new Results();
                        tResult.id = (int)aReader["id"];
                        tResult.theGame = Game.find((int)aReader["game"]);
                        if (aReader["comments"] != DBNull.Value)
                            tResult.comments = (string)aReader["comments"];
                        tResult.dateReceived = (DateTime)aReader["DateReceived"];
                        if (aReader["fieldInnings1"] != DBNull.Value)
                            tResult.fieldInnings1 = (int)aReader["fieldInnings1"];
                        if (aReader["fieldInnings2"] != DBNull.Value)
                            tResult.fieldInnings2 = (int)aReader["fieldInnings2"];
                        tResult.phoneContact = (string)aReader["recordedPhone"];
                        tResult.recordedBy = (string)aReader["recordedBy"];
                        tResult.score2 = (int)aReader["score2"];
                        tResult.score1 = (int)aReader["score1"];
                        tResult._updated = false;
                        temp.Add(tResult);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
            }

            internal static ArrayList getAllPeople()
            {
                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictPerson";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    Person tPerson;
                    while (aReader.Read())
                    {
                        tPerson = new Person();
                        tPerson.id = (int)aReader["id"];
                        tPerson.FirstName = (string)aReader["firstName"];
                        tPerson.LastName = (string)aReader["lastName"];
                        if (aReader["emailAddress"] != DBNull.Value)
                            tPerson.EmailAddress = (string)aReader["emailAddress"];
                        if (aReader["homePhone"] != DBNull.Value)
                            tPerson.HomePhone = (string)aReader["homePhone"];
                        if (aReader["cellPhone"] != DBNull.Value)
                            tPerson.CellPhone = (string)aReader["cellPhone"];
                        temp.Add(tPerson);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
            }

            internal static ArrayList getAllStaff()
            {
                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictStaff";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    Staff tStaff;
                    while (aReader.Read())
                    {
                        tStaff = new Staff();
                        tStaff.id = (int)aReader["id"];
                        tStaff.position = (string)aReader["position"];
                        if (aReader["personId"] != DBNull.Value)
                            tStaff.person = Person.find((int)aReader["personId"]);
                        temp.Add(tStaff);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
            }

            internal static ArrayList getAllWebData()
            {
                ArrayList temp = new ArrayList();
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "select * from DistrictPageData";
                aCmd.CommandType = CommandType.Text;
                try
                {
                    aCmd.Connection.Open();
                    SqlDataReader aReader = aCmd.ExecuteReader();
                    while (aReader.Read())
                    {
                        WebData aData = new WebData();
                        aData.page = (string)aReader["Page"];
                        aData.tag = (string)aReader["tag"];
                        aData.text = (string)aReader["text"];
                        temp.Add(aData);
                    }
                    aCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return temp;
            }

            internal static bool insert(CalendarItem temp)
            {
                bool ans = false;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "insert into DistrictCalendar (date, subject, place, description) values (@date, @subject, @place, @description)";
                aCmd.CommandType = CommandType.Text;
                aCmd.Parameters.Add("@date", SqlDbType.DateTime);
                aCmd.Parameters.Add("@subject", SqlDbType.VarChar);
                aCmd.Parameters.Add("@place", SqlDbType.VarChar);
                aCmd.Parameters.Add("@description", SqlDbType.VarChar);

                aCmd.Parameters["@date"].Value = temp.date;
                aCmd.Parameters["@subject"].Value = temp.subject;
                aCmd.Parameters["@place"].Value = temp.place;
                aCmd.Parameters["@description"].Value = temp.description;

                try
                {
                    aCmd.Connection.Open();
                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                    ans = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }
            internal static bool insert(SpecialGame temp)
            {
                bool ans = false;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "insert into DistrictSpecialGames (name, type, ageRange, hostLeague, status, tournamentRules, registrationForm, webSite) values (@name, @type, @ageRange, @hostLeague, @status, @tournamentRules, @registrationForm, @webSite)";
                aCmd.CommandType = CommandType.Text;
                aCmd.Parameters.Add("@name", SqlDbType.VarChar);
                aCmd.Parameters.Add("@type", SqlDbType.VarChar);
                aCmd.Parameters.Add("@ageRange", SqlDbType.VarChar);
                aCmd.Parameters.Add("@hostLeague", SqlDbType.Int);
                aCmd.Parameters.Add("@status", SqlDbType.VarChar);
                aCmd.Parameters.Add("@tournamentRules", SqlDbType.VarChar);
                aCmd.Parameters.Add("@registrationForm", SqlDbType.VarChar);
                aCmd.Parameters.Add("@webSite", SqlDbType.VarChar);

                aCmd.Parameters["@name"].Value = temp.tournament;
                aCmd.Parameters["@type"].Value = temp.type;
                aCmd.Parameters["@ageRange"].Value = temp.ageRange;
                aCmd.Parameters["@status"].Value = temp.status;
                aCmd.Parameters["@tournamentRules"].Value = temp.tournamentRules.ToString();
                aCmd.Parameters["@registrationForm"].Value = temp.registrationForm.ToString();
                aCmd.Parameters["@webSite"].Value = temp.webSite.ToString();
                aCmd.Parameters["@hostLeague"].Value = temp.hostLeague.id;

                try
                {
                    aCmd.Connection.Open();
                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                    ans = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }

            internal static bool delete(CalendarItem temp)
            {
                bool ans = false;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand aCmd = new SqlCommand();
                aCmd.Connection = aConn;
                aCmd.CommandText = "delete from DistrictCalendar where id = @anId";
                aCmd.CommandType = CommandType.Text;
                aCmd.Parameters.Add("@anId", SqlDbType.Int);

                aCmd.Parameters["@anId"].Value = temp.id;

                try
                {
                    aCmd.Connection.Open();
                    aCmd.ExecuteNonQuery();
                    aCmd.Connection.Close();
                    ans = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        aCmd.Connection.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }
            internal static bool delete(League temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand deleteLeagueCmd = new SqlCommand();
                SqlCommand deletePersonCmd = new SqlCommand();
                SqlTransaction aTran = null;

                deleteLeagueCmd.Connection = aConn;
                deleteLeagueCmd.CommandText = "delete from DistrictLeague where id = @anId";
                deleteLeagueCmd.CommandType = CommandType.Text;

                deleteLeagueCmd.Parameters.AddWithValue("@anId", temp.id);

                deletePersonCmd.Connection = aConn;
                deletePersonCmd.CommandText = "delete from DistrictPerson where id = @anId";
                deletePersonCmd.CommandType = CommandType.Text;

                if (temp.President != null)
                    deletePersonCmd.Parameters.AddWithValue("@anId", temp.President.id);

                try
                {
                    deleteLeagueCmd.Connection.Open();
                    aTran = deleteLeagueCmd.Connection.BeginTransaction();
                    deleteLeagueCmd.Transaction = aTran;

                    int rowsDeleted = deleteLeagueCmd.ExecuteNonQuery();
                    if (rowsDeleted != 1)
                    {
                        aTran.Rollback();
                        ans = false;
                    }
                    else if (temp.President != null)
                    {
                        deletePersonCmd.Transaction = aTran;
                        rowsDeleted = deletePersonCmd.ExecuteNonQuery();

                        if (rowsDeleted != 1)
                        {
                            aTran.Rollback();
                            ans = false;
                        }
                    }
                    if (ans != false)
                        aTran.Commit();

                    aConn.Close();
                }
                catch (Exception)
                {
                    aTran.Rollback();
                    ans = false;
                }
                finally
                {
                    try
                    {
                        aConn.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }
            internal static bool delete(Game temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand deleteGameCmd = new SqlCommand();
                SqlCommand deleteGameScoreCmd = new SqlCommand();
                SqlTransaction aTran = null;

                deleteGameCmd.Connection = aConn;
                deleteGameCmd.CommandText = "delete from DistrictGame where id = @anId";
                deleteGameCmd.CommandType = CommandType.Text;

                deleteGameCmd.Parameters.AddWithValue("@anId", temp.id);

                deleteGameScoreCmd.Connection = aConn;
                deleteGameScoreCmd.CommandText = "delete from DistrictGameScore where game = @anId";
                deleteGameScoreCmd.Parameters.AddWithValue("@anId", temp.id);
                deleteGameScoreCmd.CommandType = CommandType.Text;

                try
                {
                    deleteGameCmd.Connection.Open();
                    aTran = deleteGameCmd.Connection.BeginTransaction();
                    deleteGameCmd.Transaction = aTran;

                    int rowsDeleted = deleteGameCmd.ExecuteNonQuery();
                    if (rowsDeleted != 1)
                    {
                        aTran.Rollback();
                        ans = false;
                    }
                    else
                    {
                        deleteGameScoreCmd.Transaction = aTran;
                        rowsDeleted = deleteGameScoreCmd.ExecuteNonQuery();
                    }
                    if (ans != false)
                        aTran.Commit();

                    aConn.Close();
                }
                catch (Exception)
                {
                    aTran.Rollback();
                    ans = false;
                }
                finally
                {
                    try
                    {
                        aConn.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }
            internal static bool delete(Person temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand deletePersonCmd = new SqlCommand();
                SqlTransaction aTran = null;

                deletePersonCmd.Connection = aConn;
                deletePersonCmd.CommandText = "delete from DistrictPerson where id = @anId";
                deletePersonCmd.CommandType = CommandType.Text;

                deletePersonCmd.Parameters.AddWithValue("@anId", temp.id);

                try
                {
                    aConn.Open();
                    aTran = aConn.BeginTransaction();
                    deletePersonCmd.Transaction = aTran;
                    int rowsDeleted = deletePersonCmd.ExecuteNonQuery();
                    if (rowsDeleted != 1)
                    {
                        aTran.Rollback();
                        ans = false;
                    }
                    else
                    {
                        aTran.Commit();
                    }
                    aConn.Close();
                }
                catch (Exception)
                {
                    aTran.Rollback();
                    ans = false;
                }
                finally
                {
                    try
                    {
                        aConn.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }
            internal static bool delete(Team temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand deleteTeamCmd = new SqlCommand();
                SqlTransaction aTran = null;

                deleteTeamCmd.Connection = aConn;
                deleteTeamCmd.CommandText = "delete from DistrictTeam where id = @anId";
                deleteTeamCmd.CommandType = CommandType.Text;

                deleteTeamCmd.Parameters.AddWithValue("@anId", temp.id);

                try
                {
                    deleteTeamCmd.Connection.Open();
                    aTran = deleteTeamCmd.Connection.BeginTransaction();
                    deleteTeamCmd.Transaction = aTran;

                    int rowsDeleted = deleteTeamCmd.ExecuteNonQuery();
                    if (rowsDeleted != 1)
                    {
                        aTran.Rollback();
                        ans = false;
                    }

                    if (ans != false)
                        aTran.Commit();

                    aConn.Close();
                }
                catch (Exception)
                {
                    aTran.Rollback();
                    ans = false;
                }
                finally
                {
                    try
                    {
                        aConn.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }
            internal static bool delete(DistrictMinutes temp)
            {
                bool ans = true;
                SqlConnection aConn = new SqlConnection(connectionString);
                SqlCommand deleteTeamCmd = new SqlCommand();
                SqlTransaction aTran = null;

                deleteTeamCmd.Connection = aConn;
                deleteTeamCmd.CommandText = "delete from DistrictMinutes where id = @anId";
                deleteTeamCmd.CommandType = CommandType.Text;

                deleteTeamCmd.Parameters.AddWithValue("@anId", temp.id);

                try
                {
                    deleteTeamCmd.Connection.Open();
                    aTran = deleteTeamCmd.Connection.BeginTransaction();
                    deleteTeamCmd.Transaction = aTran;

                    int rowsDeleted = deleteTeamCmd.ExecuteNonQuery();
                    if (rowsDeleted != 1)
                    {
                        aTran.Rollback();
                        ans = false;
                    }

                    if (ans != false)
                        aTran.Commit();

                    aConn.Close();
                }
                catch (Exception)
                {
                    aTran.Rollback();
                    ans = false;
                }
                finally
                {
                    try
                    {
                        aConn.Close();
                    }
                    catch (Exception) { }
                }
                return ans;
            }
        }
    }
    public class DistrictLink
    {
        private static char _splitter = '|';
        private string pLink;
        private string pName;

        public string link
        {
            get { return pLink; }
            set { pLink = value; }
        }
        public string name
        {
            get { return pName; }
            set { pName = value; }
        }
        public DistrictLink()
        {
        }
        public DistrictLink(string newLink, string newName)
        {
            link = newLink;
            name = newName;
        }
        internal DistrictLink(string aValue)
        {
            string[] temp = aValue.Split(_splitter);
            link = temp[0];
            name = temp[1];
        }
        public override string ToString()
        {
            return link + _splitter + name;
        }
    }
    public class Person : DistrictObject
    {
        private static ArrayList _allAdmins;
        private string _firstName;
        private string _lastName;
        private string _cellPhone;
        private string _homePhone;
        private string _emailAddress;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == null || !_firstName.Equals(value))
                {
                    _firstName = value;
                    _updated = true;
                }
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == null || !_lastName.Equals(value))
                {
                    _lastName = value;
                    _updated = true;
                }
            }
        }
        public string CellPhone
        {
            get { return _cellPhone; }
            set
            {
                if (_cellPhone == null || !_cellPhone.Equals(value))
                {
                    _cellPhone = value;
                    _updated = true;
                }
            }
        }
        public string HomePhone
        {
            get { return _homePhone; }
            set
            {
                if (_homePhone == null || !_homePhone.Equals(value))
                {
                    _homePhone = value;
                    _updated = true;
                }
            }
        }
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (_emailAddress == null || !_emailAddress.Equals(value))
                {
                    _emailAddress = value;
                    _updated = true;
                }
            }
        }
        public string fullName
        {
            get { return FirstName + " " + LastName; }
        }
        public static ArrayList allAdmins
        {
            get {
                if (_allAdmins == null)
                    _allAdmins = DatabaseManager.getAllPeople();
                return _allAdmins;
            }
        }

        public Person()
        {
            id = -1;
        }

        public static Person find(int p)
        {
            Person t = null;
            foreach (Person aPErson in Person.allAdmins)
            {
                if (aPErson.id == p)
                {
                    t = aPErson;
                    break;
                }
            }
            return t;
        }
        public static Person find(string anEmail)
        {
            Person t = null;
            foreach (Person aPErson in Person.allAdmins)
            {
                if (aPErson.EmailAddress.ToUpper() == anEmail.ToUpper())
                {
                    t = aPErson;
                    break;
                }
            }
            return t;
        }
        public static void reset()
        {
            _allAdmins = null;
        }
        public override string ToString()
        {
            return FirstName.Trim() + " " + LastName.Trim();
        }
        public bool remove()
        {
            if (DatabaseManager.delete(this))
            {
                _allAdmins = null;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class Staff : DistrictObject
    {
        private static ArrayList _allStaff;
        private string _position;
        private Person _person;

        public string position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Person person
        {
            get { return _person; }
            set { _person = value; }
        }
        public string personURL
        {
            get
            {
                if (person != null)
                {
                    if (person.EmailAddress != null && person.EmailAddress.Trim().Length > 0)
                    {
                        return "<a href='mailto: " + person.EmailAddress + "'>" + person.fullName + "</a>";
//                        return "<a href='LeagueEmail.aspx?staffId=" + id.ToString() + "' target='_blank'>" + person.fullName + "</a>";
                    }
                    else
                    {
                        return person.fullName;
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public static ArrayList allStaff
        {
            get {
                if (_allStaff == null)
                    _allStaff = DatabaseManager.getAllStaff();
                return _allStaff;
            }
        }

        public Staff()
        {
            id = -1;
        }

        public static void reset() {
            _allStaff = null;
        }

        public static Staff findByEmail(string anEmail)
        {
            Staff temp = null;
            foreach (Staff aStaff in allStaff)
            {
                if (aStaff.person != null && aStaff.person.EmailAddress.Equals(anEmail))
                {
                    temp = aStaff;
                    break;
                }
            }
            return temp;
        }

        public static Staff find(int p)
        {
            Staff temp = null;
            foreach (Staff aStaff in allStaff)
            {
                if (aStaff.id == p)
                {
                    temp = aStaff;
                    break;
                }
            }
            return temp;
        }

        public bool save()
        {
            resetSQLMessage();
            if (DatabaseManager.save(this))
            {
                _allStaff = null;
                Person.reset();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class Division:DistrictObject {

        private static ArrayList _allDivisions;
        private string _name;
        private bool _baseball;
        private string _poolAdvanceRules;
        private int _defaultInnings;

        public string name
        {
            get { return _name; }
            set
            {
                if (_name == null || !_name.Equals(value))
                {
                    _name = value;
                    _updated = true;
                }
            }
        }
        public string displayName
        {
            get
            {
                if (isBaseball)
                    return "Baseball - " + name;
                else
                    return "Softball - " + name;
            }
        }
        public bool isBaseball
        {
            get { return _baseball; }
            set
            {
                if (!_baseball.Equals(value))
                {
                    _baseball = value;
                    _updated = true;
                }
            }
        }
        public string poolAdvanceRules
        {
            get { return _poolAdvanceRules; }
            set
            {
                if (_poolAdvanceRules == null || !_poolAdvanceRules.Equals(value))
                {
                    _poolAdvanceRules = value;
                    _updated = true;
                }
            }
        }
        public int defaultInnings
        {
            get { return _defaultInnings; }
            set
            {
                if (_defaultInnings != value)
                {
                    _defaultInnings = value;
                    _updated = true;
                }
            }
        }
        public static ArrayList allDivisions
        {
            get
            {
                if (_allDivisions == null)
                    _allDivisions = DatabaseManager.getAllDivisions();
                return _allDivisions;
            }
        }

        public static Division find(int p)
        {
            Division temp = null;
            foreach (Division aDivision in allDivisions)
            {
                if (aDivision.id == p)
                {
                    temp = aDivision;
                    break;
                }
            }
            return temp;
        }

        public static void reset()
        {
            _allDivisions = null;
        }

        public bool save()
        {
            return DatabaseManager.save(this);
        }

        public Division()
        {
            id = -1;
        }
    }
    public class League : DistrictObject, IComparable
    {
        
        private static ArrayList _allLeagues;
        private string _name;
        private Person _president;
        private string _webSite;
        private string _charterYear;
        private string _leagueID;
        private string _town;
        private string _directions;
        private Person _treasurer;
        private Person _safetyOfficer;

        public static ArrayList allLeagues
        {
            get
            {
                if (_allLeagues == null)
                {
                    _allLeagues = DatabaseManager.getAllLagues();
                    _allLeagues.Sort();
                }
                return _allLeagues;
            }
        }

        public string Name {
            get { return _name;}
            set { 
                if (_name == null || !_name.Equals(value)) {
                    _name = value;
                    _updated = true;
                }
            }
        }
        public Person President
        {
            get { return _president; }
            set
            {
                if (_president == null || !_president.Equals(value))
                {
                    _president = value;
                    _updated = true;
                }
            }
        }
        public string WebSite
        {
            get { return _webSite; }
            set
            {
                if (_webSite == null || !_webSite.Equals(value))
                {
                    _webSite = value;
                    _updated = true;
                }
            }
        }
        public string leagueId
        {
            get { return _leagueID; }
            set
            {
                if (_leagueID == null || !_leagueID.Equals(value))
                {
                    _leagueID = value;
                    _updated = true;
                }
            }
        }
        public string charterYear
        {
            get { return _charterYear; }
            set { _charterYear = value; }
        }
        public string town
        {
            get { return _town; }
            set { _town = value; }
        }
        public string directions
        {
            get { return _directions; }
            set { _directions = value; }
        }
        public Person treasurer
        {
            get { return _treasurer; }
            set { _treasurer = value; }
        }
        public Person safetyOfficer
        {
            get { return _safetyOfficer; }
            set { _safetyOfficer = value; }
        }

        public string websiteURL
        {
        get {
            if (WebSite != null && WebSite.Trim().Length >0)
               return  "<a href='http://" + WebSite + "' target=_blank>" + Name + "</a>";
            else
               return Name;
            }
        }
 
        public string presidentURL
        {
            get
            {
                if (President != null)
                {
                    if (President.EmailAddress != null && President.EmailAddress.Trim().Length > 0)
                    {
                        return "<a href='mailto: " + President.EmailAddress + "'>" + President.fullName + "</a>";
//                        return "<a href='LeagueEmail.aspx?leagueId=" + id.ToString() + "' target='_blank'>" + President.fullName + "</a>";
                    }
                    else
                    {
                        return President.fullName;
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public static League find(int p)
        {
            League temp = null;
            foreach (League aLeague in allLeagues)
            {
                if (aLeague.id == p)
                {
                    temp = aLeague;
                    break;
                }
            }
            return temp;
        }

        public League()
        {
            id = -1;
        }

        public static void reset()
        {
            _allLeagues = null;
            Person.reset();
        }


        #region IComparable Members

        public int CompareTo(object obj)
        {
            return Name.CompareTo(((League)obj).Name);
        }

        #endregion

        public string validate()
        {
            return null;
        }

        public bool save()
        {
            resetSQLMessage();
            if (DatabaseManager.save(this))
            {
                reset();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool delete()
        {
            if (DatabaseManager.delete(this))
            {
                reset();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public abstract class Team : DistrictObject, IComparable
    {
        private static ArrayList _allTeams;
        private League _league;
        private string _name;
        private Division _division;
        private Person _manager;
        private bool _deleted;

        public League league
        {
            get { return _league; }
            set
            {
                if (_league == null || !_league.Equals(value))
                {
                    _league = value;
                    _updated = true;
                }
            }
        }
        public string name
        {
            get { return _name; }
            set
            {
                if (_name == null || !_name.Equals(value))
                {
                    _name = value;
                    _updated = true;
                }
            }
        }
        public Division division
        {
            get { return _division; }
            set
            {
                if (_division == null || !_division.Equals(value))
                {
                    _division = value;
                    _updated = true;
                }
            }
        }
        public Person manager
        {
            get { return _manager; }
            set
            {
                if (_manager == null || !_manager.Equals(value))
                {
                    _manager = value;
                    _updated = true;
                }
            }
        }
        public bool deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }
        public string displayName
        {
            get { return ToString(); }
        }
        public static ArrayList AllTeams
        {
            get
            {
                if (_allTeams == null)
                {
                    _allTeams = DatabaseManager.getAllTeams();
                    _allTeams.Sort();
                }
                return _allTeams;
            }
        }
        public static Team find(int anId)
        {
            Team temp = null;
            foreach (Team aTeam in AllTeams)
            {
                if (aTeam.id == anId)
                {
                    temp = aTeam;
                    break;
                }
            }
            return temp;
        }
        public static Team find(League aLeague, Division aDivision)
        {
            Team ans = null;
            foreach (Team ateam in Team.AllTeams)
            {
                if (ateam.league.id == aLeague.id && ateam.division.id == aDivision.id)
                {
                    ans = ateam;
                    break;
                }
            }
            return ans;
        }
        public int wins
        {
            get
            {
                int x = 0;
                foreach (Game aGame in PoolGame.find(this))
                {
                    if (aGame.personVerified != null)
                    {
                        if ((aGame.team1.id.Equals(this.id) && aGame.score1 > aGame.score2
                            || (aGame.team2.id.Equals(this.id) && aGame.score2 > aGame.score1)))
                            x++;
                    }
                }
                return x;
            }
        }
        public int losses
        {
            get
            {
                int x = 0;
                foreach (Game aGame in PoolGame.find(this))
                {
                    if (aGame.personVerified != null)
                    {
                        if ((aGame.team1.id.Equals(this.id) && aGame.score1 < aGame.score2
                            || (aGame.team2.id.Equals(this.id) && aGame.score2 < aGame.score1)))
                            x++;
                    }
                }
                return x;
            }
        }
        public int fieldInnings
        {
            get
            {
                int totalInnings = 0;
                foreach (Game aGame in PoolGame.find(this))
                {
                    if (aGame.personVerified != null)
                    {
                        if (aGame.team1.id.Equals(this.id))
                            totalInnings += aGame.fieldInnings1;
                        if (aGame.team2.id.Equals(this.id))
                            totalInnings += aGame.fieldInnings2;
                    }
                }
                return totalInnings;
            }
        }
        public int TotalOpponentScore
        {
            get
            {
                int totalRuns = 0;
                foreach (Game aGame in PoolGame.find(this))
                {
                    if (aGame.personVerified != null)
                    {
                        if (aGame.team1.id.Equals(this.id))
                            totalRuns += aGame.score2;
                        if (aGame.team2.id.Equals(this.id))
                            totalRuns += aGame.score1;
                    }
                }
                return totalRuns;
            }
        }
        public decimal runsPerInning
        {
            get
            {
                decimal rpi = 0;
                if (TotalOpponentScore > 0)
                    rpi = Math.Round(Convert.ToDecimal(TotalOpponentScore) / Convert.ToDecimal(fieldInnings), 4);
                return rpi;
            }
        }

        public static void reset()
        {
            _allTeams = null;
        }
        public bool save()
        {
            return DatabaseManager.save(this);
        }

        public static ArrayList getTeamsFor(Division aDivision)
        {
            ArrayList teamList = new ArrayList();
            foreach (Team aTeam in Team.AllTeams)
            {
                if (aTeam.division.id == aDivision.id)
                {
                    teamList.Add(aTeam);
                }
            }
            return teamList;
        }
        public static ArrayList getTeamsFor(Division aDivision, string aPool)
        {
            ArrayList teamList = new ArrayList();
            foreach (Team aTeam in Team.AllTeams)
            {
                if (aTeam.isDistrictTeam() && aTeam.division.id == aDivision.id && ((DistrictTeam)aTeam).pool.Equals(aPool))
                {
                    teamList.Add(aTeam);
                }
            }
            return teamList;
        }
        public override string ToString()
        {
            if (name == null || name.Trim().Length == 0)
                return league.Name;
            else return name;
        }

        public abstract bool isDistrictTeam();
        public abstract bool isInterleagueTeam();

        #region IComparable Members

        public int CompareTo(object obj)
        {
            int ans = 0;
            if (name != null)
            {
                if (((Team)obj).name != null)
                    ans = name.CompareTo(((Team)obj).name);
                else
                    ans = name.CompareTo(((Team)obj).league.Name);
            }
            else
            {
                if (((Team)obj).name != null)
                    ans = league.Name.CompareTo(((Team)obj).name);
                else
                    ans = league.Name.CompareTo(((Team)obj).league.Name);
            }
            return ans;
        }
        #endregion

    }

    public class DistrictTeam : Team
    {
        private string _pool;

        public string pool
        {
            get { return _pool; }
            set
            {
                if (_pool == null || !_pool.Equals(value))
                {
                    _pool = value;
                    _updated = true;
                }
            }
        }
        public DistrictTeam()
        {
            id = -1;
        }

        public static ArrayList getPoolsFor(Division division)
        {
            ArrayList temp = new ArrayList();

            foreach (Team aTeam in Team.AllTeams)
            {
                if (aTeam.isDistrictTeam())
                {
                    DistrictTeam theTeam = (DistrictTeam)aTeam;
                    if (!temp.Contains(theTeam.pool))
                    {
                        temp.Add(theTeam.pool);
                    }
                }
            }
            return temp;
        }

        public override bool isDistrictTeam()
        {
            return true;
        }

        public override bool isInterleagueTeam()
        {
            return false;
        }

        public static ArrayList getTeamsFor(Division aDivision)
        {
            ArrayList teamList = new ArrayList();
            foreach (Team ateam in Team.AllTeams)
            {
                if (ateam.isDistrictTeam() && ateam.division.Equals(aDivision))
                    teamList.Add(ateam);
            }
            return teamList;
        }
    }

    public class InterleagueTeam : Team
    {
        public InterleagueTeam()
        {
            id = -1;
        }

        public override bool isDistrictTeam()
        {
            return false;
        }

        public override bool isInterleagueTeam()
        {
            return true;
        }

        public static ArrayList allTeams
        {
            get
            {
                ArrayList interleagueTeams = new ArrayList();
               foreach(Team ateam in Team.AllTeams)
                {
                    if (ateam.isInterleagueTeam())
                        interleagueTeams.Add(ateam);
                }
                return interleagueTeams;
            }
        }

        public static ArrayList getTeamsFor(Division aDivision)
        {
            ArrayList teamList = new ArrayList();
            foreach(InterleagueTeam ateam in allTeams)
            {
                if (ateam.division.Equals(aDivision))
                    teamList.Add(ateam);
            }
            return teamList;
        }
    }

    public abstract class Game : DistrictObject, IComparable
    {
        private static ArrayList _allGames;
        private Team _team1;
        private Team _team2;
        private DateTime _gameDate;
        private int _score1;
        private int _score2;
        private int _fieldInnings1;
        private int _fieldInnings2;
        private string _location;
        private Person _verified;
        private DateTime _dateVerified;
//        private ArrayList _results;
        private string _updateComment;
        private Location _aLoc;

        public Team team1
        {
            get { return _team1; }
            set
            {
                if (_team1 == null || !_team1.Equals(value))
                {
                    _team1 = value;
                    _updated = true;
                }
            }
        }
        public Team team2
        {
            get { return _team2; }
            set
            {
                if (_team2 == null || !_team2.Equals(value))
                {
                    _team2 = value;
                    _updated = true;
                }
            }
        }
        public DateTime gameDate
        {
            get { return _gameDate; }
            set
            {
                if (_gameDate == null || !_gameDate.Equals(value))
                {
                    _gameDate = value;
                    _updated = true;
                }
            }
        }
        public string gameDateDisplay
        {
            get
            {
                return gameDate.ToString("MM/dd/yyyy hh:mm tt");
            }
        }
        public int score1
        {
            get { return _score1; }
            set
            {
                if (!_score1.Equals(value))
                {
                    _score1 = value;
                    _updated = true;
                }
            }
        }
        public int score2
        {
            get { return _score2; }
            set
            {
                if (!_score2.Equals(value))
                {
                    _score2 = value;
                    _updated = true;
                }
            }
        }
        public int fieldInnings1
        {
            get { return _fieldInnings1; }
            set
            {
                if (!_fieldInnings1.Equals(value))
                {
                    _fieldInnings1 = value;
                    _updated = true;
                }
            }
        }
        public int fieldInnings2
        {
            get { return _fieldInnings2; }
            set
            {
                if (!_fieldInnings2.Equals(value))
                {
                    _fieldInnings2 = value;
                    _updated = true;
                }
            }
        }
        public string location
        {
            get { return _location; }
            set
            {
                if (_location == null || !_location.Equals(value))
                {
                    _location = value;
                    _updated = true;
                }
            }
        }
        public string updateComment
        {
            get { return _updateComment; }
            set
            {
                if (_updateComment == null || !_updateComment.Equals(value))
                {
                    _updateComment = value;
                    _updated = true;
                }
            }
        }
        public Location gameLocation
        {
            get { return _aLoc; }
            set { _aLoc = value; }
        }
        public abstract bool isPool();
        public abstract bool isBracket();
        public abstract bool isInterleague();
        public ArrayList results {
            get { return DatabaseManager.getResultsFor(this);}
        }
        public override string ToString()
        {
            return this.gameDate.ToString() + " " + this.team1.displayName + " vs " + this.team2.displayName + " @ " + this.location;
        }

        public static Game find(int anID)
        {
            Game foundGame = null;
            foreach (Game aGame in AllGames)
            {
                if (aGame.id == anID)
                {
                    foundGame = aGame;
                    break;
                }
            }
            return foundGame;
        }
        public static ArrayList find(League aLeague)
        {
            ArrayList foundGames = new ArrayList();
            foreach (Game aGame in AllGames)
            {
                if (aGame.team1.league.id.Equals(aLeague.id)
                    || aGame.team2.league.id.Equals(aLeague.id))
                {
                    foundGames.Add(aGame);
                }
            }
            return foundGames;
        }
        public static ArrayList find(Team aTeam)
        {
            return find(aTeam, null);
        }
        protected static ArrayList find(Team aTeam, string aType)
        {
            ArrayList foundGames = new ArrayList();
            foreach (Game aGame in AllGames)
            {
                if ((aType ==null ||(aGame.isPool() && aType == "P")|| (aGame.isBracket() && aType == "B"))
                    && (aGame.team1.id.Equals(aTeam.id)|| aGame.team2.id.Equals(aTeam.id)))
                {
                    foundGames.Add(aGame);
                }
            }
            return foundGames;
        }
        public static void reset()
        {
            _allGames = null;
        }
        public Person personVerified
        {
            get { return _verified; }
            set
            {
                if (_verified == null || !_verified.Equals(value))
                {
                    _verified = value;
                    _updated = true;
                }
            }
        }
        public DateTime dateVerified
        {
            get { return _dateVerified; }
            set
            {
                if (_dateVerified == null || !_dateVerified.Equals(value))
                {
                    _dateVerified = value;
                    _updated = true;
                }
            }
        }
        public static ArrayList AllGames
        {
            get
            {
                if (_allGames == null) 
                {
                    _allGames = DatabaseManager.getAllGames();
                    _allGames.Sort();
                }
                return _allGames;
            }
        }
        public static ArrayList AllPoolGames
        {
            get
            {
                ArrayList temp = new ArrayList();
                foreach (Game aGame in AllGames)
                {
                    if (aGame.isPool())
                        temp.Add(aGame);
                }
                return temp;
            }
        }
        public static ArrayList AllBracketGames
        {
            get
            {
                ArrayList temp = new ArrayList();
                foreach (Game aGame in AllGames)
                {
                    if (aGame.isBracket())
                        temp.Add(aGame);
                }
                return temp;
            }
        }

        public bool IsGameDateEqual(DateTime aDate)
        {
            if (gameDate.Year.Equals(aDate.Year)
                && gameDate.Month.Equals(aDate.Month)
                && gameDate.Day.Equals(aDate.Day)
                    )
                return true;
            else
                return false;
        }

        public bool save()
        {
            return DatabaseManager.save(this);
        }

        public bool delete()
        {
            return DatabaseManager.delete(this);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            int ans = 0;
            Game otherGame = (Game)obj;
            if (isPool() && otherGame.isPool()
                || isBracket() && otherGame.isBracket())
            {
                if (isPool())
                {
                    if (gameDate.Equals(otherGame.gameDate))
                    {
                        if (((PoolGame)this).pool.Equals(((PoolGame)otherGame).pool))
                        {
                            if (location.Equals(otherGame.location))
                            {
                                ans = team1.division.displayName.CompareTo(otherGame.team1.division.displayName);
                            }
                            else
                            {
                                ans = location.CompareTo(otherGame.location);
                            }
                        }
                        else
                        {
                            ans = ((PoolGame)this).pool.CompareTo(((PoolGame)otherGame).pool);
                        }
                    }
                    else
                    {
                        ans = gameDate.CompareTo(otherGame.gameDate);
                    }
                }
                else
                {
                    ans = (((BracketGame)this).gameNumber.CompareTo(((BracketGame)otherGame).gameNumber));
                }
            }
            else
            {
                if (isPool() && otherGame.isBracket())
                    ans = -1;
                else
                    ans = 1;
            }
            return ans;
        }

        #endregion

        public static int HeadToHead(Team firstTeam, Team otherTeam)
        {
            int firstTeamWins = 0;
            int otherTeamWins = 0;
            foreach (Game aGame in PoolGame.find(firstTeam))
            {
                if (aGame.personVerified != null)
                {
                    if (aGame.team1.id == firstTeam.id && aGame.team2.id == otherTeam.id
                        || aGame.team2.id == firstTeam.id && aGame.team1.id == otherTeam.id)
                    {
                        if (aGame.team1.id == firstTeam.id)
                            if (aGame.score1 > aGame.score2)
                                firstTeamWins++;
                            else
                                otherTeamWins++;
                        else
                            if (aGame.score1 > aGame.score2)
                                otherTeamWins++;
                            else
                                firstTeamWins++;
                    }
                }
            }
            if (firstTeamWins > otherTeamWins)
                return 1;
            if (firstTeamWins < otherTeamWins)
                return -1;
            else
                return 0;
        }
    }
    public class PoolGame : Game
    {
        private string _pool;
        public string pool
        {
            get { return _pool; }
            set
            {
                if (_pool == null || !_pool.Equals(value))
                {
                    _pool = value;
                    _updated = true;
                }
            }
        }
        public override bool isPool()
        {
            return true;
        }
        public override bool isBracket()
        {
           return false;
        }
        public override bool isInterleague()
        {
            return false;
        }
        public PoolGame()
        {
            id = -1;
        }
        public new static ArrayList find(Team aTeam)
        {
            return Game.find(aTeam, "P");
        }
    }
    public class InterleagueGame : Game
    {
        public override bool isPool()
        {
            return false;
        }
        public override bool isBracket()
        {
            return false;
        }
        public override bool isInterleague()
        {
            return true;
        }
        public InterleagueGame()
        {
            id = -1;
        }
        public new static ArrayList find(Team aTeam)
        {
            return Game.find(aTeam, "P");
        }
    }
    public class BracketGame : Game
    {
        private int _gameNumber;
        private int _nextWinnerGameNumber;
        private int _nextLoserGameNumber;
        private string _pool1;
        private string _pool2;
        public int gameNumber
        {
            get { return _gameNumber; }
            set
            {
                if (_gameNumber != value)
                {
                    _gameNumber = value;
                    _updated = true;
                }
            }
        }
        public int nextWinnerGameNumber
        {
            get { return _nextWinnerGameNumber; }
            set
            {
                if (_nextWinnerGameNumber != value)
                {
                    _nextWinnerGameNumber = value;
                    _updated = true;
                }
            }
        }
        public int nextLoserGameNumber
        {
            get { return _nextLoserGameNumber; }
            set
            {
                if (_nextLoserGameNumber != value)
                {
                    _nextLoserGameNumber = value;
                    _updated = true;
                }
            }
        }
        public string pool1
        {
            get { return _pool1; }
            set
            {
                if (_pool1 == null || _pool1 != value)
                {
                    _pool1 = value;
                    _updated = true;
                }
            }
        }
        public string pool2
        {
            get { return _pool2; }
            set
            {
                if (pool2 == null || _pool2 != value)
                {
                    _pool2 = value;
                    _updated = true;
                }
            }
        }
        public override bool isPool()
        {
           return false;
        }
        public override bool isBracket()
        {
            return true;
        }
        public override bool isInterleague()
        {
            return false;
        }
        public new static ArrayList find(Team aTeam)
        {
            return Game.find(aTeam, "B");
        }
        public BracketGame()
        {
            id = -1;
        }
        public BracketGame(Division aDivision)
        {
            id = -1;
            gameNumber = 0;
            foreach (BracketGame aGame in AllBracketGames)
            {
                if (aGame.team1.division.Equals(aDivision) && aGame.gameNumber <= gameNumber)
                    gameNumber = aGame.gameNumber+1;
            }
        }
    }
    public class Results : DistrictObject
    {
        private int _gameId;
        private int _score1;
        private int _score2;
        private int _fieldInnings1;
        private int _fieldInnings2;
        private string _recordedBy;
        private string _phone;
        private DateTime _dateReceived;
        private string _comments;

        public Results()
        {
            id = -1;
        }
        public Game theGame {
            get { return Game.find(_gameId);}
            set
            {
                if (_gameId != value.id)
                {
                    _gameId = value.id;
                    _updated = true;
                }
            }
        }
        public int score1
        {
            get { return _score1; }
            set
            {
                if (!_score1.Equals(value))
                {
                    _score1 = value;
                    _updated = true;
                }
            }
        }
        public int score2
        {
            get { return _score2; }
            set
            {
                if (!_score2.Equals(value))
                {
                    _score2 = value;
                    _updated = true;
                }
            }
        }
        public int fieldInnings1
        {
            get { return _fieldInnings1; }
            set
            {
                if (!_fieldInnings1.Equals(value))
                {
                    _fieldInnings1 = value;
                    _updated = true;
                }
            }
        }
        public int fieldInnings2
        {
            get { return _fieldInnings2; }
            set
            {
                if (!_fieldInnings2.Equals(value))
                {
                    _fieldInnings2 = value;
                    _updated = true;
                }
            }
        }
        public string recordedBy
        {
            get { return _recordedBy; }
            set
            {
                if (_recordedBy == null || !_recordedBy.Equals(value))
                {
                    _recordedBy = value;
                    _updated = true;
                }
            }
        }
        public string phoneContact
        {
            get { return _phone; }
            set
            {
                if (_phone == null || !_phone.Equals(value))
                {
                    _phone = value;
                    _updated = true;
                }
            }
        }
        public string comments
        {
            get { return _comments; }
            set
            {
                if (_comments == null || !_comments.Equals(value))
                {
                    _comments = value;
                    _updated = true;
                }
            }
        }
        public string team1
        {
            get { return theGame.team1.displayName; }
        }
        public string team2
        {
            get { return theGame.team2.displayName; }
        }

        public DateTime dateReceived
        {
            get { return _dateReceived; }
            set
            {
                if (_dateReceived == null || !_dateReceived.Equals(value))
                {
                    _dateReceived = value;
                    _updated = true;
                }
            }
        }


        public bool save()
        {
            return DatabaseManager.save(this);
        }

    }
    public class teamComparer:DistrictObject,IComparer
    {
        public int Compare(Team firstTeam, Team otherTeam)
        {
            int ans = 0;

            if (firstTeam.isDistrictTeam())
            {
                DistrictTeam firstDistrictTeam = (DistrictTeam)firstTeam;
                DistrictTeam otherDistrictTeam = (DistrictTeam)otherTeam;

                if (firstDistrictTeam.pool.ToUpper().Equals(otherDistrictTeam.pool.ToUpper()))
                {
                    if (firstDistrictTeam.wins == otherDistrictTeam.wins)
                    {
                        if (Game.HeadToHead(firstDistrictTeam, otherDistrictTeam) == 0)
                        {
                            ans = -1 * firstDistrictTeam.runsPerInning.CompareTo(otherDistrictTeam.runsPerInning);
                        }
                        else
                        {
                            ans = Game.HeadToHead(firstDistrictTeam, otherDistrictTeam);
                        }
                    }
                    else
                        ans = firstDistrictTeam.wins.CompareTo(otherDistrictTeam.wins);
                }
                else
                {
                    ans = firstDistrictTeam.pool.ToUpper().CompareTo(otherDistrictTeam.pool.ToUpper());
                }
            }
            return (ans * -1);
        }

        #region IComparer Members

        int IComparer.Compare(object x, object y)
        {
            return Compare((Team)x, (Team)y);
        }

        #endregion
    }

    public class teamRPIComparer : DistrictObject, IComparer
    {
        public int Compare(Team firstTeam, Team otherTeam)
        {
            return firstTeam.runsPerInning.CompareTo(otherTeam.runsPerInning);
        }

        #region IComparer Members

        int IComparer.Compare(object x, object y)
        {
            return Compare((Team)x, (Team)y);
        }

        #endregion
    }

    public class WebData : DistrictObject
    {
        private static ArrayList pWebDataList;
        private string pPage;
        private string pTag;
        private string pText;

        public string page
        {
            get { return pPage; }
            set { pPage = value; }
        }
        public string tag
        {
            get { return pTag; }
            set { pTag = value; }
        }
        public string text
        {
            get { return pText; }
            set { pText = value; }
        }

        public static ArrayList webDataList
        {
            get
            {
                if (pWebDataList == null)
                    pWebDataList = DatabaseManager.getAllWebData();
                return pWebDataList;
            }
        }

        public void reset()
        {
            pWebDataList = null;
        }

        public static ArrayList find(string aPage)
        {
            ArrayList tempList = new ArrayList();
            foreach (WebData aData in webDataList)
            {
                if (aData.page.Equals(aPage))
                    tempList.Add(aData);
            }
            return tempList;
        }
        public static WebData find(string aPage,string aTag)
        {
            WebData returnData = null;
            foreach (WebData aData in webDataList)
            {
                if (aData.page.Equals(aPage) && aData.tag.Equals(aTag))
                {
                    returnData = aData;
                    break;
                }
            }
            return returnData;
        }

        public bool save()
        {
            return DatabaseManager.save(this);
        }
    }

    public class CalendarItem : DistrictObject,IComparable
    {
        private static ArrayList pAllItems;
        private DateTime pDate;
        private string pSubject;
        private string pPlace;
        private string pDescription;

        public DateTime date
        {
            get { return pDate; }
            set { pDate = value; }
        }
        public string subject
        {
            get { return pSubject; }
            set { pSubject = value; }
        }
        public string place
        {
            get { return pPlace; }
            set { pPlace = value; }
        }
        public string description
        {
            get { return pDescription; }
            set { pDescription = value; }
        }

        public CalendarItem()
        {
            id = -1;
            date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 00, 00);
        }

        public static ArrayList allItema
        {
            get
            {
                if (pAllItems == null)
                {
                    pAllItems = DatabaseManager.getAllCalendarEntries();
                    pAllItems.Sort();
                }
                return pAllItems;
            }
        }
        public static void reset()
        {
            pAllItems = null;
        }
        public bool save()
        {
            bool ans = false;
            if (id == -1)
            {
                ans = DatabaseManager.insert(this);
                reset();
            }
            else
            {
                ans =  DatabaseManager.save(this);
            }
            return ans;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (date.CompareTo(((CalendarItem)obj).date)==0)
                return subject.CompareTo(((CalendarItem)obj).subject);
            else
                return date.CompareTo(((CalendarItem)obj).date);
        }

        #endregion

        public bool delete()
        {
            if (DatabaseManager.delete(this))
            {
                pAllItems = null;
                return true;
            }
            else
                return false;
        }
    }

    public class SpecialGame : DistrictObject, IComparable
    {
        private static ArrayList pAllSpecialGames;

        private string pTournament;
        private string pType;
        private string pAgeRange;
        private int pHostLeague;
        private string pStatus;
        private DistrictLink pTournamentRules;
        private DistrictLink pRegistrationForm;
        private DistrictLink pWebSite;

        public string tournament
        {
            get { return pTournament; }
            set { pTournament = value; }
        }
        public string type
        {
            get { return pType; }
            set { pType = value; }
        }
        public string ageRange
        {
            get { return pAgeRange; }
            set { pAgeRange = value; }
        }
        public League hostLeague
        {
            get { return League.find(pHostLeague); }
            set { pHostLeague = value.id; }
        }
        public string status
        {
            get { return pStatus; }
            set { pStatus = value; }
        }
        public DistrictLink tournamentRules
        {
            get { return pTournamentRules; }
            set { pTournamentRules = value; }
        }
        public DistrictLink registrationForm
        {
            get { return pRegistrationForm; }
            set { pRegistrationForm = value; }
        }
        public DistrictLink webSite
        {
            get { return pWebSite; }
            set { pWebSite = value; }
        }

        public static ArrayList allSpecialGames
        {
            get
            {
                if (pAllSpecialGames == null)
                {
                    pAllSpecialGames = DatabaseManager.getAllSpecialGames();
                    pAllSpecialGames.Sort();
                }
                return pAllSpecialGames;
            }
        }

        public SpecialGame()
        {
            id = -1;
        }
        public bool save()
        {
            bool ans = true;
            if (id == -1)
            {
                ans = DatabaseManager.insert(this);
                reset();
            }
            else
                ans = DatabaseManager.save(this);
            return ans;
        }
        public static void reset()
        {
            pAllSpecialGames = null;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            int ans = 0;
            SpecialGame otherGame = (SpecialGame)obj;
            if (tournament.CompareTo(otherGame.tournament) == 0)
            {
                ans = hostLeague.CompareTo(otherGame.hostLeague);
            }
            else
                ans = tournament.CompareTo(otherGame.tournament);
            return ans;
        }

        #endregion
    }

    public class PastChampion : DistrictObject
    {
        private static string sectionColor = "#66FF66";
        private static string stateColor = "#ffff00";
        private static string regionColor = "#00ffcc";
        private static string nationalColor = "#ff9966";

        private static ArrayList _allChamps = new ArrayList();
        private static int _masterId = 0;
        private string _fontColor;
        private string _team;
        private int _year;
        private string _otherText;
        private bool _sections;
        private bool _states;
        private bool _regions;
        private bool _nationals;

        public string team
        {
            get { return _team; }
            set { _team = value; }
        }
        public string cellColor
        {
            get
            {
                string backgroundColor = "";
                if (nationals)
                    backgroundColor = nationalColor;
                else if (regions)
                    backgroundColor = regionColor;
                else if (states)
                    backgroundColor = stateColor;
                else if (sections)
                    backgroundColor = sectionColor;
                else
                    backgroundColor = "White";
                return backgroundColor;
            }
        }
        public string fontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }
        public int year
        {
            get { return _year; }
            set { _year = value; }
        }
        public string otherText
        {
            get { return _otherText; }
            set { _otherText = value; }
        }
        public bool sections
        {
            get { return _sections; }
            set { _sections = value; }
        }
        public bool states
        {
            get { return _states; }
            set { _states = value; }
        }
        public bool regions
        {
            get { return _regions; }
            set { _regions = value; }
        }
        public bool nationals
        {
            get { return _nationals; }
            set { _nationals = value;}
        }

        public static void reset()
        {
            _allChamps = new ArrayList();
            _masterId = 0;
        }

        internal PastChampion(System.Xml.XmlNode aNode)
        {
            id = _masterId++;
            team = aNode["team"].InnerText;
            otherText = aNode["otherText"].InnerText;
            sections = Convert.ToBoolean(aNode["Sections"].InnerText);
            regions = Convert.ToBoolean(aNode["Regions"].InnerText);
            states = Convert.ToBoolean(aNode["States"].InnerText);
            nationals = Convert.ToBoolean(aNode["Nationals"].InnerText);
            _allChamps.Add(this);
        }
        public PastChampion()
        {
            id = _masterId++;
            _allChamps.Add(this);
        }

        internal System.Xml.XmlElement toXML(System.Xml.XmlDocument aDoc)
        {
            System.Xml.XmlElement champElement = aDoc.CreateElement("Champion");
            System.Xml.XmlElement teamElement = aDoc.CreateElement("team");
            System.Xml.XmlElement otherTextElement = aDoc.CreateElement("otherText");
            System.Xml.XmlElement SectionsElement = aDoc.CreateElement("Sections");
            System.Xml.XmlElement RegionsElement = aDoc.CreateElement("Regions");
            System.Xml.XmlElement StatesElement = aDoc.CreateElement("States");
            System.Xml.XmlElement NationalsElement = aDoc.CreateElement("Nationals");

            teamElement.InnerText = team;
            otherTextElement.InnerText = otherText;
            SectionsElement.InnerText = sections.ToString();
            RegionsElement.InnerText = regions.ToString();
            StatesElement.InnerText = states.ToString();
            NationalsElement.InnerText = nationals.ToString();

            champElement.AppendChild(teamElement);
            champElement.AppendChild(otherTextElement);
            champElement.AppendChild(SectionsElement);
            champElement.AppendChild(RegionsElement);
            champElement.AppendChild(StatesElement);
            champElement.AppendChild(NationalsElement);

            return champElement;
        }

        public static PastChampion find(int anId)
        {
            PastChampion temp = null;
            foreach (PastChampion aChamp in _allChamps)
            {
                if (aChamp.id == anId)
                {
                    temp = aChamp;
                    break;
                }
            }
            return temp;
        }
    }
    public class PastChampionSeason:DistrictObject,IComparable
    {
        private static ArrayList _allChampions;
        public int _year;
        public PastChampion _bbMinors;
        public PastChampion _bbMinors2;
        public PastChampion _bbMajors;
        public PastChampion _bbIntermediate;
        public PastChampion _bbJuniors;
        public PastChampion _bbSeniors;
        public PastChampion _sbMinors;
        public PastChampion _sbMinors2;
        public PastChampion _sbMajors;
        public PastChampion _sbJuniors;
        public PastChampion _sbSeniors;

        public static void reset()
        {
            _allChampions = null;
        }

        public string cellStyleYear
        {
            get { return ""; }
        }

        public static ArrayList allChampions
        {
            get
            {
                if (_allChampions == null)
                    getAllChampions();
                _allChampions.Sort();
                return _allChampions;
            }
        }

        private static void getAllChampions()
        {
            _allChampions = new ArrayList();
            string xmlDocString = getDistrictData("grdPastChampions");
            System.Xml.XmlDocument aDoc = new System.Xml.XmlDocument();
            aDoc.LoadXml(xmlDocString);
            if (xmlDocString.Trim().Length > 0)
            {
                aDoc.LoadXml(xmlDocString);
                foreach (System.Xml.XmlNode xmlNode in aDoc["Champions"].ChildNodes)
                {

                    PastChampionSeason aSeason = new PastChampionSeason();
                    aSeason.year = Convert.ToInt32(xmlNode["value"].InnerText);
                    foreach (System.Xml.XmlNode tChamps in xmlNode["ChampionList"].ChildNodes)
                    {
                        PastChampion aChamp = new PastChampion(tChamps);
                        if (tChamps.Attributes["node"].InnerText.Equals("bbMinors"))
                            aSeason.bbMinors = aChamp;
                        else if (tChamps.Attributes["node"].InnerText.Equals("bbMinors2"))
                            aSeason.bbMinors2 = aChamp;
                        else if (tChamps.Attributes["node"].InnerText.Equals("bbMajors"))
                            aSeason.bbMajors = aChamp;
                        else if (tChamps.Attributes["node"].InnerText.Equals("bbJuniors"))
                            aSeason.bbJuniors = aChamp;
                        else if (tChamps.Attributes["node"].InnerText.Equals("bbSeniors"))
                            aSeason.bbSeniors = aChamp;
                        else if (tChamps.Attributes["node"].InnerText.Equals("sbMinors"))
                            aSeason.sbMinors = aChamp;
                        else if (tChamps.Attributes["node"].InnerText.Equals("sbMinors2"))
                            aSeason.sbMinors2 = aChamp;
                        else if (tChamps.Attributes["node"].InnerText.Equals("sbMajors"))
                            aSeason.sbMajors = aChamp;
                        else if (tChamps.Attributes["node"].InnerText.Equals("sbJuniors"))
                            aSeason.sbJuniors = aChamp;
                        else if (tChamps.Attributes["node"].InnerText.Equals("sbSeniors"))
                            aSeason.sbSeniors = aChamp;
                        else if (tChamps.Attributes["node"].InnerText.Equals("bbIntermediate"))
                            aSeason.bbIntermediate = aChamp;
                    }
//                    _allChampions.Add(aSeason);
                }
            }
        }

        public int year
        {
            get { return _year; }
            set { _year = value; }
        }
        public PastChampion bbMinors
        {
            get { return _bbMinors; }
            set { _bbMinors = value; }
        }
        public PastChampion bbMinors2
        {
            get { return _bbMinors2; }
            set { _bbMinors2 = value; }
        }
        public PastChampion bbMajors
        {
            get { return _bbMajors; }
            set { _bbMajors = value; }
        }
        public PastChampion bbIntermediate
        {
            get { if (_bbIntermediate == null) {
                    _bbIntermediate = new PastChampion();
                }
                return  _bbIntermediate; }
            set { _bbIntermediate = value; }
        }
        public PastChampion bbJuniors
        {
            get { return _bbJuniors; }
            set { _bbJuniors = value; }
        }
        public PastChampion bbSeniors
        {
            get { return _bbSeniors; }
            set { _bbSeniors = value; }
        }
        public PastChampion sbMinors
        {
            get { return _sbMinors; }
            set { _sbMinors = value; }
        }
        public PastChampion sbMinors2
        {
            get { return _sbMinors2; }
            set { _sbMinors2 = value; }
        }
        public PastChampion sbMajors
        {
            get { return _sbMajors; }
            set { _sbMajors = value; }
        }
        public PastChampion sbJuniors
        {
            get { return _sbJuniors; }
            set { _sbJuniors = value; }
        }
        public PastChampion sbSeniors
        {
            get { return _sbSeniors; }
            set { _sbSeniors = value; }
        }

        public PastChampionSeason()
        {
            bbMinors = new PastChampion();
            bbMinors2 = new PastChampion();
            bbMajors = new PastChampion();
            bbJuniors = new PastChampion();
            bbSeniors = new PastChampion();

            sbMinors = new PastChampion();
            sbMinors2 = new PastChampion();
            sbMajors = new PastChampion();
            sbJuniors = new PastChampion();
            sbSeniors = new PastChampion();
            allChampions.Add(this);
        }
        public static PastChampionSeason find(int aYear)
        {
            PastChampionSeason theSeason = null;
            foreach (PastChampionSeason aSeason in allChampions)
            {
                if (aSeason.year.Equals(aYear))
                {
                    theSeason = aSeason;
                    break;
                }
            }
            return theSeason;
        }

        public bool save()
        {
            System.Xml.XmlDocument aDoc = new System.Xml.XmlDocument();
            System.Xml.XmlElement ChampionsElement = aDoc.CreateElement("Champions");
            foreach (PastChampionSeason aSeason in allChampions)
            {
                System.Xml.XmlNode aNode = aSeason.toXML(aDoc);
                ChampionsElement.AppendChild(aNode);
            }
            aDoc.AppendChild(ChampionsElement);
            return saveDistrictData("grdPastChampions", aDoc.OuterXml);
        }

        private System.Xml.XmlNode toXML(System.Xml.XmlDocument seasonDoc)
        {
            System.Xml.XmlNode seasonNode = seasonDoc.CreateElement("Year");

            System.Xml.XmlNode yearElement = seasonDoc.CreateElement("value");

            yearElement.InnerText = year.ToString();
            seasonNode.AppendChild(yearElement);

            System.Xml.XmlNode championListElement = seasonDoc.CreateElement("ChampionList");

            System.Xml.XmlNode bbMinorsNode = bbMinors.toXML(seasonDoc);
            System.Xml.XmlAttribute attrbbMinors = seasonDoc.CreateAttribute("node");
            attrbbMinors.InnerText = "bbMinors";
            bbMinorsNode.Attributes.Append(attrbbMinors);
            championListElement.AppendChild(bbMinorsNode);

            System.Xml.XmlNode bbMinors2Node = bbMinors2.toXML(seasonDoc);
            System.Xml.XmlAttribute attrbbMinors2 = seasonDoc.CreateAttribute("node");
            attrbbMinors2.InnerText = "bbMinors2";
            bbMinors2Node.Attributes.Append(attrbbMinors2);
            championListElement.AppendChild(bbMinors2Node);

            System.Xml.XmlNode bbMajorsNode = bbMajors.toXML(seasonDoc);
            System.Xml.XmlAttribute attrbbMajors = seasonDoc.CreateAttribute("node");
            attrbbMajors.InnerText = "bbMajors";
            bbMajorsNode.Attributes.Append(attrbbMajors);
            championListElement.AppendChild(bbMajorsNode);

            System.Xml.XmlNode bbIntermediateNode = bbIntermediate.toXML(seasonDoc);
            System.Xml.XmlAttribute attrbbIntermediate = seasonDoc.CreateAttribute("node");
            attrbbIntermediate.InnerText = "bbIntermediate";
            bbIntermediateNode.Attributes.Append(attrbbIntermediate);
            championListElement.AppendChild(bbIntermediateNode);

            System.Xml.XmlNode bbJuniorsNode = bbJuniors.toXML(seasonDoc);
            System.Xml.XmlAttribute attrbbJuniors = seasonDoc.CreateAttribute("node");
            attrbbJuniors.InnerText = "bbJuniors";
            bbJuniorsNode.Attributes.Append(attrbbJuniors);
            championListElement.AppendChild(bbJuniorsNode);

            System.Xml.XmlNode bbSeniorsNode = bbSeniors.toXML(seasonDoc);
            System.Xml.XmlAttribute attrbbSeniors = seasonDoc.CreateAttribute("node");
            attrbbSeniors.InnerText = "bbSeniors";
            bbSeniorsNode.Attributes.Append(attrbbSeniors);
            championListElement.AppendChild(bbSeniorsNode);

            System.Xml.XmlNode sbMinorsNode = sbMinors.toXML(seasonDoc);
            System.Xml.XmlAttribute attrsbMinors = seasonDoc.CreateAttribute("node");
            attrsbMinors.InnerText = "sbMinors";
            sbMinorsNode.Attributes.Append(attrsbMinors);
            championListElement.AppendChild(sbMinorsNode);

            System.Xml.XmlNode sbMinors2Node = sbMinors2.toXML(seasonDoc);
            System.Xml.XmlAttribute attrsbMinors2 = seasonDoc.CreateAttribute("node");
            attrsbMinors2.InnerText = "sbMinors2";
            sbMinors2Node.Attributes.Append(attrsbMinors2);
            championListElement.AppendChild(sbMinors2Node);

            System.Xml.XmlNode sbMajorsNode = sbMajors.toXML(seasonDoc);
            System.Xml.XmlAttribute attrsbMajors = seasonDoc.CreateAttribute("node");
            attrsbMajors.InnerText = "sbMajors";
            sbMajorsNode.Attributes.Append(attrsbMajors);
            championListElement.AppendChild(sbMajorsNode);

            System.Xml.XmlNode sbJuniorsNode = sbJuniors.toXML(seasonDoc);
            System.Xml.XmlAttribute attrsbJuniors = seasonDoc.CreateAttribute("node");
            attrsbJuniors.InnerText = "sbJuniors";
            sbJuniorsNode.Attributes.Append(attrsbJuniors);
            championListElement.AppendChild(sbJuniorsNode);

            System.Xml.XmlNode sbSeniorsNode = sbSeniors.toXML(seasonDoc);
            System.Xml.XmlAttribute attrsbSeniors = seasonDoc.CreateAttribute("node");
            attrsbSeniors.InnerText = "sbSeniors";
            sbSeniorsNode.Attributes.Append(attrsbSeniors);
            championListElement.AppendChild(sbSeniorsNode);

            seasonNode.AppendChild(championListElement);
            return seasonNode ;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            PastChampionSeason otherSeason = (PastChampionSeason)obj;

            return -1 * (year.CompareTo(otherSeason.year));

        }

        #endregion
    }

    public class Umpire : DistrictObject, IComparable
    {
        private static ArrayList _allUmps;
        private string _firstName;
        private string _lastName;
        private string _umpireSince;
        private string _homeLeague;
        private string _credits;
        private string _image;

        public string firstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string lastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string umpireSince
        {
            get { return _umpireSince; }
            set { _umpireSince = value; }
        }
        public string homeLeague
        {
            get { return _homeLeague; }
            set { _homeLeague = value; }
        }
        public string credits
        {
            get { return _credits; }
            set { _credits = value; }
        }
        public string image
        {
            get { return _image; }
            set { _image = value; }
        }

        public string fullName
        {
            get
            {
                return firstName.Trim() + " " + lastName.Trim();
            }
        }

        public string umpireDetailLink
        {
            get
            {
                return "UmpireDetail.aspx?id=" + id.ToString();
            }
        }

        public static ArrayList allUmps
        {
            get
            {
                if (_allUmps == null)
                    _allUmps = DatabaseManager.getAllUmpires();
                return _allUmps;
            }
        }

        public Umpire()
        {
            id = -1;
        }
        public static Umpire find (int anId) {

            Umpire temp = null;
            foreach (Umpire anUmp in allUmps)
            {
                if (anUmp.id.Equals(anId)) {
                    temp = anUmp;
                    break;
                }
            }
            return temp;
        }
        public static void reset()
        {
            _allUmps = null;
        }

        public bool save()
        {
            if (DatabaseManager.save(this))
            {
                _allUmps = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (lastName.CompareTo(((Umpire)obj).lastName) == 0)
                return firstName.CompareTo(((Umpire)obj).firstName);
            else
                return lastName.CompareTo(((Umpire)obj).lastName);
        }

        #endregion
    }
    public class Location : DistrictObject
    {
        private static ArrayList _allLocations;
        private string _name;
        private string _street;
        private string _city;
        private string _state;
        private string _zip;
        private string _directions;
        private League _hostLeague;
        private string _latatude;
        private string _longitude;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string street
        {
            get { return _street; }
            set { _street = value; }
        }
        public string city
        {
            get { return _city; }
            set { _city = value; }
        }
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }
        public string zip
        {
            get { return _zip; }
            set { _zip = value; }
        }
        public string directions
        {
            get { return _directions; }
            set { _directions = value; }
        }
        public League hostLeague
        {
            get { return _hostLeague; }
            set { _hostLeague = value; }
        }
        public string latitude
        {
            get { return _latatude; }
            set { _latatude = value; }
        }
        public string longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        public static ArrayList allLocations
        {
            get
            {
                if (_allLocations == null)
                    _allLocations = DatabaseManager.getAllLocations();
                return _allLocations;
            }
        }
        public static ArrayList leagueLocations(League aLeague)
        {
            ArrayList temp = new ArrayList();
            foreach (Location aLoc in allLocations)
            {
                if(aLoc.hostLeague.id.Equals(aLeague.id))
                    temp.Add(aLoc);
            }
            return temp;
        }
        public bool save()
        {
            bool ans = false;
            if (id == -1)
            {
                ans = DatabaseManager.insert(this);
                _allLocations = null;
            }
            else
            {
                ans =  DatabaseManager.save(this);
            }
            return ans;
        }
        public static void reset()
        {
            _allLocations = null;
        }
    }
    public class DistrictMinutes:DistrictObject
    {
        private static ArrayList _allMinutes;
        private string _year;
        private string _month;
        private string _fileName;

        public string year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
            }
        }
        public string month
        {
            get { return _month; }
            set { _month = value; }
        }
        public string fileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public static ArrayList allDistrictMinutes
        {
            get
            {
                if (_allMinutes == null)
                    _allMinutes = DatabaseManager.getAllMinutes();
                return _allMinutes;
            }
        }

        public static void reset()
        {
            _allMinutes = null;
        }

        public static DistrictMinutes find(int anId)
        {
            DistrictMinutes tMinutes = null;
            foreach (DistrictMinutes aMinutes in allDistrictMinutes)
            {
                if (aMinutes.id == anId)
                {
                    tMinutes = aMinutes;
                    break;
                }
            }
                return tMinutes;
        }

        public DistrictMinutes()
        {
            id = -1;
        }

        public static DistrictMinutes find(string aMonth, string aYear)
        {
            DistrictMinutes tMinutes = null;
            foreach (DistrictMinutes aMinutes in allDistrictMinutes)
            {
                if (aMinutes.year == aYear && aMinutes.month == aMonth)
                {
                    tMinutes = aMinutes;
                    break;
                }
            }
            return tMinutes;
        }

        public bool Save()
        {
            bool ans = false;
            if (id == -1)
            {
                ans = DatabaseManager.insert(this);
                _allMinutes = null;
            }
            return ans;
        }
    }

    public class DistrictCard:DistrictObject,IEquatable<DistrictCard>, IComparable
    {
        private static CardCollection _allCards = null;
        private int _cardNumber;
        private string _cardTitle;
        private string _cardBody;
        private string _priority;
        private string _image;

        public static CardCollection allCards
        {
            get
            {
                string jsonString = null;
                if (_allCards  == null)
                {
                    CardCollection temp = new CardCollection();

                    _allCards = new CardCollection();
                   jsonString = DistrictObject.getDistrictData("lblCards");
                 //jsonString = "{\"Cards\":[{\"CardClass\":\"<div class = 'carousel-item active' />\",\"cardNumber\":2,\"cardTitle\":\"2019 District 1 State Champion\",\"cardBody\":\"This is a test.\",\"priority\":\"Hot\",\"CardColor\":\"<div class = 'card text-white bg-danger '>\",\"cardFooter\":\"2/6/2021 2:10:00 PM by jbooch@booch.net\",\"id\":0,\"updated\":false,\"lastUser\":\"jbooch@booch.net\",\"LastTmp\":\"2021-02-06T14:10:00.841928-05:00\"}],\"id\":0,\"updated\":false,\"LastTmp\":\"0001-01-01T00:00:00\"}";
                 //jsonString = "{\"Cards\":[{\"CardClass\":\" < div class = 'carousel-item active' />\",\"cardNumber\":2,\"cardTitle\":\"2019 District 1 Section Champions\",\"cardBody\":\"Junior Softball:  Wharton, Majors Softball:  Hanover Township\",\"priority\":\"Hot\",\"CardColor\":\"<div class = 'card text-white bg-danger '>\",\"cardFooter\":\"1/1/0001 12:00:00 AM by \",\"id\":1,\"updated\":false,\"LastTmp\":\"0001-01-01T00:00:00\"},{\"CardClass\":\"<div class = 'carousel-item' />\",\"cardNumber\":3,\"cardTitle\":\"2019 District 1 State Champions\",\"cardBody\":\"Senior Softball:   Dover,Senior Baseball:  Randolph, 11s Baseball:  Madison Little League\",\"priority\":\"Info\",\"CardColor\":\"<div class = 'card text-white bg-info '>\",\"cardFooter\":\"2/5/2021 9:26:55 PM by jbooch @booch.net\",\"id\":2,\"updated\":false,\"lastUser\":\"jbooch @booch.net\",\"LastTmp\":\"2021-02-05T21:26:55.1863998-05:00\"},{\"CardClass\":\"<div class = 'carousel-item active' />\",\"cardNumber\":4,\"cardTitle\":\"2019 District 1 Champions\",\"cardBody\":\"Senior Softball:   Dover<br />Junior Softball:  Wharton<br />Intermediate 50/70 Baseball:  Chester/Mendham<br />Senior Baseball:  Randolph<br />Majors Softball:  Hanover Township<br />Junior Baseball:  Randolph<br />10s Softball:  Hanover Township<br />11s Baseball:  Madison<br />Majors Baseball: Par Troy West<br />10s Baseball: Morristown American\",\"priority\":\"High\",\"CardColor\":\"<div class = 'card text-white bg-primary '>\",\"cardFooter\":\"3/7/2020 4:54:50 PM by jbooch@booch.net\",\"id\":1,\"updated\":false,\"lastUser\":\"jbooch@booch.net\",\"LastTmp\":\"2020-03-07T16:54:50.3378603-05:00\"},{\"CardClass\":\"<div class = 'carousel-item' />\",\"cardNumber\":5,\"cardTitle\":\"NJ State Majors Softball Champions\",\"cardBody\":\"Congratulations to Robbinsville for defeating Swedesboro.<br />Tournament was hosted by Par-Troy West\",\"priority\":\"Success\",\"CardColor\":\"<div class = 'card text-white bg-success '>\",\"cardFooter\":\"1/1/0001 12:00:00 AM by \",\"id\":3,\"updated\":false,\"LastTmp\":\"0001-01-01T00:00:00\"},{\"CardClass\":\"<div class = 'carousel-item' />\",\"cardNumber\":6,\"cardTitle\":\"Congratulations to Nutley American\",\"cardBody\":\"2019 East Region Champion for the Intermediate Division.\",\"priority\":\"Warning\",\"CardColor\":\"<div class = 'card text-white bg-warning '>\",\"cardFooter\":\"3/7/2020 5:24:02 PM by jbooch@booch.net\",\"id\":4,\"updated\":false,\"lastUser\":\"jbooch@booch.net\",\"LastTmp\":\"2020-03-07T17:24:02.4884027-05:00\"}]}";

                    if (jsonString != null && jsonString.Length > 0)
                    {
                        temp = JsonConvert.DeserializeObject<CardCollection>(jsonString);
                        foreach(DistrictCard aCard in temp.Cards)
                        {
                            _allCards.Add(aCard);
                        }
                    }
                 }
                _allCards.Cards.Sort();
                return _allCards;
            }
        }

        public string CardClass
        {
            get
            {
                if (((DistrictCard)cardCountArray[0]).id.Equals(this.id))
                    return "<div class = 'carousel-item active' />";
                else
                    return "<div class = 'carousel-item ' />";
            }
        }

        public static ArrayList cardCountArray
        {
            get
            {
                ArrayList aList = new ArrayList();
//                int i = 0;
                foreach(DistrictCard aCard in allCards.Cards)
                {
                    aList.Add(aCard);
                }
                return aList;
            }
        }

/*        public static bool SaveAll()
        {
            JsonSerializerSettings aSettings = new JsonSerializerSettings();
            aSettings.Formatting = Formatting.None;
            aSettings.NullValueHandling = NullValueHandling.Ignore;

            bool ans = true;
            string jsonString = null;

            if (DistrictCard.allCards.Cards.Count > 0)
                jsonString = "[" + JsonConvert.SerializeObject(DistrictCard.allCards.Cards[0], aSettings);
            for (int x=1;x<DistrictCard.allCards.Cards.Count;x++)
            {
                jsonString += ", " + JsonConvert.SerializeObject(DistrictCard.allCards.Cards[x]);
            }
            jsonString += "]";
  
            //            ans = DistrictObject.saveDistrictData("lblCards", jsonString);

            //            if (ans)
            //                Reset();

            return ans;
        }
*/
        public bool Save()
        {
            if (id == -1)
            {
                id = allCards.Cards.Count;
                allCards.Cards.Add(this);
            }
            return allCards.Save();
        }

        public int cardNumber
        {
            get { return _cardNumber; }
            set { _cardNumber = value; }
        }
        public string cardTitle
        {
            get { return _cardTitle; }
            set { _cardTitle = value; }
        }
        public string cardBody
        {
            get { return _cardBody; }
            set { _cardBody = value; }
        }
        public string priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        public string image
        {
            get { return _image; }
            set { _image = value; }
        }

        public string CardColor
        {
            //  <div class="card text-white bg-primary">

            get
            {
                if (priority.ToLower() == "info")
                    return "card text-white bg-info";
                else if (priority.ToLower() == "high")
                    return "card text-white bg-primary";
                else if (priority.ToLower() == "hot")
                    return "card text-white bg-danger";
                else if (priority.ToLower() == "success")
                    return "card text-white bg-success";
                else if (priority.ToLower()=="warning")
                    return "card text-dark bg-warning";
                else
                    return "card text-white bg-dark";
            }
        }

        public string cardFooter
        {
            get
            {
                return LastTmp.ToString() + " by " + lastUser;
            }
        }

        public static void Add(DistrictCard aCard)
        {
            allCards.Add(aCard);
        }
        public static DistrictCard find(string aTitle)
        {
            DistrictCard temp = null;
            foreach(DistrictCard aCard in allCards.Cards)
            {
                if (aCard.cardTitle == aTitle)
                {
                    temp = aCard;
                    break;
                }
            }
            return temp;
        }

        public DistrictCard() {
            id = -1;
        }
        public override string ToString()
        {
            return cardTitle;
        }

        public static void Remove(DistrictCard aCard)
        {
            allCards.Remove(aCard);
            allCards.Save(); //  SaveAll();
        }

        public bool Equals(DistrictCard other)
        {
            return this.id.Equals(other.id) && this.cardTitle.Equals(other.cardTitle);
        }

        public static void Reset()
        {
            _allCards = null;
        }

        public int CompareTo(object obj)
        {
            return cardNumber.CompareTo(((DistrictCard)obj).cardNumber);
        }
    }
    public class CardCollection : DistrictObject
    {
        private List<DistrictCard> _cards;

        public List<DistrictCard> Cards { get => _cards; set => _cards = value; }

        internal void Add(DistrictCard aCard)
        {
            Cards.Add(aCard);
        }
        public CardCollection()
        {
            Cards = new List<DistrictCard>();
        }

        public void Remove(DistrictCard aCard) {
            _cards.Remove(aCard);
        }

        public bool Save()
        {
            CardCollection temp = new CardCollection();


            foreach(DistrictCard aCard in this.Cards)
            {
                temp.Add(aCard);
            }
            
            JsonSerializerSettings aSettings = new JsonSerializerSettings();
            aSettings.Formatting = Formatting.None;
            aSettings.NullValueHandling = NullValueHandling.Ignore;

            bool ans = true;
            string jsonString = null;

            jsonString = JsonConvert.SerializeObject(temp, aSettings);
 
            ans = DistrictObject.saveDistrictData("lblCards", jsonString);

            return ans;
        }

    }

    public class SponsorCollection:DistrictObject
    {
        private static List<DistrictSponsor> pSponsors;

        public static List<DistrictSponsor> Sponsors
        {
            get
            {
                if (pSponsors == null)
                {
                    string jsonString = DistrictObject.getDistrictData("lblSponsors");

                    if (jsonString != null && jsonString.Length > 0)
                        pSponsors = JsonConvert.DeserializeObject<List<DistrictSponsor>>(jsonString);
                }
                return pSponsors;
            }
        }

        public static void Add(DistrictSponsor aSponsor)
        {
            Sponsors.Add(aSponsor);
        }
        public static void Remove (DistrictSponsor aSponsor)
        {
            Sponsors.Remove(aSponsor);
        }
        public static bool Save()
        {
            JsonSerializerSettings aSettings = new JsonSerializerSettings();
            aSettings.Formatting = Formatting.None;
            aSettings.NullValueHandling = NullValueHandling.Ignore;

            bool ans = true;
            string jsonString = null;

            jsonString = JsonConvert.SerializeObject(pSponsors, aSettings);

            ans = DistrictObject.saveDistrictData("lblSponsors", jsonString);

            return ans;
        }

        public static DistrictSponsor Find(string aCompanyName)
        {
            DistrictSponsor theSponsor = null;
            foreach(DistrictSponsor aSponsor in Sponsors)
            {
                if (aSponsor.companyName.Equals(aCompanyName))
                {
                    theSponsor = aSponsor;
                    break;
                }
            }
            return theSponsor;
        }

        public static DistrictSponsor Find(int anId)
        {
            DistrictSponsor theSponsor = null;
            foreach (DistrictSponsor aSponsor in Sponsors)
            {
                if (aSponsor.id.Equals(anId))
                {
                    theSponsor = aSponsor;
                    break;
                }
            }
            return theSponsor;
        }

        internal List<DistrictSponsor> getList()
        {
            return pSponsors;
        }

        internal static void SetMaxID()
        {
            int maxId = 10;
            foreach(DistrictSponsor aSponsor in Sponsors)
            {
                if (aSponsor.id > maxId)
                    maxId = aSponsor.id;
            }
            DistrictSponsor.setNextId(maxId++);
        }

        public static void Reset()
        {
            pSponsors = null;
        }
     }

    public class DistrictSponsor : DistrictObject
    {
        internal static int newId = 10;
        public static void setNextId(int aNumber)
        {
            newId = aNumber;
        }
        string pName;
        string pStreet1;
        string pStreet2;
        string pCity;
        string pState;
        string pZipCode;
        string pPhoneNumber;
        string pemailAddress;
        string pImage;
        string pContactName;
        string pWebSite;

        public string companyName
        {
            get
            {
                return pName;
            }
            set
            {
                pName = value;
            }
        }
        public string street1
        {
            get { return pStreet1; }
            set { pStreet1 = value; }
        }
        public string street2
        {
            get { return pStreet2; }
            set { pStreet2 = value; }
        }
        public string city
        {
            get { return pCity; }
            set
            {
                pCity = value;
            }
        }
        public string state
        {
            get { return pState; }
            set { pState = value; }
        }
        public string zipCode
        {
            get { return pZipCode; }
            set { pZipCode = value; }
        }
        public string businessPhone
        {
            get { return pPhoneNumber; }
            set { pPhoneNumber = value; }
        }
        public string emailAddress
        {
            get { return pemailAddress; }
            set
            {
                pemailAddress = value;
            }
        }
        public string image
        {
            get { return pImage; }
            set { pImage = value; }
        }
        public string contactName
        {
            get { return pContactName; }
            set
            {
                pContactName = value;
            }
        }
        public string webSite
        {
            get
            {
                return pWebSite;
            }
            set
            {
                pWebSite = value;
            }
        }

        public DistrictSponsor()
        {
            id = newId++;
        }

        public string bodyDisplay(string imagePath)
        {
            string temp = "";
            if (image != null)
            {
                // Build image in HTML
                if (webSite != null)
                    temp += "<a href=\"http://" + webSite + "\" target=\"_blank\"\"><img src=\"" + imagePath + image + "\" class=\"img-fluid\" ald=\"sponsor image\" width=\"250px\"></a>";
                else
                    temp += "<img src=\"" + imagePath +  image + "\" class='img-fluid' alt='sponsor image' width='250px'>";
            }
            else if (webSite == null)
            {
                temp += street1 + "<br/>";
                if (street2 != null)
                    temp += street2 + "<br/>";
                temp += city + "<br/>";
                temp += state + " " + zipCode + "<br/>";
            }
            else
            {
                temp += "<a href=\"http://" + webSite + "\" target=\"_blank\">" + street1 + "<br/>";
                if (street2 != null)
                    temp += street2 + "<br/>";
                temp += city + "<br/>";
                temp += state + " " + zipCode + "</a><br/>";
            }
            return temp;
        }

        public string footerDisplay()
        {
            string temp = "";
            if (emailAddress != null)
                temp += "<a href=\"MailTo:" + emailAddress + "\">" + emailAddress + "</a>&nbsp";
            if (businessPhone != null)
                temp += "Phone:&nbsp" + businessPhone;
            return temp;
        }
    }


    public class DistrictMailer:DistrictObject
    {
        private static SmtpClient _mailClient;

        public static SmtpClient mailClient
        {
            get
            {
                if (_mailClient == null)
                {
                    _mailClient = new SmtpClient();
                    /*
                    _mailClient = new SmtpClient("smtpout.secureserver.net")
                    {
                        Port = 80,
                        Credentials = new NetworkCredential("jbooch@booch.net", "uQWQTt9onFg87yHF"),
                        EnableSsl = true,
                    };
                */
                }
                return _mailClient;
            }
        }

        public static void SendEmail(string emailAddress, string subject, string body)
        {
            MailMessage theMessage = new MailMessage(new MailAddress("admin@booch.net"),new MailAddress(emailAddress));
            theMessage.Subject = subject;
            theMessage.Body = body;

            mailClient.Send(theMessage);
        }
    }
}