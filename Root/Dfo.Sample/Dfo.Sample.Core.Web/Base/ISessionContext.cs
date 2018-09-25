﻿namespace Dfo.Sample.Core.Web.Base
{
    /// <summary>
    /// Provide the common component across multi layers in application
    /// </summary>
    public interface ISessionContext
    {
        string TenantId { get; set; }
    }
}