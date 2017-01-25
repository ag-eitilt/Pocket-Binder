﻿using SQLite;
using Xamarin.Forms;

namespace Database {
	/// <summary>
	/// Encapsulates database-management code dependant on the underlying platform.
	/// </summary>
	public abstract class LocalDatabaseHelper {
		/// <summary>
		/// Retrieves the file path of the given database, specific to the system.
		/// </summary>
		/// <remarks>
		/// Later sections on <https://developer.xamarin.com/guides/xamarin-forms/working-with/databases/>
		/// contain example implementations for various platforms.
		/// </remarks>
		/// <param name="name">
		/// The underlying filename of the database, without any path or extension.
		/// </param>
		/// <returns>The complete file path of the database.</returns>
		public abstract string GetLocalDatabasePath(string name);

		/// <summary>
		/// Decorates the database name with the local path as long as it's not
		/// intended to be saved in-memory, in which case <paramref name="name"/>
		/// is simply returned unchanged.
		/// </summary>
		/// <param name="name">
		/// The underlying filename of the database, without any path or extension.
		/// </param>
		/// <returns>The complete file path of the database.</returns>
		/// <seealso cref="GetLocalDatabasePath(string)"/>
		public static string MemoryOrLocalPath(string name) {
			if (name.Equals(":memory:") || name.StartsWith("file::memory:") || name.Contains("mode=memory"))
				return name;
			else
				return DependencyService.Get<LocalDatabaseHelper>().GetLocalDatabasePath(name);
		}
	}

	/// <summary>
	/// Represents an open connection to a SQLite database, delegating file path
	/// selection to platform-specific code.
	/// </summary>
	/// <seealso cref="LocalDatabaseAsync"/>
	public class LocalDatabase : SQLiteConnection {
		/// <summary>
		/// Opens or creates a SQLite database at a system path generated from the
		/// given name.
		/// </summary>
		/// <param name="name">
		/// The underlying filename of the database, without any path or extension.
		/// </param>
		/// <seealso cref="LocalDatabaseHelper.MemoryOrLocalPath(string)"/>
		public LocalDatabase(string name)
			: this(name, (SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create)) { }

		/// <summary>
		/// Opens a SQLite database at a system path generated from the given name.
		/// </summary>
		/// <param name="name">
		/// The underlying filename of the database, without any path or extension.
		/// </param>
		/// <param name="openFlags">
		/// Bit array specifying the nature of the opened connection.
		/// </param>
		/// <seealso cref="LocalDatabaseHelper.MemoryOrLocalPath(string)"/>
		public LocalDatabase(string name, SQLiteOpenFlags openFlags)
			: base(LocalDatabaseHelper.MemoryOrLocalPath(name), openFlags, true) { }
	}

	/// <summary>
	/// Represents an open connection to a SQLite database, running I/O in the
	/// background and delegating file path selection to platform-specific code.
	/// </summary>
	/// <seealso cref="LocalDatabase"/>
	public class LocalDatabaseAsync : SQLiteAsyncConnection {
		/// <summary>
		/// Opens or creates a SQLite database at a system path generated from the
		/// given name.
		/// </summary>
		/// <param name="name">
		/// The underlying filename of the database, without any path or extension.
		/// </param>
		/// <seealso cref="LocalDatabaseHelper.MemoryOrLocalPath(string)"/>
		public LocalDatabaseAsync(string name)
			: this(name, (SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create)) { }

		/// <summary>
		/// Opens a SQLite database at a system path generated from the given name.
		/// </summary>
		/// <param name="name">
		/// The underlying filename of the database, without any path or extension.
		/// </param>
		/// <param name="openFlags">
		/// Bit array specifying the nature of the opened connection.
		/// </param>
		/// <seealso cref="LocalDatabaseHelper.MemoryOrLocalPath(string)"/>
		public LocalDatabaseAsync(string name, SQLiteOpenFlags openFlags)
			: base(LocalDatabaseHelper.MemoryOrLocalPath(name), openFlags, true) { }
	}
}