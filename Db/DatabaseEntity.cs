﻿namespace Locator.Db;

internal class DatabaseEntity
{
    public int DatabaseId { get; set; }

    public string DatabaseName { get; set; }

    public string DatabaseUser { get; set; }

    public int DatabaseServerId { get; set; }

    public byte DatabaseTypeId { get; set; }

    public byte DatabaseStatusId { get; set; }

    public virtual ICollection<ConnectionEntity> Connections { get; set; } = [];

    public virtual DatabaseServerEntity DatabaseServer { get; set; }

    public virtual DatabaseTypeEntity DatabaseType { get; set; }
}
