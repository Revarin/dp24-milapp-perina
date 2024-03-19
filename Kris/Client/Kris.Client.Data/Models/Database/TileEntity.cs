﻿using SQLite;
using SQLitePCL;

namespace Kris.Client.Data.Models.Database;

[Table("tiles")]
public sealed class TileEntity
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public int S { get; set; }
    public sqlite3_blob Image { get; set; }
}
