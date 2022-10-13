using SDE_Project.Models;
using SQLite;

namespace SDE_Project.SQLite
{
    public class DatabaseController
    {
        private readonly string PathDatabase = "/Database/SDEProject.db3";

        #region Nation

        public int GetNationID(string NationCode)
        {
            int IDNation = -1;
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var NationItem = database.Table<Nation>().Where(item => item.NationCode == NationCode).FirstOrDefaultAsync();

            if (NationItem != null)
                IDNation = NationItem.Result.IDNation;

            return IDNation;
        }

        public List<Nation> GetAllNation()
        {
            List<Nation> _listNation = new List<Nation>();
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var NationItem = database.Table<Nation>();

            if (NationItem != null)
                _listNation = NationItem.ToListAsync().Result;

            return _listNation;
        }

        public async Task<int> InsertNationAsync(Nation item)
        {
            int ID = -1;
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            ID = await database.InsertAsync(item);

            return ID;
        }

        public async Task<int> UpdateNationAsync(Nation item)
        {
            int ID = -1;
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            ID = await database.UpdateAsync(item);

            return ID;
        }

        public void DeleteNationAsync(Nation item)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            database.DeleteAsync(item);
        }

        #endregion

        #region City

        public int GetCityNation(string CityDescription)
        {
            int IDCity = -1;
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(PathDatabase);

            var CityItem = database.Table<City>().Where(item => item.CityDescription == CityDescription).FirstOrDefaultAsync();

            if (CityItem != null)
                IDCity = CityItem.Result.IDCity;

            return IDCity;
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

        #endregion
    }
}
