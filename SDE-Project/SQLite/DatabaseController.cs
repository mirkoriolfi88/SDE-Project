using SDE_Project.Models;
using SDE_Project.Response;
using SQLite;

namespace SDE_Project.SQLite
{
    public class DatabaseController
    {
        private readonly string PathDatabase = @"C:\Database\SDEProject.db3";

        #region Nation

        public List<Nation> GetAllNation()
        {
            List<Nation> _listNation = new List<Nation>();
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var NationItem = database.Table<Nation>();

            if (NationItem != null)
                _listNation = NationItem.ToListAsync().Result;

            return _listNation;
        }

        public Nation GetNationByCode(string CodeNation)
        {
            Nation _nationResponse = new Nation();
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var NationItem = database.Table<Nation>().Where(item => item.NationCode == CodeNation).FirstOrDefaultAsync();

            if (NationItem != null)
                _nationResponse = NationItem.Result;

            return _nationResponse;
        }

        public async Task<int> InsertNationAsync(Nation item)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            int ID = await database.InsertAsync(item);

            return ID;
        }

        public int UpdateNation(Nation item)
        {
            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            SQLiteCommand _command = new SQLiteCommand(database);

            _command.CommandText = "UPDATE Nation SET NationDescription = '" + item.NationDescription + "' WHERE NationCode = '" + item.NationCode + "'";

            int ID = _command.ExecuteNonQuery();

            return ID;
        }

        public int DeleteNation(string NationCode)
        {
            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            SQLiteCommand _command = new SQLiteCommand(database);

            _command.CommandText = "DELETE FROM Nation WHERE NationCode = '" + NationCode + "'";

            int ID = _command.ExecuteNonQuery();

            return ID;
        }

        #endregion

        #region City

        public NationOfCity GetCityNation(string CityCode)
        {
            NationOfCity _response = new NationOfCity();

            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var CityItem = database.Table<City>().Where(item => item.CityCode == CityCode).FirstOrDefaultAsync();

            if (CityItem != null)
            {
                _response.CityDescription = CityItem.Result.CityDescription;

                var NationItem = database.Table<Nation>().Where(nation => nation.NationCode == CityItem.Result.CodeNation).FirstOrDefaultAsync();

                if (NationItem != null)
                {
                    _response._esito.Executed = true;
                    _response._esito.ErrorCode = string.Empty;
                    _response._esito.ErrorDescription = string.Empty;

                    _response.NationDescription = NationItem.Result.NationDescription;
                }
                else
                {
                    _response._esito.Executed = false;
                    _response._esito.ErrorCode = "01";
                    _response._esito.ErrorDescription = "Unrecognized nation of the city.";

                    _response.NationDescription = string.Empty;
                }
            }
            else
            {
                _response._esito.Executed = false;
                _response._esito.ErrorCode = "00";
                _response._esito.ErrorDescription = "Unrecognized city.";

                _response.NationDescription = string.Empty;
            }

            return _response;
        }

