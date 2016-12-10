﻿/*!
* StoryCLM.SDK Library v0.5.0
* Copyright(c) 2016, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using StoryCLM.SDK;
using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
   public class Tables
    {
        [Fact]
        public async void GetTables()
        {
            SCLM sclm = SCLM.Instance;
            IEnumerable<ApiTable> tables = await sclm.GetTablesAsync(1);
            ApiTable table = tables.FirstOrDefault(t => t.Name.Contains("Profile"));
            Assert.True(tables.Count() > 0);
            Assert.NotNull(table);
        }

        [Theory]
        [InlineData(6)]
        public async void Insert(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            Profile profile = await sclm.InsertAsync<Profile>(tableId, CreateProfile());
            Assert.NotNull(profile);
            List<Profile> profiles = new List<Profile>(await sclm.InsertAsync<Profile>(tableId, CreateProfiles()));
            Assert.NotNull(profiles);
            Assert.True(profiles.Count() == 3);
        }

        [Theory]
        [InlineData(6)]
        public async void Update(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            Profile profile = await sclm.InsertAsync<Profile>(tableId, CreateProfile());
            List<Profile> profiles = new List<Profile>(await sclm.InsertAsync<Profile>(tableId, CreateProfiles()));

            Assert.NotNull(profile);
            Assert.NotNull(profiles);
            Assert.True(profiles.Count() == 3);

            Profile updatedProfile = await sclm.UpdateAsync<Profile>(tableId, UpdateProfile(profile));
            List<Profile> updatedProfiles = new List<Profile>(await sclm.UpdateAsync<Profile>(tableId, UpdateProfiles(profiles)));

            Assert.NotNull(updatedProfile);
            Assert.NotNull(updatedProfiles);
            Assert.True(updatedProfiles.Count() == 3);
            Assert.False(profile.Equals(updatedProfile));
        }

        [Theory]
        [InlineData(6)]
        public async void Count(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            long count = await sclm.CountAsync(tableId);
            Assert.True(count > 0);
            count = await sclm.LogCountAsync(tableId, (DateTime.Now.AddDays(-25)));
            Assert.True(count > 0);
            count = await sclm.CountAsync(tableId, "[age][gt][30]");
            Assert.True(count > 0);
        }

        [Theory]
        [InlineData(6)]
        public async void Find(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            Profile profile = await sclm.InsertAsync<Profile>(tableId, CreateProfile());
            profile = await sclm.InsertAsync<Profile>(tableId, CreateProfile1());
            profile = await sclm.InsertAsync<Profile>(tableId, CreateProfile2());
            profile = await sclm.InsertAsync<Profile>(tableId, CreateProfile3());
            profile = await sclm.InsertAsync<Profile>(tableId, CreateProfile4());
            List<Profile> profiles = new List<Profile>(await sclm.InsertAsync<Profile>(tableId, CreateProfiles()));

            IEnumerable<Profile> results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            //возраст меньше или равен 30
            results = sclm.FindAsync<Profile>(tableId, "[age][lte][30]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //поле "name" начинается с символа "A"
            results = sclm.FindAsync<Profile>(tableId, "[name][sw][\"A\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, "[name][sw][\"A\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //поле "name" содержит строку "ad"
            results = sclm.FindAsync<Profile>(tableId, "[Name][cn][\"ad\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //поиск имен из списка
            results = sclm.FindAsync<Profile>(tableId, "[Name][in][\"Stanislav\",\"Tamerlan\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //Выбрать женщин, имя ("name") которых начинается со строки "V" 
            results = sclm.FindAsync<Profile>(tableId, "[gender][eq][false][and][Name][sw][\"V\"]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //Выбрать мужчин младше 30 и женщин старше 30
            results = sclm.FindAsync<Profile>(tableId, "[gender][eq][true][and][age][lt][30][or][gender][eq][false][and][age][gt][30]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            #region сложные запросы со скобочками

            //поле "name" начинается с символов "T" или "S" при этом возраст должен быть равен 22
            results = sclm.FindAsync<Profile>(tableId, "([name][sw][\"S\"][or][name][sw][\"T\"])[and][age][eq][22]", "age", 1, 0, 100).Result;
            Assert.True(results.Count() > 0);

            //Выбрать всех с возрастом НЕ в интервале [25,30] и с именами на "S" и "Т"
            results = sclm.FindAsync<Profile>(tableId, "([age][lt][22][or][age][gt][30])[and]([name][sw][\"S\"][or][name][sw][\"T\"])", "age", 1, 0, 100).Result;
           // Assert.True(results.Count() > 0);

            #endregion

            //Получить запись по идентификатору
            profile = await sclm.FindAsync<Profile>(tableId, profile._id);
            Assert.NotNull(profile);

            ////Получить коллекцию записей по списку идентификаторов
            results = await sclm.FindAsync<Profile>(tableId, profiles.Select(t => t._id).ToArray());
            Assert.True(results.Count() > 0);
        }

        [Theory]
        [InlineData(6)]
        public async void Delete(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            IEnumerable<Profile> results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            ApiLog deleteResult = sclm.DeleteAsync(tableId, results.First()._id).Result;
            Assert.NotNull(deleteResult);

            results = sclm.FindAsync<Profile>(tableId, 0, 100).Result;
            Assert.True(results.Count() > 0);

            IEnumerable<ApiLog> deleteResults = sclm.DeleteAsync(tableId, results.Select(t=> t._id)).Result;
            Assert.True(deleteResults.Count() > 0);
        }

        [Theory]
        [InlineData(6)]
        public async void Full(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            IEnumerable<Profile> results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);

            results = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
            Assert.True(results.Count() > 0);
        }

        #region TestData

        static IEnumerable<Profile> UpdateProfiles(IEnumerable<Profile> o)
        {
            return o.Select(t => UpdateProfile(t));
        }

        /// <summary>
        /// Обновляет профиль
        /// </summary>
        /// <param name="o">Исходный профиль</param>
        /// <returns>Обновленный профиль</returns>
        static Profile UpdateProfile(Profile o)
        {
            return new Profile()
            {
                _id = o._id,
                Name = "Anna",
                Age = 33,
                Gender = false,
                Rating = 3.3D,
                Created = DateTime.Now
            };
        }

        /// <summary>
        /// Создает коллекцию профилей
        /// </summary>
        /// <returns>Коллекция профилей</returns>
        static IEnumerable<Profile> CreateProfiles()
        {
            List<Profile> result = new List<Profile>();
            for (int i = 0; i < 3; i++)
                result.Add(CreateProfile());
            return result;
        }

        /// <summary>
        /// Создает новый профимль
        /// </summary>
        /// <returns></returns>
        static Profile CreateProfile()
        {
            Profile test = new Profile()
            {
                Name = "Vladimir",
                Age = 22,
                Gender = true,
                Rating = 2.2D,
                Created = DateTime.Now
            };
            return test;
        }

        /// <summary>
        /// Создает новый профиль
        /// </summary>
        /// <returns>Профиль</returns>
        static Profile CreateProfile1()
        {
            Profile test = new Profile()
            {
                Name = "Valentina",
                Age = 22,
                Gender = false,
                Rating = 2.2D,
                Created = DateTime.Now
            };
            return test;
        }

        /// <summary>
        /// Создает новый профиль
        /// </summary>
        /// <returns>Профиль</returns>
        static Profile CreateProfile2()
        {
            Profile test = new Profile()
            {
                Name = "Vladimir",
                Age = 28,
                Gender = true,
                Rating = 2.2D,
                Created = DateTime.Now
            };
            return test;
        }
        /// <summary>
        /// Создает новый профиль
        /// </summary>
        /// <returns>Профиль</returns>
        static Profile CreateProfile3()
        {
            Profile test = new Profile()
            {
                Name = "Stanislav",
                Age = 22,
                Gender = true,
                Rating = 2.2D,
                Created = DateTime.Now
            };
            return test;
        }

        static Profile CreateProfile4()
        {
            Profile test = new Profile()
            {
                Name = "Tamerlan",
                Age = 22,
                Gender = true,
                Rating = 2.2D,
                Created = DateTime.Now
            };
            return test;
        }

        /// <summary>
        /// Профиль пользователя
        /// </summary>
        public class Profile
        {
            /// <summary>
            /// Идендификатор записи
            /// Зависит от провайдера таблиц
            /// </summary>
            public string _id { get; set; }

            /// <summary>
            /// Имя пользователя
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Возраст
            /// </summary>
            public long Age { get; set; }

            /// <summary>
            /// Пол
            /// </summary>
            public bool Gender { get; set; }

            /// <summary>
            /// Рейтинг
            /// </summary>
            public double Rating { get; set; }

            /// <summary>
            /// Дата регистрации
            /// </summary>
            public DateTime Created { get; set; }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as Profile);
            }

            public bool Equals(Profile p)
            {
                if (Object.ReferenceEquals(p, null)) return false;
                if (Object.ReferenceEquals(this, p)) return true;
                if (this.GetType() != p.GetType()) return false;
                return (_id == p._id) && (Name == p.Name) && (Age == p.Age) && (Gender == p.Gender) && (Rating == p.Rating);
            }

        }

        #endregion
    }
    
}