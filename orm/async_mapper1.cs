using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace orm
{
	public class async_mapper1
	{
		//static string path = "../../../../objekty/bin/Debug/net6.0/objekty.dll";
		string fullpath;

		string connection_string;

		Assembly assembly;

		/// <summary>
		/// Creates a mapper for an assembly on a given path to a database with a given connection string
		/// </summary>
		/// <param name="path">path to the assembly</param>
		/// <param name="connection_string">connection string to the database</param>
		public async_mapper1(string path, string connection_string)
		{
			this.fullpath = Path.GetFullPath(path);
			this.connection_string = connection_string;
			this.assembly = Assembly.LoadFrom(fullpath);
		}

		public bool test()
		{

			var types = assembly.GetTypes();
			foreach (var type in types)
			{
				Console.WriteLine(type.Name);
				var props = type.GetProperties();
				foreach (var prop in props)
				{
					Console.WriteLine("\t" + prop.Name + " -> " + prop.PropertyType);
				}
			}
			//var assembly = Assembly.GetAssembly(typeof(objekty.user));
			try
			{
				using (SqliteConnection conn = new SqliteConnection(this.connection_string))
				{
					conn.Open();
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public async void create_tables()
		{
			using (SqliteConnection conn = new SqliteConnection(connection_string))
			{
				await conn.OpenAsync();
				var types = assembly.GetTypes();
				foreach (var type in types)
				{
					if (type.Name != type.Name.ToLower())
					{
						continue;
					}
					Console.WriteLine(type.Name);
					using SqliteCommand comm = conn.CreateCommand();
					comm.CommandText = "CREATE TABLE IF NOT EXISTS " + type.Name + " (";
					var properties = type.GetProperties();
					List<string> columns = new List<string>();

					List<string> keys = new List<string>();

					foreach (var prop in properties)
					{

						string column = prop.Name;

						Type prop_type = prop.PropertyType;

						if (prop_type == typeof(string))
						{
							column += " TEXT";
						}
						if (prop_type == typeof(int))
						{
							column += " INTEGER";
						}
						if (prop_type == typeof(DateTime))
						{
							column += " INTEGER";
						}

						if (prop.Name.EndsWith("ID"))
						{
							keys.Add(prop.Name);
						}

						columns.Add(column);
					}
					columns.Add("PRIMARY KEY (" + String.Join(", ", keys.ToArray()) + ")");
					string cols = String.Join(", ", columns.ToArray());
					comm.CommandText += cols + ");";
					Console.WriteLine(comm.CommandText);
					await comm.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task<bool> create(object to_create)
		{
			if (to_create is null) return false;
			using (SqliteConnection conn = new SqliteConnection(connection_string))
			{
				await conn.OpenAsync();

				using SqliteCommand cmd = conn.CreateCommand();
				Type obj_type = to_create.GetType();
				var properties = obj_type.GetProperties();
				List<string> columns = new List<string>();
				List<object> values = new List<object>();

				foreach (var prop in properties)
				{
					columns.Add(prop.Name);

					object? value = prop.GetValue(to_create);
					if (value is null) { continue; }

					Type prop_type = prop.PropertyType;

					if (prop_type == typeof(string))
					{
						values.Add(value);
					}
					if (prop_type == typeof(int))
					{
						values.Add(value);
					}
					if (prop_type == typeof(DateTime))
					{
						int converted = helpers.date_to_unix((DateTime)value);
						values.Add(converted);
					}
				}
				cmd.CommandText = "INSERT INTO " + obj_type.Name + "(";
				cmd.CommandText += String.Join(", ", columns.ToArray());
				cmd.CommandText += ") VALUES(@";
				cmd.CommandText += String.Join(", @", columns.ToArray());
				cmd.CommandText += ");";
				Console.WriteLine(cmd.CommandText);
				for (int i = 0; i < values.Count; i++)
				{
					Console.WriteLine("Param: @" + columns[i]);
					Console.WriteLine("Value: " + values[i]);
					cmd.Parameters.AddWithValue("@" + columns[i], values[i]);
				}
				Console.WriteLine(cmd.CommandText);
				await cmd.ExecuteNonQueryAsync();
			}
			//todo
			return true;
		}

		public async Task<bool> update(object to_update)
		{
			//todo
			Type obj_type = to_update.GetType();
			var properties = obj_type.GetProperties();

			using (SqliteConnection conn = new SqliteConnection(connection_string))
			{
				await conn.OpenAsync();
				using SqliteCommand cmd = conn.CreateCommand();

				cmd.CommandText = "UPDATE " + obj_type.Name + " SET ";

				List<string> keys = new List<string>();

				List<string> columns = new List<string>();
				List<object> values = new List<object>();

				foreach (var prop in properties)
				{
					string column = prop.Name;
					string column_asign = column + " = @" + column;

					Type prop_type = prop.PropertyType;

					object? value = prop.GetValue(to_update);
					if (value is null) { continue; }

					if (prop.Name.EndsWith("ID"))
					{
						keys.Add(column_asign);
						values.Add(value);
						continue;
					}
					columns.Add(column_asign);
					values.Add(value);
				}
				cmd.CommandText += String.Join(", ", columns.ToArray()) + " WHERE ";
				cmd.CommandText += String.Join(" AND ", keys.ToArray()) + ";";
				for (int i = 0; i < properties.Count(); i++)
				{
					Console.WriteLine("Param: @" + properties[i].Name);
					Console.WriteLine("Value: " + values[i]);
					if (values[i].GetType() == typeof(DateTime))
					{
						values[i] = helpers.date_to_unix((DateTime)values[i]);
						Console.WriteLine("Converted Value: " + values[i]);
					}
					cmd.Parameters.AddWithValue("@" + properties[i].Name, values[i]);
				}
				Console.WriteLine(cmd.CommandText);
				await cmd.ExecuteNonQueryAsync();
			}

			return true;
		}

		public async Task<bool> delete(object to_delete)
		{
			Type obj_type = to_delete.GetType();
			var properties = obj_type.GetProperties();

			using (SqliteConnection conn = new SqliteConnection(connection_string))
			{
				await conn.OpenAsync();
				using SqliteCommand cmd = conn.CreateCommand();
				cmd.CommandText = "DELETE FROM " + obj_type.Name + " WHERE ";

				List<string> keys = new List<string>();
				List<string> keys_names = new List<string>();

				List<object> values = new List<object>();


				foreach (var prop in properties)
				{
					if (prop.Name.EndsWith("ID"))
					{
						string column = prop.Name;
						string column_asign = column + " = @" + column;

						keys.Add(column_asign);
						keys_names.Add(prop.Name);

						object? value = prop.GetValue(to_delete);
						if (value is null) { continue; }

						values.Add(value);
						continue;
					}
				}
				cmd.CommandText += String.Join(" AND ", keys.ToArray()) + ";";
				for (int i = 0; i < keys_names.Count(); i++)
				{
					Console.WriteLine("Param: @" + keys_names[i]);
					Console.WriteLine("Value: " + values[i]);
					if (values[i].GetType() == typeof(DateTime))
					{
						values[i] = helpers.date_to_unix((DateTime)values[i]);
						Console.WriteLine("Converted Value: " + values[i]);
					}
					cmd.Parameters.AddWithValue("@" + keys_names[i], values[i]);
				}
				Console.WriteLine(cmd.CommandText);
				await cmd.ExecuteNonQueryAsync();
			}
			return true;
		}

		public async Task<List<object>> readall(string classname)
		{
			Console.WriteLine(classname);
			List<object> list = new List<object>();
			using (SqliteConnection conn = new SqliteConnection(connection_string))
			{
				await conn.OpenAsync();
				using SqliteCommand cmd = conn.CreateCommand();
				var all_types = assembly.GetTypes();
				Type obj_type = all_types.First(t => t.Name == classname);
				var properties = obj_type.GetProperties();
				cmd.CommandText = "SELECT * FROM " + classname;

				using SqliteDataReader reader = await cmd.ExecuteReaderAsync();
				while (reader.Read()) //go through rows
				{
					List<object> params_for_object = new List<object>();
					foreach (var prop in properties) //go through columns
					{
						Type prop_type = prop.PropertyType;

						if (prop_type == typeof(string))
						{
							string got_string = reader.GetString(reader.GetOrdinal(prop.Name));
							params_for_object.Add(got_string);
							continue;
							//Console.WriteLine(got_string);
						}
						if (prop_type == typeof(int))
						{
							int got_number = (int)reader.GetDecimal(reader.GetOrdinal(prop.Name));
							params_for_object.Add(got_number);
							continue;
							//Console.WriteLine(got_number);
						}
						if (prop_type == typeof(DateTime))
						{
							DateTime got_date = helpers.unix_to_date((int)reader.GetDecimal(reader.GetOrdinal(prop.Name)));
							params_for_object.Add(got_date);
							continue;
							//Console.WriteLine(got_date);
						}
					}
					object? new_object = Activator.CreateInstance(obj_type, params_for_object.ToArray());
					if (new_object != null)
					{
						list.Add(new_object);
					}
					//Console.WriteLine("---------------- end of row --------------------");
				}
			}
			return list;
		}
	}
}
