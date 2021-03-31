﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace RKSoftware.DAL.EntityFramework.EFExtensions
{
    /// <summary>
    /// This class contains Extensions method that can be used to locate Key Values for particular Entity on Context
    /// </summary>
    public static class EFKeyLocationExtensions
    {
        /// <summary>
        /// Obtain Primary Key values for particular entity
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="dbContext">DB context to locate Key Values for particular entity</param>
        /// <param name="entity">Entity to obtain </param>
        /// <returns></returns>
        public static IEnumerable<object> FindPrimaryKeyValues<T>(this DbContext dbContext, T entity)
        {
            return from p in dbContext.FindPrimaryKeyProperties<T>()
                   select entity.GetPropertyValue(p.Name);
        }

        private static IReadOnlyList<IProperty> FindPrimaryKeyProperties<T>(this DbContext dbContext)
        {
            return dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey()?.Properties ?? new List<IProperty>();
        }

        private static object GetPropertyValue<T>(this T entity, string name)
        {
            return entity.GetType().GetProperty(name).GetValue(entity, null);
        }
    }
}
