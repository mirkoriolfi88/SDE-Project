using SDE_Project.Models;
using SDE_Project.Response;
using SQLite;

namespace SDE_Project.SQLite
{
    public class DatabaseController
    {
        private readonly string PathDatabase = @"C:\GitRepo\SDE-Project\SDE-Project\Database\SDEProject.db3";

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

        public async Task<int> InsertCityAsync(City item)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            int ID = await database.InsertAsync(item);

            return ID;
        }

        public async Task<int> UpdateCityAsync(City item)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            int ID = await database.UpdateAsync(item);

            return ID;
        }

        public void DeleteCityAsync(City item)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            database.DeleteAsync(item);
        }

        #endregion
    }
}
