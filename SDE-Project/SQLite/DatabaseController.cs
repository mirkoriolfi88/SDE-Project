using BingMapsRESTToolkit;
using Newtonsoft.Json;
using SDE_Project.Models;
using SDE_Project.Response;
using SDE_Project.WeatherClass;
using SQLite;
using System.Net;

namespace SDE_Project.SQLite
{
    public class DatabaseController
    {
        private readonly string PathDatabase = AppDomain.CurrentDomain.BaseDirectory + @"\Database\SDEProject.db3";
        //private readonly string PathDatabase = AppDomain.CurrentDomain.BaseDirectory + @"/Database/SDEProject.db3";

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

        public NationResponseData GetNationByCode(string CodeNation)
        {
            NationResponseData _nationResponse = new NationResponseData();
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var NationItem = database.Table<Nation>().Where(item => item.NationCode == CodeNation).FirstOrDefaultAsync();

            if (NationItem != null)
            {
                Nation _nation = NationItem.Result;

                if (_nation == null)
                    _nationResponse = new NationResponseData();
                else
                {
                    _nationResponse.NationCode = _nation.NationCode;
                    _nationResponse.NationDescription = _nation.NationDescription;

                    try
                    {
                        WebRequest request = WebRequest.Create("https://restcountries.com/v3.1/alpha/" + CodeNation);
                        request.ContentType = "application/json; charset=utf-8";

                        request.Method = "GET";

                        string text;
                        var response = (HttpWebResponse)request.GetResponse();

                        using (var sr = new StreamReader(response.GetResponseStream()))
                        {
                            text = sr.ReadToEnd();
                        }

                        dynamic doStuff = JsonConvert.DeserializeObject(text);

                        if (doStuff != null)
                        {
                            _nationResponse.region = doStuff[0].region.ToString();
                            _nationResponse.subregion = doStuff[0].subregion.ToString();
                            _nationResponse.area = Convert.ToDouble(doStuff[0].area.ToString());

                            _nationResponse.borders = JsonConvert.DeserializeObject<List<string>>(doStuff[0].borders.ToString());
                            _nationResponse.timeZone = JsonConvert.DeserializeObject<List<string>>(doStuff[0].timezones.ToString());
                            _nationResponse.maps = JsonConvert.DeserializeObject<MapsClass>(doStuff[0].maps.ToString());
                            _nationResponse.continents = JsonConvert.DeserializeObject<List<string>>(doStuff[0].continents.ToString());

                            Dictionary<string, Dictionary<string, string>> deser = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(doStuff[0].currencies.ToString());

                            foreach (string _key in deser.Keys)
                            {
                                Currencies currencyObj = new Currencies()
                                {
                                    name = deser[_key]["name"],
                                    symbol = deser[_key]["symbol"]
                                };

                                _nationResponse.currencies.Add(currencyObj);
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
            else
                _nationResponse = new NationResponseData();

            return _nationResponse;
        }

        public async Task<NationInsertionResponse> InsertNationAsync(Nation item)
        {
            NationInsertionResponse _response = new NationInsertionResponse();

            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            if (string.IsNullOrEmpty(GetNationByCode(item.NationCode).NationCode))
            {
                await database.InsertAsync(item);

                _response._esito.Executed = true;
                _response._esito.ErrorCode = "";
                _response._esito.ErrorDescription = "";

                _response.NationCode = GetNationByCode(item.NationCode).NationCode;
            }
            else
            {
                _response._esito.Executed = false;
                _response._esito.ErrorCode = "00";
                _response._esito.ErrorDescription = "The Nation has already been on master data";

                _response.NationCode = item.NationCode;
            }

            return _response;
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

        public CityWithCurrentWeather GetCity(string CodeCity)
        {
            CityWithCurrentWeather _response = new CityWithCurrentWeather();
            City _city = new City();
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var CityItem = database.Table<City>().Where(item => item.CityCode == CodeCity).FirstOrDefaultAsync();

            if (CityItem != null)
            {
                _city = CityItem.Result;

                if (_city == null)
                    _response = new CityWithCurrentWeather();
                else
                {
                    _response.IDCity = _city.IDCity;
                    _response.CityCode = _city.CityCode;
                    _response.CodeNation = _city.CodeNation;
                    _response.CityDescription = _city.CityDescription;

                    string textToSearch = "apikey=M9JrCiPpaSrAft6DmRALfMsOoKIUEJyo&q=" + _city.CityDescription + ", " + _city.CodeNation + "&language=en-EN&details=false";
                    string locationKey = string.Empty;

                    try
                    {
                        WebRequest request = WebRequest.Create("http://dataservice.accuweather.com/locations/v1/cities/search?" + textToSearch);
                        request.ContentType = "application/json; charset=utf-8";

                        request.Method = "GET";

                        string text;
                        var response = (HttpWebResponse)request.GetResponse();

                        using (var sr = new StreamReader(response.GetResponseStream()))
                        {
                            text = sr.ReadToEnd();
                        }

                        List<CitySearcher> accuweatherSearch = JsonConvert.DeserializeObject<List<CitySearcher>>(text);
                        locationKey = accuweatherSearch[0].Key;
                    }
                    catch (Exception) { }

                    if (locationKey != null)
                    {
                        try
                        {
                            textToSearch = "apikey=M9JrCiPpaSrAft6DmRALfMsOoKIUEJyo&language=en-EN&details=false";
                            WebRequest request = WebRequest.Create("http://dataservice.accuweather.com/currentconditions/v1/" + locationKey + "?" + textToSearch);
                            request.ContentType = "application/json; charset=utf-8";

                            request.Method = "GET";

                            string text;
                            var response = (HttpWebResponse)request.GetResponse();

                            using (var sr = new StreamReader(response.GetResponseStream()))
                            {
                                text = sr.ReadToEnd();
                            }

                            List<CurrentConditions> weatherConditions = JsonConvert.DeserializeObject<List<CurrentConditions>>(text);

                            _response.currentConditions = weatherConditions;
                        }
                        catch (Exception) { }
                    }
                }
            }
            else
                _response = new CityWithCurrentWeather();

            return _response;
        }

        public City GetCityByCodeAndNation(string CodeCity, string NationCode)
        {
            City _city = new City();
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var CityItem = database.Table<City>().Where(item => item.CityCode == CodeCity && item.CodeNation == NationCode).FirstOrDefaultAsync();

            if (CityItem != null)
            {
                _city = CityItem.Result;

                if (_city == null)
                    _city = new City();
            }
            else
                _city = new City();

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

        public CityInsertionResponse InsertCity(City item)
        {
            CityInsertionResponse _response = new CityInsertionResponse();
            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            SQLiteCommand _command = new SQLiteCommand(database);

            if (GetCityByCodeAndNation(item.CityCode, item.CodeNation).IDCity <= 0)
            {
                _command.CommandText = "INSERT into City (CityCode, CityDescription, CodeNation) VALUES ('" + item.CityCode + "', '" + item.CityDescription + "', '" + item.CodeNation + "')";
                _command.ExecuteNonQuery();

                _response._esito.Executed = true;
                _response._esito.ErrorCode = "";
                _response._esito.ErrorDescription = "";

                _response.IDCity = GetCity(item.CityCode).IDCity;
            }
            else
            {
                _response._esito.Executed = false;
                _response._esito.ErrorCode = "00";
                _response._esito.ErrorDescription = "The city has already been in master data!";

                _response.IDCity = GetCity(item.CityCode).IDCity;
            }

            return _response;
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
            int IDCity = GetCityByCodeAndNation(item.CityCode, item.NationCode).IDCity;

            string _ApiKey = "AjjlMHgnmg_TLEotFJwVxSWYb1A7NCWTTL2w9SzVNi9PZwDPeLoKRZ7lPxXjKNat";

            if (string.IsNullOrEmpty(item.Latitude) || string.IsNullOrEmpty(item.Longitude))
            {
                //retrive the data with Bing Map
                var request = new GeocodeRequest()
                {
                    BingMapsKey = _ApiKey,
                    UserRegion = "IT",
                    Query = item.Description + " " + item.CityCode
                };

                var resources = GetResourcesFromRequest(request);

                if (resources.Length > 0)
                {
                    item.Latitude = ((Location)resources.FirstOrDefault()).Point.Coordinates[0].ToString();
                    item.Longitude = ((Location)resources.FirstOrDefault()).Point.Coordinates[1].ToString();
                }
            }

            if (IDCity > 0)
            {
                _objToInsert.IDCity = IDCity;
                _objToInsert.Latitude = item.Latitude;
                _objToInsert.Longitude = item.Longitude;
                _objToInsert.Description = item.Description;

                SQLiteCommand _command = new SQLiteCommand(database);
                _command.CommandText = "INSERT INTO PointOfInterest (IDCity, Description, Latitude, Longitude) VALUES (" + IDCity.ToString() + ", '" + item.Description + "', '" + item.Latitude + "', '" + item.Longitude + "')";

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

        public PoinOfInterestWithActivity GetAllPointOfInterestById(int IDPoint)
        {
            PointOfInterest _point = new PointOfInterest();
            SQLiteConnection database = new SQLiteConnection(PathDatabase);
            List<BusinessActivity> _listActivity = new List<BusinessActivity>();

            PoinOfInterestWithActivity _response = new PoinOfInterestWithActivity();

            var Points = database.Table<PointOfInterest>().Where(obj => obj.ID == IDPoint).FirstOrDefault();
            _point = Points;

            if (_point.ID != null)
            {
                if (_point.ID > 0)
                {
                    Coordinate cpoint = new Coordinate(double.Parse(_point.Latitude.Replace('.', ',')), double.Parse(_point.Longitude.Replace('.', ',')));

                    try
                    {
                        var request = new LocationRecogRequest()
                        {
                            BingMapsKey = "AjjlMHgnmg_TLEotFJwVxSWYb1A7NCWTTL2w9SzVNi9PZwDPeLoKRZ7lPxXjKNat"
                            ,
                            CenterPoint = cpoint
                            ,
                            Radius = 2
                            ,
                            DistanceUnits = DistanceUnitType.Kilometers
                        };

                        var resources = GetResourcesFromRequest(request);
                        var r = (resources[0] as LocationRecog);

                        if (r.AddressOfLocation.Length > 0)
                            Console.WriteLine($"Address:\n{r.AddressOfLocation.ToString()}");

                        if (r.BusinessAtLocation != null)
                        {
                            foreach (LocalBusiness business in r.BusinessAtLocation)
                            {
                                BusinessActivity _item = new BusinessActivity()
                                {
                                    EntityName = business.BusinessInfo.EntityName
                                    ,
                                    Address = business.BusinessAddress.FormattedAddress
                                    ,
                                    Phone = business.BusinessInfo.Phone != null ? business.BusinessInfo.Phone : string.Empty
                                };

                                _listActivity.Add(_item);
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }

            _response._pointOfInterest = _point;
            _response._listActivity = _listActivity;

            return _response;
        }

        static Resource[] GetResourcesFromRequest(BaseRestRequest rest_request)
        {
            var r = ServiceManager.GetResponseAsync(rest_request).GetAwaiter().GetResult();

            if (!(r != null && r.ResourceSets != null &&
                r.ResourceSets.Length > 0 &&
                r.ResourceSets[0].Resources != null &&
                r.ResourceSets[0].Resources.Length > 0))

                throw new Exception("No results found.");

            return r.ResourceSets[0].Resources;
        }

        #endregion
    }
}
