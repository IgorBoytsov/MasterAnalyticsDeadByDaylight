// See https://aka.ms/new-console-template for more information
using DBDAnalytics.Infrastructure.Context;

var context = new DBDContext();

var killers = context.Killers.Skip(1).FirstOrDefault().KillerAddons.ToList();


foreach (var item in killers)
{
    Console.WriteLine(item.AddonName);
}

Console.WriteLine("Hello, World!");


