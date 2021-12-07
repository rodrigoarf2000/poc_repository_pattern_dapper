using FastMapper.NetCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FastCore.Application.Commom
{
    public static class EntityExtension
    {
        #region Methods
        /// <summary>
        /// Mapeia uma entidade de banco de dados para uma entidade de visualização, utilizando o padrão MVVM.
        /// </summary>
        /// <typeparam name="TEntity">Modelo de banco de dados.</typeparam>
        /// <typeparam name="TEntityVm">Modelo de Visualização.</typeparam>
        /// <returns>Obtem uma entidade de visualização</returns>
        public static TEntityVm ToViewModel<TEntity, TEntityVm>(this TEntity entity)
        {
            try
            {
                var result = TypeAdapter.Adapt<TEntity, TEntityVm>(entity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Mapeia uma entidade de visualização para uma entidade modelo de banco de dados, utilizando o padrão MVVM.
        /// </summary>
        /// <typeparam name="TEntity">Modelo de banco de dados.</typeparam>
        /// <typeparam name="TEntityVm">Modelo de Visualização.</typeparam>
        /// <returns>Obtem uma entidade de banco de dados</returns>
        public static TEntity ToModelView<TEntity, TEntityVm>(this TEntityVm entity)
        {
            try
            {
                var result = TypeAdapter.Adapt<TEntityVm, TEntity>(entity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Mapeia uma coleção de entidade de banco de dados para uma coleção de entidade de visualização, utilizando o padrão MVVM.
        /// </summary>
        /// <typeparam name="TEntity">Modelo de banco de dados.</typeparam>
        /// <typeparam name="TEntityVm">Modelo de Visualização.</typeparam>
        /// <returns>Obtem uma coleção de entidade de banco de dados.</returns>
        public static List<TEntityVm> ToViewModel<TEntity, TEntityVm>(this List<TEntity> entity)
        {
            try
            {
                var result = entity.Select(x => x.ToViewModel<TEntity, TEntityVm>()).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Mapeia uma coleção de entidade de visualização para uma coleção de entidade de banco de dados, utilizando o padrão MVVM.
        /// </summary>
        /// <typeparam name="TEntity">Modelo de banco de dados.</typeparam>
        /// <typeparam name="TEntityVm">Modelo de Visualização.</typeparam>
        /// <returns>Obtem uma coleção de entidade de visualização</returns>
        public static List<TEntity> ToModelView<TEntity, TEntityVm>(this List<TEntityVm> entity)
        {
            try
            {
                var result = entity.Select(x => x.ToModelView<TEntity, TEntityVm>()).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Captaliza uma string, isto é, deixa toda primeira letra de cada palavra maiúscula.
        /// </summary>
        /// <param name="value">String a ser captalizada.</param>
        /// <returns>Retorna a string captalizada.</returns>
        public static string ToCapitalize(this string value)
        {
            if (string.IsNullOrEmpty(value)) { return value; }
            var result = value.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + value.Substring(1, value.Length - 1);
            return result;
        }

        /// <summary>
        /// Gera um hash padrao MD5 de uma string.
        /// </summary>
        /// <param name="value">String que sera gerada o hash.</param>
        /// <returns>Retorna um hash MD5.</returns>
        public static string ToCreateMd5(this string value)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(value);
                var hashBytes = md5.ComputeHash(inputBytes);
                var stringBuilder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                    stringBuilder.Append(hashBytes[i].ToString("X2"));

                return stringBuilder.ToString().ToUpper();
            }
        }

        /// <summary>
        /// Mapea uma entidade para padrão json.
        /// </summary>
        /// <param name="entity">Entitdade de a ser mapeada</param>
        /// <returns>Obtem a entidade mapeada para json.</returns>
        public static string ToJson<TEntity>(this TEntity entity)
        {
            if (entity == null) return null;
            var json = JsonConvert.SerializeObject(entity);
            return json;
        }

        /// <summary>
        /// Mapeia uma coleção List<T> em DataTable.
        /// </summary>
        /// <param name="collection">Coleção List<T></param>
        /// <returns>Obtem um DataTable da coleção List<T>.</returns>
        public static DataTable ToDataTable<TEntity>(this List<TEntity> collection)
        {
            var dataTable = new DataTable();
            PropertyInfo[] propertyCollection = null;

            if (collection == null) return dataTable;

            foreach (TEntity item in collection)
            {
                if (propertyCollection == null)
                {
                    propertyCollection = item.GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in propertyCollection)
                    {
                        Type columnType = propertyInfo.PropertyType;

                        if ((columnType.IsGenericType) && (columnType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            columnType = columnType.GetGenericArguments()[0];
                        }

                        dataTable.Columns.Add(new DataColumn(propertyInfo.Name, columnType));
                    }
                }

                DataRow dataRow = dataTable.NewRow();

                foreach (PropertyInfo propertyInfo in propertyCollection)
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(item, null) == null ? DBNull.Value : propertyInfo.GetValue(item, null);
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        /// <summary>
        /// Verifica se o inteiro é maior que o valor informado em comparator.
        /// </summary>
        /// <param name="i">Valor inteiro source.</param>
        /// <param name="value">Valor inteiro a ser comparado.</param>
        /// <returns>Verdadeiro ou Falso.</returns>
        public static bool IsGreaterThan(this int i, int comparator)
        {
            var result = i > comparator;
            return result;
        }

        /// <summary>
        /// Efetua lógicas de manipulação das propriedades do objeto.
        /// </summary>
        /// <typeparam name="TEntity">Objeto a ser manipulado.</typeparam>
        /// <param name="item">Objeto a ser trabalhado.</param>
        /// <param name="action">Ação a ser construída.</param>
        public static void IfType<TEntity>(this object item, Action<TEntity> action) where TEntity : class
        {
            if (item is TEntity)
                action(item as TEntity);
        }

        /// <summary>
        /// Extensão que adiciona um elemento a uma coleção apenas se uma determinada condição for atendida.
        /// </summary>
        /// <typeparam name="TEntity">Objeto a ser manipulado.</typeparam>
        /// <param name="collection">Coleção a ser trabalhada.</param>
        /// <param name="predicate">Ação a ser construída</param>
        /// <param name="item">Objeto a ser adicionado.</param>
        public static void AddIf<TEntity>(this ICollection<TEntity> collection, Func<bool> predicate, TEntity item)
        {
            if (predicate.Invoke())
                collection.Add(item);
        }

        /// <summary>
        /// Extensão que adiciona um elemento a uma coleção apenas se uma determinada condição for atendida.
        /// </summary>
        /// <typeparam name="TEntity">Objeto a ser manipulado.</typeparam>
        /// <param name="collection">Coleção a ser trabalhada.</param>
        /// <param name="predicate">Ação a ser construída</param>
        /// <param name="item">Objeto a ser adicionado.</param>
        public static void AddIf<TEntity>(this ICollection<TEntity> collection, Func<TEntity, bool> predicate, TEntity item)
        {
            if (predicate.Invoke(item))
                collection.Add(item);
        }

        /// <summary>
        /// Adiciona dois espaços insere o proximo valor e quebra linha. 
        /// </summary>
        /// <param name="stringBuilder">Objeto com o texto a ser conctenado.</param>
        /// <param name="value">Valor a ser concatenado.</param>
        public static void AppendLineWithTwoWhiteSpacePrefix(this StringBuilder stringBuilder, string value)
        {
            stringBuilder.AppendFormat($"{0}{1}{2}", "  ", value, Environment.NewLine);
        }

        /// <summary>
        /// Adiciona dois espaços e quebra linha.
        /// </summary>
        /// <param name="stringBuilder">Objeto com o texto a ser conctenado.</param>
        public static void AppendLineWithTwoWhiteSpacePrefix(this StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat($"{0}{1}", "  ", Environment.NewLine);
        }

        /// <summary>
        /// Obtem a descrição de um enumerador.
        /// </summary>
        /// <param name="value">Enumerador</param>
        /// <returns>Retorna a descrição de um enumerador.</returns>
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            return displayAttribute?.Name ?? value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Não encontrado.", nameof(description));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetGroupName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            return displayAttribute?.GroupName ?? string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, int>> ToKeyValuePair<T>(this Enum value) where T : IComparable
        {
            var dimensionType = new List<KeyValuePair<string, int>>();

            foreach (var type in Enum.GetValues(typeof(T)))
            {
                dimensionType.Add(new KeyValuePair<string, int>((type as Enum).GetDescription(), (int)type));
            }

            return dimensionType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, int>> ToKeyValuePair<T>(this Enum value, string groupName) where T : IComparable
        {
            var dimensionType = new List<KeyValuePair<string, int>>();

            foreach (var type in Enum.GetValues(typeof(T)))
            {
                var typeValue = (type as Enum);

                if (typeValue.GetGroupName().Equals(groupName))
                {
                    dimensionType.Add(new KeyValuePair<string, int>(typeValue.GetDescription(), (int)type));
                }
            }

            return dimensionType;
        }
        #endregion
    }
}
