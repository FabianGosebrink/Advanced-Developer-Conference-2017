using System.Threading.Tasks;
using DotnetcliWebApi.Dtos;
using DotnetcliWebApi.Entities;
using Microsoft.AspNetCore.SignalR;

namespace DotnetcliWebApi.Hubs
{
    public class FoodHub : Hub
    {
        public Task FoodAdded(FoodItemDto foodItem)
        {
            return Clients.All.InvokeAsync("foodAdded", foodItem);
        }
    }
}