        public string GetCodeNationFromCityCode(string CityCode)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);
            string NationCode = string.Empty;

            var CityItem = database.Table<City>().Where(item => item.CityCode == CityCode).FirstOrDefaultAsync();

            if (CityItem != null)
                NationCode = CityItem.Result.CodeNation;

            return NationCode;
        }

        public string GetCodeNationFromCityDescription(string CityDescription)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);
            string NationCode = string.Empty;

            var CityItem = database.Table<City>().Where(item => item.CityDescription == CityDescription).FirstOrDefaultAsync();

            if (CityItem != null)
                NationCode = CityItem.Result.CodeNation;

            return NationCode;
        }

        public List<City> GetAllCities()
        {
            List<City> _listNation = new List<City>();
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var CityItem = database.Table<City>();

            if (CityItem != null)
                _listNation = CityItem.ToListAsync().Result;

            return _listNation;
        }

        public City GetCity(string CodeCity)
        {
            City _city = new City();
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var CityItem = database.Table<City>().Where(item => item.CityCode == CodeCity).FirstOrDefaultAsync();

            if (CityItem != null)
                _city = CityItem.Result;

            return _city;
        }

        public List<City> GetAllCitiesByNation(string NationCode)
        {
            List<City> _listNation = new List<City>();
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var CityItem = database.Table<City>().Where(item => item.CodeNation == NationCode);

            if (CityItem != null)
                _listNation = CityItem.ToListAsync().Result;

            return _listNation;
        }

        public int InsertCityAsync(City item)
        {
            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            SQLiteCommand _command = new SQLiteCommand(database);

            _command.CommandText = "INSERT into City (CityCode, CityDescription, CodeNation) VALUES ('" + item.CityCode + "', '" + item.CityDescription + "', '" + item.CodeNation + "')";

            return _command.ExecuteNonQuery();
        }

        public int UpdateCity(int ID, City item)
        {
            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            SQLiteCommand _command = new SQLiteCommand(database);

            _command.CommandText = "UPDATE City SET CityDescription = '" + item.CityDescription + "', CodeNation = '" + item.CodeNation + "', CityCode = '" + item.CityCode + "' WHERE IDCity = " + ID.ToString();

            return _command.ExecuteNonQuery();
        }

        public int DeleteCity(int ID)
        {
            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            SQLiteCommand _command = new SQLiteCommand(database);

            _command.CommandText = "DELETE FROM City WHERE IDCity = " + ID.ToString();

            return _command.ExecuteNonQuery(); ;
        }

        #endregion

        #region PoinOfInterest

        public int InsertPointOfInterest(PoinInterestRequest item)
        {
            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            PointOfInterest _objToInsert = new PointOfInterest();
            var City = database.Table<City>().Where(obj => obj.CityCode == item.CityCode && obj.CodeNation == item.NationCode).FirstOrDefault();

            if (City != null)
            {
                _objToInsert.IDCity = City.IDCity;
                _objToInsert.Latitude = item.Latitude;
                _objToInsert.Longitude = item.Longitude;
                _objToInsert.Description = item.Description;

                SQLiteCommand _command = new SQLiteCommand(database);
                _command.CommandText = "INSERT INTO PointOfInterest (IDCity, Description, Latitude, Longitude) VALUES (" + City.IDCity.ToString() + ", '" + item.Description + "', '" + item.Latitude + "', '" + item.Longitude + "')";

                return _command.ExecuteNonQuery();
            }
            else
                return -1;
        }

        public int UpdatePointOfInterest(int IDPoint, PointOfInterest item)
        {
            SQLiteConnection database = new SQLiteConnection(PathDatabase);

            return database.Update(item);
        }

        public int DeletePointOfInterest(int IDPoint)
        {
            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            SQLiteCommand _command = new SQLiteCommand(database);

            _command.CommandText = "DELETE FROM PointOfInterest WHERE ID = " + IDPoint.ToString() + "";

            return _command.ExecuteNonQuery();
        }

        public PointOfInterest GetPointOfInterestByID(int ID)
        {
            SQLiteConnection database = new SQLiteConnection(PathDatabase);

            return (PointOfInterest)database.Get(ID, database.Table<PointOfInterest>().Table);
        }

        public DetailsPointInterestResponse DetailOfPointOfInterest(string NationCode, string CityCode)
        {
            DetailsPointInterestResponse response = new DetailsPointInterestResponse();
            SQLiteConnection database = new SQLiteConnection(PathDatabase);

            var NationItem = database.Table<Nation>().Where(obj => obj.NationCode == NationCode).FirstOrDefault();

            if (NationItem != null)
            {
                var CityItem = database.Table<City>().Where(item => item.CityCode == CityCode && item.CodeNation == NationCode);

                if (CityItem != null)
                {
                    List<City> _citiesList = CityItem.ToList();
                    var Points = database.Table<PointOfInterest>();

                    List<PointOfInterest> _resPoint = Points.ToList();

                    _resPoint = _resPoint.Where(item => _citiesList.Any(obj => obj.IDCity == item.IDCity)).ToList();

                    if (_resPoint.Count > 0)
                    {
                        response.esito = new Esito() { ErrorCode = string.Empty, ErrorDescription = string.Empty, Executed = true };

                        foreach (PointOfInterest obj in _resPoint)
                        {
                            var CityProperty = _citiesList.Where(c => c.IDCity == obj.IDCity).FirstOrDefault();

                            response.details.Add(new DetailsPoint()
                            {
                                item = obj,
                                CityDescription = CityProperty.CityDescription,
                                NationCode = NationItem.NationCode,
                                NationDescription = NationItem.NationDescription
                            });
                        }
                    }
                    else
                    {
                        response.esito = new Esito() { ErrorCode = "02", ErrorDescription = "No point of interest to the city", Executed = false };
                        response.details = new List<DetailsPoint>();
                    }
                }
                else
                {
                    response.esito = new Esito() { ErrorCode = "02", ErrorDescription = "No city and/or wrong code sent", Executed = false };
                    response.details = new List<DetailsPoint>();
                }
            }
            else
            {
                response.esito = new Esito() { ErrorCode = "01", ErrorDescription = "Wrong Nation Code", Executed = false };
                response.details = new List<DetailsPoint>();
            }

            return response;
        }

        public List<PointOfInterest> GetAllPointOfInterestByNationAndCode(string NationCode, string CityCode)
        {
            List<PointOfInterest> _response = new List<PointOfInterest>();

            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            var CityItem = database.Table<City>().Where(item => item.CityCode == CityCode && item.CodeNation == NationCode);

            if (CityItem != null)
            {
                List<City> _cityList = CityItem.ToList();

                var Points = database.Table<PointOfInterest>();

                _response = Points.ToList();
                _response = _response.Where(item => _cityList.Any(obj => obj.IDCity == item.IDCity)).ToList();
            }

            return _response;
        }

        public List<PointOfInterest> GetAllPointOfInterestByLongitude(string LongitudeValue)
        {
            List<PointOfInterest> _response = new List<PointOfInterest>();

            SQLiteConnection database = new SQLiteConnection(PathDatabase);

            var Points = database.Table<PointOfInterest>().Where(obj => obj.Longitude == LongitudeValue);
            _response = Points.ToList();

            return _response;
        }

        public List<PointOfInterest> GetAllPointOfInterestByLatitude(string LatitudeValue)
        {
            List<PointOfInterest> _response = new List<PointOfInterest>();

            SQLiteConnection database = new SQLiteConnection(PathDatabase);

            var Points = database.Table<PointOfInterest>().Where(obj => obj.Latitude == LatitudeValue);
            _response = Points.ToList();

            return _response;
        }

        public List<PointOfInterest> GetAllPointOfInterestByNationAndDescription(string NationCode, string PointDescription)
        {
            List<PointOfInterest> _response = new List<PointOfInterest>();

            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            var CityItem = database.Table<City>().Where(item => item.CodeNation == NationCode);

            if (CityItem != null)
            {
                List<City> _citiesList = CityItem.ToList();
                var Points = database.Table<PointOfInterest>();
                _response = Points.ToList();

                _response = _response.Where(item => _citiesList.Any(obj => obj.IDCity == item.IDCity)).ToList();
                _response = _response.Where(obj => obj.Description.Contains(PointDescription)).ToList();
            }

            return _response;
        }

        public List<PointOfInterest> GetAllPointOfInterestByNation(string NationCode)
        {
            List<PointOfInterest> _response = new List<PointOfInterest>();

            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            var CityItem = database.Table<City>().Where(item => item.CodeNation == NationCode);

            if (CityItem != null)
            {
                List<City> _citiesList = CityItem.ToList();
                var Points = database.Table<PointOfInterest>();
                _response = Points.ToList();

                _response = _response.Where(item => _citiesList.Any(obj => obj.IDCity == item.IDCity)).ToList();
            }

            return _response;
        }

        public List<PointOfInterest> GetAllPointOfInterestByCityAndDescription(int IDCity, string PointDescription)
        {
            List<PointOfInterest> _response = new List<PointOfInterest>();
            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            
            var Points = database.Table<PointOfInterest>().Where(obj => obj.IDCity == IDCity && obj.Description.Contains(PointDescription));
            _response = Points.ToList();

            return _response;
        }

        public List<PointOfInterest> GetAllPointOfInterestByCity(int IDCity)
        {
            List<PointOfInterest> _response = new List<PointOfInterest>();
            SQLiteConnection database = new SQLiteConnection(PathDatabase);

            var Points = database.Table<PointOfInterest>().Where(obj => obj.IDCity == IDCity);
            _response = Points.ToList();

            return _response;
        }

        public List<PointOfInterest> GetAllPointOfInterest()
        {
            List<PointOfInterest> _response = new List<PointOfInterest>();
            SQLiteConnection database = new SQLiteConnection(PathDatabase);

            var Points = database.Table<PointOfInterest>();
            _response = Points.ToList();

            return _response;
        }

        public PointOfInterest GetAllPointOfInterestById(int IDPoint)
        {
            PointOfInterest _response = new PointOfInterest();
            SQLiteConnection database = new SQLiteConnection(PathDatabase);

            var Points = database.Table<PointOfInterest>().Where(obj => obj.ID == IDPoint).FirstOrDefault();
            _response = Points;

            return _response;
        }

        #endregion
    }
}
