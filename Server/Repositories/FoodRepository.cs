using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DotnetcliWebApi.Entities;

namespace DotnetcliWebApi.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly ConcurrentDictionary<int, FoodItem> _storage = new ConcurrentDictionary<int, FoodItem>();

        public FoodItem GetSingle(int id)
        {
            FoodItem foodItem;
            return _storage.TryGetValue(id, out foodItem) ? foodItem : null;
        }

        public void Add(FoodItem item)
        {
            item.Id = !GetAll().Any() ? 1 : GetAll().Max(x => x.Id) + 1;

            if (!_storage.TryAdd(item.Id, item))
            {
                throw new Exception("Item could not be added");
            }
        }

        public void Delete(int id)
        {
            FoodItem foodItem;
            if (!_storage.TryRemove(id, out foodItem))
            {
                throw new Exception("Item could not be removed");
            }
        }

        public FoodItem Update(int id, FoodItem item)
        {
            _storage.TryUpdate(id, item, GetSingle(id));
            return item;
        }

        public ICollection<FoodItem> GetAll()
        {
            return _storage.Values;
        }

        public int Count()
        {
            return _storage.Count;
        }

        public bool Save()
        {
            // To keep interface consistent with Controllers, Tests & EF Interfaces
            return true;
        }
    }
}