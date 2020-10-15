using System;
using System.IO;
using GDipSA51_Team5.Models;

namespace GDipSA51_Team5.Data
{
    public class SeedData
    {
        private readonly Team5_Db db;

        public SeedData(Team5_Db db)
        {
            this.db = db;
        }

        public void Init()
        {
            AddUsers();
            AddProducts();
        }

        protected void AddUsers()
        {
            string[] lines = File.ReadAllLines("./Data/Users.data");

            foreach (string line in lines)
            {
                string[] row = line.Split(";");
                db.Users.Add(new User
                {
                    //UserId = Convert.ToInt32(row[0]),
                    Username = row[1],
                    Password = row[2],
                });
            }

            db.SaveChanges();
        }

        protected void AddProducts()
        {
            string[] lines = File.ReadAllLines("./Data/Products.data");

            foreach (string line in lines)
            {
                string[] row = line.Split(";");
                db.Products.Add(new Product
                {
                    //ProductId = Convert.ToInt32(row[0]),
                    Name = row[1],
                    Description = row[2],                    Price = Convert.ToDouble(row[3]),
                    Url = row[4],
                });
            }

            db.SaveChanges();
        }
    }
}
