using System.Collections.Generic;
using System.Linq;
using DotnetcliWebApi.Entities;
using DotnetcliWebApi.Models;

namespace DotnetcliWebApi.Repositories
{
    public interface IFoodRepository
    {
        FoodItem GetSingle(int id);
        void Add(FoodItem item);
        void Delete(int id);
        FoodItem Update(int id, FoodItem item);
        IQueryable<FoodItem> GetAll(QueryParameters queryParameters);

        ICollection<FoodItem> GetRandomMeal();
        int Count();

        bool Save();
    }
}
